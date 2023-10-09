using E_Exam.Models;
using System.Text.Json.Serialization;

namespace E_Exam.Dto
{
    public class StudentDto
    {
        public string UserId { get; set; }
        public int DepartmentId { get; set; }
        public int internationalID { get; set; }

        public int Grade { get; set; }
    }
}
