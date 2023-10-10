using System.Text.Json.Serialization;

namespace E_Exam.Models
{
    public class FacultyAdmin
    {
        public int Id { get; set; }
        [JsonIgnore]
        public ApplicationUser Admin { get; set; }
        public string AdminID { get; set; }
        public string AdminName { get; set; }
        [JsonIgnore]
        public FacultyModel faculty { get; set; }
        public int FacultyId { get; set; }

        public string FacultyName { get; set; }


    }
}
