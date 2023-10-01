using E_Exam.Data;
using E_Exam.Dto;
using E_Exam.Migrations;
using E_Exam.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace E_Exam.Services
{
    public class LecturerService : ILecturerService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LecturerService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Exam> AddExam(Exam exams, int subjectID, List<Questions> questions)
        {
            var sub = await _context.subject.FindAsync(subjectID);
            if (sub == null)
                return null;
            int totalScore = questions.Sum(q => q.Score);
            var exam = new Exam
            {
                Name = exams.Name,
                Description = exams.Description,
                SubjectId = exams.SubjectId,
                ExamScore = totalScore,
                SubjectName = exams.SubjectName,
                Grade = exams.Grade,
                questions = questions,
                QuestionsCount = exams.QuestionsCount,
                start = exams.start,
                end = exams.end,
                duration = exams.duration,                          
            };

            foreach (var question in questions)
            {
                var answers = new List<AnswersModel>();
                foreach (var ans in question.answersModels)
                {
                    var answer = new AnswersModel
                    {
                        Text = ans.Text,
                        Questionsid = question.id,
                    };
                    answers.Add(answer);
                }
                question.answersModels.AddRange(answers);
            }

            var result = _context.exams.Add(exam);
            await _context.SaveChangesAsync();
            return exam;
        }

        public Exam GetExam(int ExamId)
        {
            var exam =  _context.exams.Find(ExamId);
            return exam;
        }

        public async Task<IEnumerable<Exam>> GetAllexams()
        {
            var exam = await _context.exams.Include(c=>c.questions).ThenInclude(c=>c.answersModels).ToListAsync();
            return exam;
        }

        public async Task<Exam> CalculteTotalScore(int examID)
        {
            var exam = await _context.exams.Include(e => e.questions).FirstOrDefaultAsync(e => e.Id == examID);
            if (exam == null)
                return null;
            int totalScore =  exam.questions.Sum(q=>q.Score);
            exam.ExamScore =  totalScore;
            var result = _context.exams.Update(exam);
             _context.SaveChanges();
            return exam;
        }
        public async Task<Exam> IncrementQuestionsCount(int id)
        {
            var Exam = GetExam(id);
            if (Exam is  null)
            {
                return null;
            }
            Exam.QuestionsCount++;
            await _context.SaveChangesAsync();
            return Exam;
        }


        public async Task<SubjectModel> GetSubject(int SubjectId)
        {
            var subject = await _context.subject.FindAsync(SubjectId);
            return subject;
        }

        public string GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userIdClaim;
        }

        public async Task<IEnumerable<LecturerModel>> GetLecturers()
        {
            var LecturerSub = await _context.lecturers.ToListAsync();
            return LecturerSub;
        }
    }
}
