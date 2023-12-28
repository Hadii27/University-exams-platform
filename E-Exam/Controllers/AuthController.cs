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

        [HttpGet("Colleges")]
        public async Task<IActionResult> Faculties()
        {
            var faculties = await _authService.GetFaculties();
            return Ok(faculties);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RequestRegisterDto Dto)
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
                FaculityID = Dto.ReqCollegeID,
                DepartmentID = Dto.ReqDepartmentID,
                RoleID = Dto.Role,
                Username = Dto.Username,
                
            };
            var result = await _authService.RegisterAsync(model, Dto.ReqCollegeID, Dto.Role, Dto.ReqDepartmentID);
            if (!result.isAuthenticated)            
                return BadRequest(result.Message);

            if (result is null)
                return BadRequest("International id is already exist");
            return Ok(result);
        }

        [HttpPost("MasterRegister")]
        public async Task<IActionResult> MasterRegisterAsync([FromBody] MasterRegisterDto Dto)
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
                Username = Dto.Username,

            };

            var result = await _authService.MasterRegister(model, Dto.masterID);
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

        [HttpPost("Requests/AddRole/User/Role")]
        [Authorize(Roles = "Master,Admin")]
        public async Task<IActionResult> AddRoleAsync(AddRoleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.AddRole(dto.UserID, dto.RoleName);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);
            return Ok(result);
        }
    }
}
