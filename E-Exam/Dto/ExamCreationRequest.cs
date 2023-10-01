namespace E_Exam.Dto
{
    public class ExamCreationRequest
    {
        public ExamDto Exam { get; set; }
        public List<QuestionsDto> Questions { get; set; }


    }
}
