using E_Exam.Data;
using E_Exam.Dto;
using E_Exam.Migrations;
using E_Exam.Models;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace E_Exam.Services
{
    public class StudentService : IStudentService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<SubjectDepartmentModel>> GetSubjects(string StudentID)
        {
            var student = await _context.students.Where(x => x.UserId == StudentID).FirstOrDefaultAsync();
            if (student == null)
                return null;
            var StudentGrade = student.Grade;
            var departmentOdStudent = student.DepartmentId;
            var Subjects = await _context.subjectDepartments.Where(x => x.DepartmentId == departmentOdStudent).Include(c => c.Subject).ToListAsync();
            var subject = Subjects.Where(x => x.Subject.Grade == StudentGrade);
            return subject;
        }

        public async Task<IEnumerable<ChooseSubjects>> StudentsSubjects(string StudentId, IEnumerable<int> SubIDs)
        {
            var addedSubjects = new List<ChooseSubjects>();
            foreach (int SubID in SubIDs)
            {
                var sub = await _context.subject.FindAsync(SubID);
                if (sub is null)
                {
                    return null;
                }

                var choosed = new ChooseSubjects
                {
                    SubjectId = sub.Id,
                    UserId = StudentId,
                    grade = sub.Grade,
                    
                };

                var result = await _context.chooseSubjects.AddAsync(choosed);
                addedSubjects.Add(result.Entity);
            }

            await _context.SaveChangesAsync();

            return addedSubjects;
        }

        public async Task<IEnumerable<ChooseSubjects>> GetChoosedSubjects(string studentID)
        {
            var student = await _context.students.Where(s => s.UserId == studentID).FirstOrDefaultAsync();
            if (student is null)
                return null;
            var studentGrade = student.Grade;
            var studentSub = await _context.chooseSubjects.Where(s => s.UserId == studentID && s.grade == studentGrade).ToListAsync();
            return studentSub;
        }



        public async Task<object> GetExamInfo(string studentId, int subjectId)
        {
            var student = await _context.students.FirstOrDefaultAsync(x => x.UserId == studentId);
            if (student == null)
                return null;

            var choosed = await _context.chooseSubjects.Where(s => s.UserId == studentId && s.SubjectId == subjectId).FirstOrDefaultAsync();
            if (choosed is null)
                return null;

            var exam = await _context.exams
                .Where(e => e.SubjectId == subjectId)
                .Select(e => new
                {
                    e.Id,
                    e.Name,
                    e.Description
                })
                .FirstOrDefaultAsync();

            return exam;
        }

        public async Task<IEnumerable<Exam>> GetExamsOfSubject(string StudentID, int SubjectID,int ExamID)
        {
            var student = await _context.students.FirstOrDefaultAsync(x => x.UserId == StudentID);
            if (student == null)
                return null;

            var Subject = await _context.subject.FindAsync(SubjectID);
            if (Subject == null)
                return null;

            var choosed = await _context.chooseSubjects.Where(s => s.UserId == StudentID && s.SubjectId == SubjectID).FirstOrDefaultAsync();
            if (choosed is null)
                return null;

            var exams = await _context.exams
                .Where(e => e.SubjectId == Subject.Id)
                .Include(e => e.questions)
                .ThenInclude(e => e.answersModels)
                .ToListAsync();

            var examList = exams.Select(e => new Exam
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                SubjectName = e.SubjectName,
                Grade = e.Grade, 
                QuestionsCount = e.QuestionsCount, 
                ExamScore = e.ExamScore, 
                questions = e.questions.Select(q => new Questions
                {
                    id = q.id,
                    Question = q.Question,
                    answersModels = q.answersModels.Select(a => new AnswersModel
                    {
                        Id = a.Id,
                        Text = a.Text,
                    }).ToList()
                }).ToList()
            }).ToList();

            return examList;

        }

        public async Task<Models.ChoosenAnswers> ChooseAnswer(string UserId , int examID,int QuestionID,int AnswerID)
        {
            var exam = await _context.exams.FindAsync(examID);
            if (exam == null)
                return null;

            var ques = await _context.questions.Where(q => q.ExamID == examID && q.id == QuestionID).FirstOrDefaultAsync();
            if (ques == null)
                return null;

            var answer = await _context.answers.Where(a => a.Questionsid == QuestionID && a.Id == AnswerID).FirstOrDefaultAsync();
            if (answer == null)
                return null;

            var choosen = new Models.ChoosenAnswers
            {
                userId = UserId,
                questionsId = QuestionID,
                answerId = AnswerID,
                ExamID = examID,
            };

            if (!answer.CorrectAnswer)            
                choosen.QuestionScore = 0;
            
            else            
                choosen.QuestionScore = ques.Score;
            
            _context.choosenAnswers.Add(choosen);
            await _context.SaveChangesAsync();
            return choosen;
        }

        public string GetCurrentStudent()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userIdClaim;
        }

        public async Task<IEnumerable<Exam>> GetExamByID(int examID)
        {
            var exam = await _context.exams.Where(x=>x.Id==examID).Include(c => c.questions).ThenInclude(c => c.answersModels).ToListAsync();
            return exam;
        }

    }
}
