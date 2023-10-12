using E_Exam.Migrations;
using System.ComponentModel.DataAnnotations;

namespace E_Exam.Models
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public int internationalID { get; set; }

        public int Grade { get; set; }

        public string RoleID { get; set; }

        public int FaculityID { get; set; }
        public int DepartmentID { get; set; }



    }
}
