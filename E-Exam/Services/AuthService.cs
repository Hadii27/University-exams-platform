using E_Exam.Data;
using E_Exam.Dto;
using E_Exam.Helpers;
using E_Exam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace E_Exam.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly jwt _jwt;
        private readonly DataContext _context;

        public AuthService (UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<jwt> jwt, DataContext dataContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwt = jwt.Value;
            _context = dataContext;
        }

        public async Task<string> ReqRegister(ReqRegister model)
        {
            var existingUser = await _context.reqRegisters.FirstOrDefaultAsync(u => u.Email == model.Email || u.Username == model.Username);
            var nationalID = await _context.reqRegisters.Where(i => i.internationalID == model.internationalID).FirstOrDefaultAsync();
            if (nationalID is not null)
                return "This international ID is already exist";
            if (existingUser != null)            
                return "This Email or Username is already in use.";
            
            var Request = new ReqRegister
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Email = model.Email,
                internationalID = model.internationalID,
                PhoneNumber = model.PhoneNumber,
                role = model.role,
                status = "Pending",
            };

            await _context.reqRegisters.AddAsync(Request);
            await _context.SaveChangesAsync();
            return $"Your request has been succesfully and the request id is {Request.id}";
        }

        public async Task<IEnumerable<ReqRegister>> getRequests()
        {
            var request = await _context.reqRegisters.Where(r => r.status == "Pending").ToListAsync();
            return request;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email Is Already Exist" };
            if (await _userManager.FindByNameAsync(model.Username) is not null)
                return new AuthModel { Message = "Username Is already exist" };

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,

            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }
                return new AuthModel { Message = errors };               
            }
              
            await _userManager.AddToRoleAsync(user, "Student");

            var jwtSecurityToken = await CreateJwtTokenAsync(user);

            return new AuthModel
            {
               
                ExpireOn = jwtSecurityToken.ValidTo,
                isAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserData = new UserDataModel
                {
                    Username = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    UserId = user.Id,
                    Roles = new List<string> { "Student" },
                }
            };
        }

        public async Task<AuthModel> GetToken(TokenRequestModel model)
        {
            var authModel = new AuthModel();
            var user =await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email Or Password Is invalid";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtTokenAsync(user);
            var roleList = await _userManager.GetRolesAsync(user);
            var userData = new UserDataModel
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = roleList.ToList()

            };
            authModel.UserData = userData;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.ExpireOn = jwtSecurityToken.ValidTo;
            authModel.isAuthenticated = true;
                  
            return authModel;
        }

        public async Task<string> AddRole(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.userID);

            if (user == null || !await _roleManager.RoleExistsAsync(model.RoleName))            
                return "Invalid user or role";
            

            if (await _userManager.IsInRoleAsync(user, model.RoleName))           
                return "This user is already assigned to this role";

            var AddRole = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (AddRole.Succeeded)
            {
                var removeUserRoleResult = await _userManager.RemoveFromRoleAsync(user, "Student");
                return string.Empty;
            }

            else
                return "Failed to add role to rhis user";
        }

        public async Task<JwtSecurityToken> CreateJwtTokenAsync(ApplicationUser user)
        {
            var userclaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("UserId", user.Id.ToString()),

            }
            .Union(userclaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var JwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_jwt.DurationInHours),
                signingCredentials: signingCredentials
                );
            return JwtSecurityToken;
        }

 

    }
}
