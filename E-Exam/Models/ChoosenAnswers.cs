using System.Text.Json.Serialization;

namespace E_Exam.Models
{
    public class ChoosenAnswers
    {
        public int id { get; set; }
        [JsonIgnore]
        public ApplicationUser user { get; set; }

        public string userId { get; set; }
        public int ExamID { get; set; }
        public int questionsId { get; set; }

        public int answerId { get; set; }

        public int QuestionScore { get; set; }

    }
}
