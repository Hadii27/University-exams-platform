using E_Exam.Dto;
using E_Exam.Models;
using E_Exam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMasterService _masterService;

        public MasterController(IMasterService masterService)
        {
            _masterService = masterService;
        }

        [HttpPost("AddFaculty")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> CreateFaculty(FacultyDto dto)
        {
            var current = _masterService.GetCurrentUser();
            if (current == null)
                return Unauthorized("Unauthorized");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto.Name is null || dto.Description is null)
                return BadRequest("One or more required fields are missing.");

            var facultiess = await _masterService.GetFaculties();

            if (facultiess.Any(F => F.Name == dto.Name))
                return BadRequest("This faculty name is already exist");

            var faculty = new FacultyModel
            {
                Name = dto.Name,
                Description = dto.Description,
            };
            var result = await _masterService.AddFaculty(faculty);

            if (result == null)
                return BadRequest("Failed to create faculty");

            return Ok(result);
        }

        [HttpPost("Admins/AssignFaculty")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> AssignFacultyToAdmin(FAcultyAdminDto dto)
        {
            var current = _masterService.GetCurrentUser();
            if (current == null)
                return Unauthorized("Unauthorized");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto.AdminID is null)
                return BadRequest("One or more required fields are missing.");

            var admins = await _masterService.GetAllAssignFacultyAdmin();
            if (admins.Any(a => a.FacultyId == dto.FAcultyID && a.AdminID == dto.AdminID))
                return BadRequest("This faculty is already assigned to admin");

            var assign = new FacultyAdmin
            {
                AdminID = dto.AdminID,
                FacultyId = dto.FAcultyID,
            };

            var result = await _masterService.AssignFacultyAdmin(assign);
            if (result == null)
                return BadRequest("One or more required fields are missing. Or this user not admin");
            return Ok(result);


        }

        [HttpGet("AllFacultyWithAdmins")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> GetAllAssignedFaculity()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var current = _masterService.GetCurrentUser();
            if (current == null)
                return Unauthorized("Unauthorized");

            var result = await _masterService.GetAllAssignFacultyAdmin();
            return Ok(result);
        }

        [HttpGet("Admins")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> Admins()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var current = _masterService.GetCurrentUser();
            if (current == null)
                return Unauthorized("Unauthorized");

            var result = await _masterService.Admins();
            return Ok(result);
        }

        [HttpGet("GetFaculties")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> GetAllFaculties()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var current = _masterService.GetCurrentUser();
            if (current == null)
                return Unauthorized("Unauthorized");

            var result = await _masterService.GetFaculties();
            return Ok(result);
        }

        [HttpDelete("Admin/Delete{AdminID}")]
        public async Task<IActionResult> UnAssignAdmin(string AdminID)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var current = _masterService.GetCurrentUser();
            if (current == null)
                return Unauthorized("Unauthorized");

            var removeAdmin = await _masterService.UnAssignAdmin(AdminID);
            return Ok(removeAdmin); 
        }





    }
}
