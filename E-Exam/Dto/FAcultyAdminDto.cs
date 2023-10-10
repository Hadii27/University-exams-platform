using System.ComponentModel.DataAnnotations;

namespace E_Exam.Dto
{
    public class FAcultyAdminDto
    {
        [Required]
        public string AdminID { get; set; }
        [Required]
        public int FAcultyID { get; set; }
    }
}
