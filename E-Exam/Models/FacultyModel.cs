using System.Text.Json.Serialization;

namespace E_Exam.Models
{
    public class FacultyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentsCount { get; set; }
        public List<Departments> departments { get; set; }
        public string Description { get; set; }

    }
}
