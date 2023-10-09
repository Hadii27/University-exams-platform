using E_Exam.Models;

namespace E_Exam.Dto
{
    public class QuestionsDto
    {
        public string Question { get; set; }
        public int Score { get; set; }
        public string CorrectAnswer { get; set; }

        public List<AnswersDto> Answers { get; set; }
    }
}
