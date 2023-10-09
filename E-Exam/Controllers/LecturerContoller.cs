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

        [HttpPost("SubjectID/{SubjectID}/AddExam")]
        public async Task<IActionResult> AddExam([FromBody] ExamCreationRequest request, int SubjectID)
        {
            if (request.Exam.Name.IsNullOrEmpty() || request.Exam.Description.IsNullOrEmpty())
                return BadRequest("One or more fields are missing");

            var subject = await _lecturerService.GetSubject(SubjectID);

            if (subject == null)
                return NotFound("Subject Not Found");

            var subjectName = subject.Name;
            var subjectGrade = subject.Grade;

            var exam = new Exam
            {
                Name = request.Exam.Name,
                Description = request.Exam.Description,
                SubjectId = SubjectID,
                SubjectName = subjectName,
                Grade = subjectGrade,
                questions = new List<Questions>(),
                start = request.Exam.Start, 
                end = request.Exam.End,
                duration = request.Exam.Duration,
                
            };
            
            foreach (var questionDto in request.Questions)
            {
                var ques = new Questions
                {
                    Question = questionDto.Question,
                    Score = questionDto.Score,
                    correctAnswer = questionDto.CorrectAnswer,
                    answersModels = new List<AnswersModel>() 
                    
                };

                if (request.Questions == null || request.Questions.Count == 0)
                    return BadRequest("You must provide at least one question for the exam.");

                foreach (var AnswerDto in questionDto.Answers) 
                {
                    var answer = new AnswersModel
                    {
                        Questionsid = ques.id,
                        Text = AnswerDto.Answer,
                    };

                    ques.answersModels.Add(answer);
                }

                if (ques.answersModels.Count == 0)                
                    return BadRequest("You cannot add questions with empty answers.");
                
                exam.questions.Add(ques);
                exam.QuestionsCount++;
            }

            if (request.Questions == null)
                return BadRequest("You Couldn't Add Empty exam!");
            
            var result = await _lecturerService.AddExam(exam, SubjectID, exam.questions);

            if (result is null)           
                return BadRequest("You cannot add exams to this subject");

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
