using E_Exam.Models;
using Microsoft.AspNetCore.Identity;

namespace E_Exam.Services
{
    public interface IAuthService
    {
        public Task<AuthModel> RegisterAsync(RegisterModel model);
        public Task<AuthModel> GetToken(TokenRequestModel model);
        public Task<string> AddRole(AddRoleModel model, string UserID, string RoleID);
        public Task<IEnumerable<ReqRegister>> GetRequests();
        public Task<string> ChangeStatusOfReq(int internationalID);
        public Task<List<IdentityRole>> GetRoles();

        public Task<List<FacultyModel>> GetFaculties();


    }
}
