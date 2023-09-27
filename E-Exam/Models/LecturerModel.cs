using System.Text.Json.Serialization;

namespace E_Exam.Models
{
    public class LecturerModel
    {
        public int Id { get; set; }

        public string UserID { get; set; }

        [JsonIgnore]

        public ApplicationUser User { get; set; }
        public int SubjectId { get; set; }
        [JsonIgnore]

        public List<SubjectDepartmentModel> subjects { get; set; }
    }
}
