using E_Exam.Models;

namespace E_Exam.Services
{
    public interface IStudentService
    {
        public Task<IEnumerable<Exam>> GetExamsOfSubject(string StudentID, int SubjectID, int ExamID);
        public Task<IEnumerable<SubjectDepartmentModel>> GetSubjects(string StudentID);
        public Task<Models.ChoosenAnswers> ChooseAnswer(string UserId, int examID, int QuestionID, int AnswerID);
        public Task<object> GetExamInfo(string studentId, int subjectId);


    }
}
