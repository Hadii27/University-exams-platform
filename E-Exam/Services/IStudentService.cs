using E_Exam.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Exam.Services
{
    public interface IStudentService
    {
        public Task<IEnumerable<Exam>> GetExamsOfSubject(string StudentID, int SubjectID, int ExamID);
        public Task<IEnumerable<SubjectDepartmentModel>> GetSubjects(string StudentID);
        public Task<Models.ChoosenAnswers> ChooseAnswer(string UserId, int examID, int QuestionID, int AnswerID);
        public Task<object> GetExamInfo(string studentId, int subjectId);
        public Task<IEnumerable<ChooseSubjects>> StudentsSubjects(string StudentId, IEnumerable<int> SubIDs);
        public Task<IEnumerable<ChooseSubjects>> GetChoosedSubjects(string studentID);



    }
}
