using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_Exam.Models
{
    public class ChooseSubjects
    {

        public int Id { get; set; }

        public int SubjectId { get; set; }

        public string UserId { get; set; }
        public SubjectModel Subject { get; set; }
        public ApplicationUser User { get; set; }
        public int grade { get; set; }
    }
}
