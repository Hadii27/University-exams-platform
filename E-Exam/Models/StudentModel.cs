using E_Exam.Migrations;
using System.Text.Json.Serialization;

namespace E_Exam.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public int internationalID { get; set; }
        public string Username { get; set; }
        public string FacultyName { get; set; }
        public int DepartmentId { get; set;}
        public string DepartmentName { get; set; }
        public int Grade { get; set; }

    }
}
