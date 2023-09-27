using System.Text.Json.Serialization;

namespace E_Exam.Models
{
    public class AnswersModel
    {
        public int Id { get; set; }
        public int Questionsid { get; set; }
        [JsonIgnore]
        public Questions Questions { get; set; }
        public string Text { get; set; }
        public bool CorrectAnswer { get; set; }
    }
}
