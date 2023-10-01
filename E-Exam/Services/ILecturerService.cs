using E_Exam.Dto;
using E_Exam.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Exam.Services
{
    public interface ILecturerService
    {
        public Task<Exam> AddExam(Exam exams, int subjectId, List<Questions> questions);
        public Exam GetExam(int ExamId);
        public Task<IEnumerable<Exam>> GetAllexams();
        public Task<Exam> CalculteTotalScore(int examID);
        public Task<SubjectModel> GetSubject(int SubjectId);
        public string GetCurrentUserId();


    }
}
