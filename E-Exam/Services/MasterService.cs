using E_Exam.Data;
using E_Exam.Migrations;
using E_Exam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Exam.Services
{
    public class MasterService: IMasterService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public MasterService (DataContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<FacultyModel> AddFaculty(FacultyModel model)
        {
            var faculty = new FacultyModel
            {
                Name = model.Name,
                Description = model.Description,
            };
            _context.faculties.Add(faculty);
            await _context.SaveChangesAsync();
            return faculty;
        }

        public async Task<IEnumerable<FacultyModel>> GetFaculties()
        {
            var FacultyAdmin = await _context.faculties.ToListAsync();
            return FacultyAdmin;
        }

        public async Task<FacultyAdmin> AssignFacultyAdmin (FacultyAdmin model)
        {
            var admin = await _context.Users.FindAsync(model.AdminID);
            var faculty =await _context.faculties.FindAsync(model.FacultyId);

            if (admin is null || faculty is null)
                return null;

            var checkAdmin = await _context.UserRoles.Where(r => r.UserId == model.AdminID && r.RoleId == "e308bc06-17e7-4b98-8ae5-a4cb16e111b8").FirstOrDefaultAsync();
            if (checkAdmin is not null)
                return null;

            var FacultyAdmin = new FacultyAdmin
            {
                AdminID = model.AdminID,
                AdminName = admin.UserName,
                FacultyId = model.FacultyId,
                FacultyName = faculty.Name,
            };

            await _context.facultyAdmins.AddAsync(FacultyAdmin);
            await _context.SaveChangesAsync();
            return FacultyAdmin;
        }

        public async Task<IEnumerable<FacultyAdmin>> GetAllAssignFacultyAdmin()
        {
            var FacultyAdmin = await _context.facultyAdmins.ToListAsync();
            return FacultyAdmin;
        }

        public async Task<FacultyAdmin> GetAssignFacultyAdmin(int facultyID)
        {
            var FacultyAdmin = await _context.facultyAdmins.Where(f => f.FacultyId == facultyID).FirstOrDefaultAsync();
            if (FacultyAdmin is null)            
                return null;
            
            return FacultyAdmin;
        }
        public string GetCurrentUser()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");
            return userIdClaim;
        }

        public async Task<string> UnAssignAdmin(string userID)
        {
            var user = await _context.Users.FindAsync(userID);
            if (user is null)            
                return "Invalid user id";
            

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin)            
                return "This user is not an admin";
            

            var result = await _userManager.RemoveFromRoleAsync(user, "Admin");

            if (result.Succeeded)            
                return "Admin role removed successfully";
            
            else           
                return "Failed to remove admin role";
            
        }
    }

}
