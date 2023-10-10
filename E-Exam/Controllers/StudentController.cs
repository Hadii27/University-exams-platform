using E_Exam.Dto;
using E_Exam.Migrations;
using E_Exam.Models;
using E_Exam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;

namespace E_Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("Subjects")]
        [Authorize(Roles ="Student")]
        public async Task<IActionResult> GetSubjects()
        {
            var student = _studentService.GetCurrentStudent();
            if (student == null)            
                return Unauthorized("UnAuthorized");
            
            var result = await _studentService.GetSubjects(student);
            if (result == null)
            {
                return NotFound("Not Found");
            }
            return Ok(result);
        }

        [HttpPost("ChooseSubjects")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AddStudentSub( IEnumerable<ChoosedSubjectDto> SubIDs)
        {
            var student = _studentService.GetCurrentStudent();
            if (student is null)
            {
                return Unauthorized("Unauthorized");
            }
            var chooseSubjects = new List<ChooseSubjects>();
            foreach (var subId in SubIDs)
            {
                var s = new ChooseSubjects
                {
                    SubjectId = subId.SubID
                };
                chooseSubjects.Add(s);
            }
            var result = await _studentService.StudentsSubjects(student, chooseSubjects);
            if (result == null)
            {
                return NotFound("Subject not found");
            }
            return Ok(result);
        }

        [HttpGet("ChoosedSubjects")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetChoosedSubjects()
        {
            var student = _studentService.GetCurrentStudent();
            if (student is null)
            {
                return Unauthorized("Unauthorized");
            }
            var result = await _studentService.GetChoosedSubjects(student);
            if (result == null)
            {
                return NotFound("Student not found");
            }
            return Ok(result);
        }

        [HttpGet("subjects/{SubjectId}/ExamsName")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetExamInfo([FromRoute] int SubjectId)
        {
            var student = _studentService.GetCurrentStudent();
            if (student is null)
            {
                return Unauthorized("Unauthorized");
            }
            var result = await _studentService.GetExamInfo(student, SubjectId);
            if (result == null)
                return NotFound("No Exams Found!");
            return Ok(result);
        }

        [HttpGet("/subjects/{SubjectId}/exams/{ExamID}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetExams([FromRoute] int SubjectId, [FromRoute] int ExamID)
        {
            var student = _studentService.GetCurrentStudent();
            if (student is null)
            {
                return Unauthorized("Unauthorized");
            }
            var result = await _studentService.GetExamsOfSubject(student, SubjectId, ExamID);
            if (result == null)
                return NotFound("No Exams Found!");

            return Ok(result);
        }

        [HttpPost("Exam/{ExamID}/Answer")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> ChooseAnswer(int ExamID, [FromBody] IEnumerable<AnswersForStudentDto> AnswerID)
        {
            var student = _studentService.GetCurrentStudent();
            if (student is null)            
                return Unauthorized("Unauthorized");
            
            var Answers = new List<AnswersModel>();
            foreach (var answer in AnswerID)
            {
                var a = new AnswersModel
                {
                    Id = answer.answerID
                };
                Answers.Add(a);
            }

            var result = await _studentService.ExamSubmit(ExamID, student, Answers);
            return Ok(result);
        }

    }
}
