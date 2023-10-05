using E_Exam.Migrations;
using E_Exam.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

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

        [HttpGet("{StudentId}/subjects")]
        public async Task<IActionResult> GetSubjects([FromRoute] string StudentId)
        {
            var result = await _studentService.GetSubjects(StudentId);
            if (result == null)
            {
                return BadRequest("sss");
            }
            return Ok(result);
        }

        [HttpPost("{StudentId}/subjects")]
        public async Task<IActionResult> AddStudentSub([FromRoute] string StudentId, IEnumerable<int> SubIDs)
        {
            var result = await _studentService.StudentsSubjects(StudentId, SubIDs);
            if (result == null)
            {
                return NotFound("Student not found");
            }
            return Ok(result);
        }

        [HttpGet("{StudentId}/ChoosedSubjects")]
        public async Task<IActionResult> GetChoosedSubjects([FromRoute] string StudentId)
        {
            var result = await _studentService.GetChoosedSubjects(StudentId);
            if (result == null)
            {
                return NotFound("Student not found");
            }
            return Ok(result);
        }


        [HttpGet("{StudentId}/subjects/{SubjectId}/ExamsName")]
        public async Task<IActionResult> GetExamInfo([FromRoute] string StudentId, [FromRoute] int SubjectId)
        {
            var result = await _studentService.GetExamInfo(StudentId, SubjectId);
            if (result == null)
                return NotFound("No Exams Found!");
            return Ok(result);
        }

        [HttpGet("{StudentId}/subjects/{SubjectId}/exams/{ExamID}")]
        public async Task<IActionResult> GetExams([FromRoute] string StudentId, [FromRoute] int SubjectId, [FromRoute] int ExamID)
        {
            var result = await _studentService.GetExamsOfSubject(StudentId, SubjectId, ExamID);
            if (result == null)
                return NotFound("No Exams Found!");

            return Ok(result);
        }


        [HttpPost("{StudentID}/Exam/{examID}/Question/{QuestionID}/Answer/{AnswerID}")]
        public async Task<IActionResult> Choose(string StudentID, int examID, int QuestionID, int AnswerID)
        {
            var result = await _studentService.ChooseAnswer(StudentID, examID,QuestionID,AnswerID);
            if (result == null)
                return NotFound("Invalid Answer");
            return Ok(result);
        }
    }
}
