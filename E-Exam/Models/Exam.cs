using System.Text.Json.Serialization;

namespace E_Exam.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        [JsonIgnore]
        public SubjectModel subject { get; set; }
        public int Grade { get; set; }
        public int QuestionsCount { get; set; }
        public int ExamScore { get; set; }
        public List<Questions> questions { get; set; }

        public DateTime start { get; set; }
        public DateTime end { get; set; }

        public decimal duration { get; set; }


    }
}
