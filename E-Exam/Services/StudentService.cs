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

        //public async Task<string> StudentsSubjects(string StudentId, IEnumerable<ChooseSubjects> SubIDs)
        //{
        //    var addedSubjects = new List<ChooseSubjects>();
        //    var subjectNames = new List<string>();

        //    foreach (var choosedSubject in SubIDs)
        //    {
        //        var sub = await _context.subject.FindAsync(choosedSubject.SubjectId);
        //        if (sub is null)
        //        {
        //            return null;
        //        }
        //        var exist = await _context.chooseSubjects.Where(u => u.UserId == StudentId && sub.Id == u.SubjectId).FirstOrDefaultAsync();
        //        if (exist is not null)
        //            return $"You have been choosed it before";

        //        var choosed = new ChooseSubjects
        //        {
        //            SubjectId = sub.Id,
        //            UserId = StudentId,
        //            grade = sub.Grade,
        //        };

        //        var result = await _context.chooseSubjects.AddAsync(choosed);
        //        addedSubjects.Add(result.Entity);

        //        subjectNames.Add(sub.Name); 
        //    }

        //    await _context.SaveChangesAsync();

        //    string subjectNamesStr = string.Join(", ", subjectNames); 

        //    return $"You have chosen the following subjects: {subjectNamesStr}";
        //}
        public async Task<string> StudentsSubjects(string StudentId, int SubID)
        {
            var subjectNames = new List<string>();
            var sub = await _context.subject.FindAsync(SubID);
            if (sub is null)
            {
                return null;
            }
            var exist = await _context.chooseSubjects.Where(u => u.UserId == StudentId && sub.Id == u.SubjectId).FirstOrDefaultAsync();
            if (exist is not null)
                return $"You have been choosed it before";

            var choosed = new ChooseSubjects
            {
                SubjectId = sub.Id,
                UserId = StudentId,
                grade = sub.Grade,
            };

            var result = await _context.chooseSubjects.AddAsync(choosed);

            subjectNames.Add(sub.Name);
        
            await _context.SaveChangesAsync();

            string subjectNamesStr = string.Join(", ", subjectNames);

            return $"You have chosen the following subjects: {subjectNamesStr}";
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
                .OrderByDescending(e => e.Id)
                .Select(e => new                
                {
                    e.Id,
                    e.Name,
                    e.Description,
                    e.start,
                    e.end,
                    e.ExamScore,
                    e.SubjectName
                })
                .FirstOrDefaultAsync();

            return exam;
        }

        public async Task<Exam> GetExamsOfSubject(string StudentID, int ExamID)
        {
            var student = await _context.students.FirstOrDefaultAsync(x => x.UserId == StudentID);
            if (student == null)
                return null;

            var Selectedexam = await _context.exams.FindAsync(ExamID);
            if (Selectedexam == null)
                return null;

            var SubjectID = Selectedexam.SubjectId;

            var Subject = await _context.subject.FindAsync(SubjectID);
            if (Subject == null)
                return null;

            var choosed = await _context.chooseSubjects.Where(s => s.UserId == StudentID && s.SubjectId == SubjectID).FirstOrDefaultAsync();
            if (choosed is null)
                return null;

            var lastExam = await _context.exams
                .Where(e => e.SubjectId == Subject.Id)
                .OrderByDescending(e => e.Id)
                .Include(e => e.questions)
                .ThenInclude(e => e.answersModels)
                .FirstOrDefaultAsync();
            if (lastExam == null)
                return null;

            var exam = new Exam
            {
                Id = lastExam.Id,
                Name = lastExam.Name,
                Description = lastExam.Description,
                SubjectName = lastExam.SubjectName,
                Grade = lastExam.Grade,
                QuestionsCount = lastExam.QuestionsCount,
                ExamScore = lastExam.ExamScore,
                questions = lastExam.questions.Select(q => new Questions
                {
                    id = q.id,
                    Question = q.Question,
                    correctAnswer = q.correctAnswer,
                    Score = q.Score,
                    answersModels = q.answersModels.Select(a => new AnswersModel
                    {
                        Id = a.Id,
                        Text = a.Text,
                        Questionsid = a.Questionsid,
                    }).ToList()
                }).ToList()
            };
            return exam;
        }

        public async Task<string> ExamSubmit(int ExamID, string UserId, IEnumerable<AnswersModel> AnswerIDs)
        {
            var exam = await _context.exams.FindAsync(ExamID);
            if (exam == null)
                return "Inavlid exam";
            var choosenAnswers = new List<Models.ChoosenAnswers>();
            int totalScore = 0;

            var CheckSubmit = await _context.submitedExams.Where(x => x.UserId == UserId && x.ExamID == ExamID).FirstOrDefaultAsync();
            if (CheckSubmit is not null)           
                return "U have been submit this exam before";
            foreach (var AnswerID in AnswerIDs)
            {
                var answer = await _context.answers.FindAsync(AnswerID.Id);
                if (answer == null)
                    return "Invalid answer";

                var ques = await _context.answers
                    .Include(q => q.Questions)
                    .Where(q => q.Questionsid == answer.Questionsid)
                    .FirstOrDefaultAsync();
                if (ques == null)
                    continue;

                var choosen = new Models.ChoosenAnswers
                {
                    userId = UserId,
                    questionsId = ques.Questionsid,
                    answerId = AnswerID.Id,
                    ExamID = ExamID,
                };

                bool areEql = string.Equals(ques.Questions.correctAnswer, answer.Text, StringComparison.OrdinalIgnoreCase);

                if (areEql)                
                    totalScore += ques.Questions.Score;
                
                _context.choosenAnswers.Add(choosen);

                choosenAnswers.Add(choosen);
            }

            var submit = new SubmitedExams
            {
                ExamID = exam.Id,
                UserId = UserId,
                SubjectName = exam.SubjectName,
                dateTime = DateTime.Now,
                Score = totalScore
            };
            _context.submitedExams.Add(submit);

            await _context.SaveChangesAsync();
            return (submit.Score).ToString();
        }

        public string GetCurrentStudent()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");
            return userIdClaim;
        }

        public async Task<IEnumerable<Exam>> GetExamByID(int examID)
        {
            var exam = await _context.exams.Where(x=>x.Id==examID).Include(c => c.questions).ThenInclude(c => c.answersModels).ToListAsync();
            return exam;
        }

  
    }
}
