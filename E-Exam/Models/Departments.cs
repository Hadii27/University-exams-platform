using System.Text.Json.Serialization;

namespace E_Exam.Models
{
    public class Departments
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int FacultyId { get; set; }
        [JsonIgnore]

        public FacultyModel Faculty { get; set; }
    }
}
