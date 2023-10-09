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


        [HttpPost("Req")]
        public async Task<IActionResult> RequestRegister([FromBody] RequestRegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var request = new ReqRegister
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                internationalID = model.internationalID,
                Username = model.Username,
                PhoneNumber = model.PhoneNumber,
                role = model.role
            };

            var result = await _authService.ReqRegister(request);
            return Ok(result);
        }

        [HttpGet("RequestRegister")]
        public async Task <IActionResult> GetRegisterRequests()
        {
            var requests = await _authService.getRequests();
            if (requests is null)
                return NotFound("No Requests");
            return Ok(requests);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.RegisterAsync(model);
            if (!result.isAuthenticated)            
                return BadRequest(result.Message);
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

        [HttpPost("AddRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRole(model);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);
            return Ok(model);
        }
    }
}
