using E_Exam.Dto;
using E_Exam.Models;
using E_Exam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController (IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> Roles()
        {
            var roles = await _authService.GetRoles();
            return Ok(roles);
        }

        [HttpGet("Colleges")]
        public async Task<IActionResult> Faculties()
        {
            var faculties = await _authService.GetFaculties();
            return Ok(faculties);
        }

        [HttpPost("College{ReqCollegeID}/Department{ReqDepartmentID}/Rule{ReqRoleID}/Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RequestRegisterDto Dto , int ReqCollegeID, string ReqRoleID, int ReqDepartmentID)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = new RegisterModel
            {
                FirstName = Dto.FirstName,
                LastName = Dto.LastName,
                Email = Dto.Email,
                PhoneNumber = Dto.PhoneNumber,
                Password = Dto.Password,
                internationalID = Dto.internationalID,
                Grade = Dto.Grade,
                FaculityID = ReqCollegeID,
                DepartmentID = ReqDepartmentID,
                RoleID = ReqRoleID,
                Username = Dto.Username,
                
            };
            var result = await _authService.RegisterAsync(model, ReqCollegeID, ReqRoleID, ReqDepartmentID);
            if (!result.isAuthenticated)            
                return BadRequest(result.Message);

            if (result is null)
                return BadRequest("International id is already exist");
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.GetToken(model);
            if (!result.isAuthenticated)
                return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpPost("Requests/AddRole/User/{UserID}/Role/{RoleID}")]
        [Authorize(Roles = "Master,Admin")]
        public async Task<IActionResult> AddRoleAsync([FromRoute] string UserID, [FromRoute] string RoleID)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.AddRole(UserID, RoleID);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);
            return Ok(result);
        }
    }
}
