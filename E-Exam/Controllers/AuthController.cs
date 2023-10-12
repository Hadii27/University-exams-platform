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


        [HttpGet("RequestRegister")]
        [Authorize(Roles ="Admin")]
        public async Task <IActionResult> GetRegisterRequests()
        {
            var requests = await _authService.GetRequests();
            if (requests is null)
                return NotFound("No Requests");
            return Ok(requests);
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> Roles()
        {
            var roles = await _authService.GetRoles();
            return Ok(roles);
        }

        [HttpGet("Faculties")]
        public async Task<IActionResult> Faculties()
        {
            var faculties = await _authService.GetFaculties();
            return Ok(faculties);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.RegisterAsync(model);
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

        [HttpPost("AddRole/User/{UserID}/Role/{RoleID}")]
        [Authorize(Roles = "Master,Admin")]
        public async Task<IActionResult> AddRoleAsync([FromBody]AddRoleDto model,[FromRoute] string UserID, [FromRoute] string RoleID)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var role = new AddRoleModel
            {               
                RoleName = model.RoleName,
                userID = UserID
            };
            var result = await _authService.AddRole(role, UserID, RoleID);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);
            return Ok(model);
        }
    }
}
