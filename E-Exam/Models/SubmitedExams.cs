using System.Text.Json.Serialization;

namespace E_Exam.Models
{
    public class SubmitedExams
    {
        public int id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public Exam exam { get; set; }
        public int ExamID { get; set; }
        public string SubjectName { get; set; }
        public DateTime dateTime { get; set; }
        public int Score { get; set; }


    }
}
