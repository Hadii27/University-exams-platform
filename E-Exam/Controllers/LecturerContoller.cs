using E_Exam.Dto;
using E_Exam.Migrations;
using E_Exam.Models;
using E_Exam.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerContoller : ControllerBase
    {
        private readonly ILecturerService _lecturerService;
        public LecturerContoller (ILecturerService laurerService)
        {
            _lecturerService = laurerService;
        }

        [HttpPost("AddExam")]
        public async Task<IActionResult> AddExam(ExamDto examDto)
        {
            if (examDto.Name.IsNullOrEmpty() || examDto.Description.IsNullOrEmpty())            
                return BadRequest("One or more fields are missing");

            var subject = await _lecturerService.GetSubject(examDto.SubjectId);
            
            if (subject == null)            
                return NotFound("Subjetc Not Found");
            
            var subjectName = subject.Name;
            var subjecGrade = subject.Grade;

            var exam = new Exam
            {
                Name = examDto.Name,
                Description = examDto.Description,
                SubjectId = examDto.SubjectId,
                SubjectName = subjectName,
                Grade = subjecGrade,

            };
  
            var result = await _lecturerService.AddExam(exam,exam.SubjectId);
            if (result is null)
            {
                return BadRequest("You cannot add exams to this subject");
            }
            return Ok(result);
   
        }

        [HttpPost("{examId}/questions")]
        public async Task<IActionResult> AddQuestions(int examId,QuestionsDto questionsDto)
        {
            var exam = _lecturerService.GetExam(examId);
            if (exam == null)
                return NotFound("Exam ID is not found");

            if (questionsDto.Question.IsNullOrEmpty())
                return BadRequest("Question Text is required");

            var ques = new Questions
            {
                ExamID = examId,
                Question = questionsDto.Question,
                Score = questionsDto.Score,
            };

            var result = await _lecturerService.AddQuestions(ques);
            await _lecturerService.CalculteTotalScore(result.ExamID);

            return Ok(result);

        }

        [HttpPost("{examId}/questions/{questionId}/answers")]
        public async Task<IActionResult> AddAnswer(int examId, int questionId, AnswersDto answersDto)
        {
            var question = _lecturerService.GetQuestions(questionId);
            if (question == null)            
                return NotFound("Question ID is not found");
            if (answersDto.Answer.IsNullOrEmpty())           
                return BadRequest("Answer Text is required");            
            
            var answer = new AnswersModel
            {
                Questionsid = questionId,
                Text = answersDto.Answer,
                CorrectAnswer = answersDto.CorrectAnswer,
            };

            var result = await _lecturerService.AddAnswers(answer);
            return Ok(result);
        }

        [HttpGet("AllExams")]
        public async Task<IActionResult> GetAllexams()
        {
            var Exmas = await _lecturerService.GetAllexams();
            if (Exmas == null)
                return NotFound("No Exams Found");
            return Ok(Exmas);
        }


    }
}
