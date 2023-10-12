using Azure.Core;
using E_Exam.Data;
using E_Exam.Dto;
using E_Exam.Helpers;
using E_Exam.Models;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService (UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<jwt> jwt, DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwt = jwt.Value;
            _context = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ReqRegister>> GetRequests()
        {
            var current = GetCurrentAdmin();
            var requests = await _context.reqRegisters
                .Where(r => r.status == "Pending")
                .ToListAsync();

            var result = new List<ReqRegister>();

            foreach (var request in requests)
            {
                var facultyAdmin = await _context.facultyAdmins
                    .Where(a => a.FacultyId == request.FaculityID && a.AdminID == current)
                    .FirstOrDefaultAsync();
                if (facultyAdmin != null)
                    result.Add(request);                                    
            }
            return result;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email Is Already Exist" };
            if (await _userManager.FindByNameAsync(model.Username) is not null)
                return new AuthModel { Message = "Username Is already exist" };

            var checkID = await _context.Users.Where(u => u.InternationalID == model.internationalID).FirstOrDefaultAsync();
            if (checkID is not null)
                return null;

            var role = await _context.Roles.FindAsync(model.RoleID);
            if (role == null)
                return null;

            var faculity = await _context.faculties.FindAsync(model.FaculityID);
            if (faculity == null)
                return null;

            var department = await _context.Departments.FindAsync(model.DepartmentID);
            if (department == null)
                return null;
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                InternationalID = model.internationalID,
                RequestedRole = role.Name,
                
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

            var req = new ReqRegister
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                internationalID = model.internationalID,
                roleID = role.Id,
                role = role.Name,
                status = "Pending",
                UserID = user.Id,                
                Grade = model.Grade,
                FaculityID = model.FaculityID,
                FaculityName = faculity.Name,
                DepartmentID = model.DepartmentID,
                DepartmentName = department.Name,
                              
            };
            var request = await  _context.reqRegisters.AddAsync(req);
            await _context.SaveChangesAsync();

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
                    Roles = new List<string> {""},
                }
            };
        }

        public async Task<AuthModel> GetToken(TokenRequestModel model)
        {
            var authModel = new AuthModel();
            var user =await _userManager.FindByEmailAsync(model.Email);

            var checkPending = await CheckStatus(user.Id);
            if (checkPending is not null && checkPending.status == "Pending")
            {
                authModel.Message = "Wait! Your status is Pending";
                return authModel;
            }

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

        public async Task<string> AddRole(AddRoleModel model ,string UserID, string RoleID)
        {
            var currentUserID = GetCurrentUser();
            if (currentUserID == null)            
                return "Unauthorized";
            
            var user = await _userManager.FindByIdAsync(UserID);

            var CheckMasterRole = await _context.UserRoles.Where(r => r.UserId == currentUserID && r.RoleId == "6e99b4dd-1ffa-4cd5-8536-c18f5be7476b").FirstOrDefaultAsync();
            var checkRole = await _context.Roles.FindAsync(RoleID);
            if (CheckMasterRole == null)
            {
                return "Invalid role id";
            }
            if (CheckMasterRole == null)
            {
                if (model.RoleName == "Master" || model.RoleName == "Admin")
                    return "You cannot Assign This Roles";
            }

            if (user == null || !await _roleManager.RoleExistsAsync(model.RoleName))            
                return "Invalid user or role";
            
            if (await _userManager.IsInRoleAsync(user, model.RoleName))           
                return "This user is already assigned to this role";

            var AddRole = await _userManager.AddToRoleAsync(user, model.RoleName);
            await ChangeStatusOfReq(user.InternationalID);

            return "Role Added Succesfully";
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

        public string GetCurrentUser()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");
            return userIdClaim;
        }

        public async Task<string> ChangeStatusOfReq(int internationalID)
        {
            var request = await _context.reqRegisters.FirstOrDefaultAsync(i => i.internationalID == internationalID);

            if (request == null)
                return "Request not found";

            request.status = "Success";

            _context.Update(request);
            await _context.SaveChangesAsync();

            return "Register succeeded";
        }

        public async Task<List<IdentityRole>> GetRoles()
        {
            var roles = await _roleManager.Roles
                .Where(r => r.Name == "Student" || r.Name == "Teacher")
                .ToListAsync();

            return roles;
        }

        public async Task<List<FacultyModel>> GetFaculties()
        {
            var roles = await _context.faculties
                .Include(f=> f.departments)
                .ToListAsync();
            return roles;
        }

        public string GetCurrentAdmin()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");
            return userIdClaim;
        }

        public async Task<ReqRegister> CheckStatus(string UserID)
        {
            var user = await _context.Users.FindAsync(UserID);
            var Check = await _context.reqRegisters
                .Where(i => i.UserID == UserID)
                .FirstOrDefaultAsync();
            if (Check == null)
                return null;
            return Check;                
        }
    }
}
