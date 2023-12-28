using E_Exam.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Exam.Services
{
    public interface IAuthService
    {
        public Task<AuthModel> RegisterAsync(RegisterModel model, int CollegeID, string role, int DepartmentID);
        public Task<AuthModel> MasterRegister(RegisterModel model, string MasterID);

        public Task<AuthModel> GetToken(TokenRequestModel model);
        public Task<string> AddRole(string UserID, string RoleID);
        public Task<string> ChangeStatusOfReq(int internationalID);
        public Task<List<IdentityRole>> GetRoles();

        public Task<List<FacultyModel>> GetFaculties();


    }
}
