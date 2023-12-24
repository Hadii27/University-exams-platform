using E_Exam.Data;
using E_Exam.Dto;
using E_Exam.Migrations;
using E_Exam.Models;
using E_Exam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;

namespace E_Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;
        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [HttpGet("College")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> College()
        {
            var admin = _adminServices.GetCurrentAdmin();
            var result = await _adminServices.College(admin);
            return Ok(result);
        }

        [HttpPost("College/CreateDepartment")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartments( DepartmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var current = _adminServices.GetCurrentAdmin();

            if (dto.Name.IsNullOrEmpty() || dto.Description.IsNullOrEmpty())
                return BadRequest("One or more required fields are missing.");
            

            var departments = await _adminServices.GetAllDepartments(current);
            if (departments.Any(d => d.Name == dto.Name))
                return BadRequest("This department is already exist");
            if (departments is null)
            {
                return BadRequest("You cannot modify this faculty");
            }

            var departmentModel = new Departments
            {
                Name = dto.Name,
                Description = dto.Description,
            };

            var result = await _adminServices.AddDepartmentToFaculty(current, departmentModel);
            if (result != null)
                return Ok(result);

            else
                return BadRequest("U donnot have permission to update this College");
        }

        [HttpPost("College/Department/{DepartmentID}/AddSubject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSubject(SubjectDto dto, int DepartmentID)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var department = await _adminServices.GetDepartmentByID(DepartmentID);

            if (department == null)
                return NotFound("Invalid Department id");

            if (dto.Name.IsNullOrEmpty() || dto.Description.IsNullOrEmpty())            
                return BadRequest("One or more required fields are missing.");            

            var Subjects = await _adminServices.GetAllSubjects();

            if (Subjects.Any(s => s.Name == dto.Name))
                return BadRequest("This subject is already exist");

            var subject = new SubjectModel
            {
                Name = dto.Name,
                Description = dto.Description,
                Grade = dto.Grade,
            };
           
            var result = await _adminServices.AddSubject(subject, DepartmentID);
            if (result == null) 
                return BadRequest("Cannot add a subject");
            return Ok(result);
        }

        [HttpPost("College/Subjects/{SubID}/Prof/{UserID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignSubjectToLecturer(string UserID, int SubID)
        {
            var lecturer = await _adminServices.GetUserByID(UserID);
            var Sub = await _adminServices.GetSubjectByID(SubID);

            if (lecturer == null || Sub == null)            
                return NotFound("Invalid Lecturer ID or Subject ID ");

            bool isAssigned = await _adminServices.IsLecturerAssignedToSubject(UserID, SubID);
            if (isAssigned)            
                return BadRequest("Lecturer is already assigned to this subject.");            

            var result = await _adminServices.AssignLecturerSubject(UserID, SubID);
            if (result is null)
                return BadRequest("This User doesnot have a teacher role");
            return Ok(result);
        }

        [HttpGet("College/Lecturers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLecturers()
        {
            var result = await _adminServices.GetLecturers();
            return Ok(result);
        }

        [HttpDelete("College/Lecturers/DeleteLecturer/{UserID}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLecturer(string UserID)
        {
            var lecturer = await _adminServices.GetUserByID(UserID);
            if (lecturer == null)            
                return NotFound("Invalid Lecturer");
            var result = await _adminServices.DeleteLecturer(UserID);
            return Ok(result);
        }

        [HttpGet("College/Subjects")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GettAllSubjects()
        {
            var result = await _adminServices.GetAllSubjects();
            return Ok(result);
        }

        [HttpGet("College/Departments")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllDepartments()
        {

            var currentUser = _adminServices.GetCurrentAdmin();
            if (currentUser == null)
                return Unauthorized("Unauthorized");

            var result = await _adminServices.GetAllDepartments(currentUser);

            return Ok(result);
        }

        [HttpGet("College/Requests")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRegisterRequests()
        {
            var requests = await _adminServices.GetRequests();
            if (requests is null)
                return NotFound("No Requests");
            return Ok(requests);
        }

        [HttpPost("College/Requests/AssignStudent/{studentID}/InternationalID{InternationalID}/College{CollegeID}/Department{DepartmentID}/Grade{Level}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignUserToUser(string studentID, int InternationalID, int CollegeID, int DepartmentID, int Level)
        {
            var DeptID = await _adminServices.GetDepartmentByID(DepartmentID);
            var user = await _adminServices.GetUserByID(studentID);
            if (user is null || DeptID is null)
                return NotFound("Invalid UserID Or DepartmentID");

            var result = await _adminServices.AssignStudent(studentID, InternationalID, CollegeID, DepartmentID, Level);
            if (result is null)
                return BadRequest("International ID is already exist");
            return Ok(result);
        }
    }
}
