using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_Exam.Models
{
    public class SubjectDepartmentModel
    {
        [Key]
        [JsonIgnore]

        public int Id { get; set; }
        [JsonIgnore]

        public int SubjectId { get; set; }
        [JsonIgnore]

        public int DepartmentId { get; set; }
        public SubjectModel Subject { get; set; }
        [JsonIgnore]
        public Departments Department { get; set; }
    }
}
