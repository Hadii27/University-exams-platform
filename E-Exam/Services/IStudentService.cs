using E_Exam.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Exam.Services
{
    public interface IStudentService
    {
        public Task<Exam> GetExamsOfSubject(string StudentID, int ExamID);
        public Task<IEnumerable<SubjectDepartmentModel>> GetSubjects(string StudentID);
        public Task<string> ExamSubmit(int ExamID, string UserId, IEnumerable<AnswersModel> AnswerIDs);
        public Task<object> GetExamInfo(string studentId, int subjectId);
        public Task<string> StudentsSubjects(string StudentId, int SubID);
        public Task<IEnumerable<ChooseSubjects>> GetChoosedSubjects(string studentID);
        public string GetCurrentStudent();

    }
}
