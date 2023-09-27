using E_Exam.Data;
using E_Exam.Dto;
using E_Exam.Migrations;
using E_Exam.Models;
using E_Exam.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("AddFaculty")]
        public async Task<IActionResult> CreateFaculty(FacultyDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto.Name is null || dto.Description is null)
                return BadRequest("One or more required fields are missing.");

            var facultiess = await _adminServices.GettAllFaculties();

            if (facultiess.Any(F=> F.Name == dto.Name))
                return BadRequest("This faculty name is already exist");                
                
            var faculty = new FacultyModel
            {
                Name = dto.Name,
                Description = dto.Description,
            };
            var result = await _adminServices.AddFaculty(faculty);

            if (result != null)            
                return Ok(result);           
            else            
                return BadRequest("Failed to create faculty");           
        }

        [HttpPost("AddDepartmentsToFaculty")]
        public async Task<IActionResult> AddDepartments(int id, DepartmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Name.IsNullOrEmpty() || dto.Description.IsNullOrEmpty())
                return BadRequest("One or more required fields are missing.");

            var faculty = await _adminServices.GetFacultyByID(id);

            if (faculty == null)
                return NotFound("Faculty not found");

            var departments = await _adminServices.GetAllDepartments();
            if (departments.Any(d=>d.Name==dto.Name))
                return BadRequest("This department is already exist");

            var departmentModel = new Departments
            {
                Name = dto.Name,
                Description = dto.Description,
            };

            var result = await _adminServices.AddDepartmentToFaculty(id, departmentModel);
            if (result != null)
                return Ok(result);

            else
                return BadRequest("Failed to add department to faculty");            
        }

        [HttpPost("AddSubject")]
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

        [HttpPost("AssignSubjectToLecturer")]
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
            return Ok(result);
        }

        [HttpDelete("DeleteLecturer")]
        public async Task<IActionResult> DeleteLecturer(string UserID)
        {
            var lecturer = await _adminServices.GetUserByID(UserID);
            if (lecturer == null)            
                return NotFound("Invalid Lecturer");
            var result = await _adminServices.DeleteLecturer(UserID);
            return Ok(result);
        }

        [HttpGet("AllSubjects")]
        public async Task<IActionResult> GettAllSubjects()
        {
            var result = await _adminServices.GetAllSubjects();
            return Ok(result);
        }

        [HttpGet("AllDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await _adminServices.GetAllDepartments();
            return Ok(result);
        }

        [HttpPost("AssginStudent")]
        public async Task<IActionResult> AssignUserToUser(StudentDto student)
        {
            var DepartmentID = await _adminServices.GetDepartmentByID(student.DepartmentId);
            var user = await _adminServices.GetUserByID(student.UserId);
            if (user is null || DepartmentID is null)
                return NotFound("Invalid UserID Or DepartmentID");
            var Student = new StudentModel
            {
                UserId = student.UserId,
                DepartmentId = student.DepartmentId,    
                Grade = student.Grade,
            };
            var result = await _adminServices.AssignStudent(Student);
            return Ok(result);
        }
    }
}
