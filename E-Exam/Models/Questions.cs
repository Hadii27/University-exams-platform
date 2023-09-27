using System.Text.Json.Serialization;

namespace E_Exam.Models
{
    public class Questions
    {
        public int id { get; set; }
        public int ExamID { get; set; }
        [JsonIgnore]
        public Exam exam { get; set; }
        public string Question { get; set; }
        public int Score { get; set; }

        public List<AnswersModel> answersModels { get; set; }
    }
}
