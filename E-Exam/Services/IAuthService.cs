using E_Exam.Models;

namespace E_Exam.Services
{
    public interface IAuthService
    {
        public Task<AuthModel> RegisterAsync(RegisterModel model);
        public Task<AuthModel> GetToken(TokenRequestModel model);
        public Task<string> AddRole(AddRoleModel model);
    }
}
