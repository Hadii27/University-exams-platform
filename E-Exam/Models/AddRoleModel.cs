using System.ComponentModel.DataAnnotations;

namespace E_Exam.Models
{
    public class AddRoleModel
    {
        [Required]
        public string userID { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
