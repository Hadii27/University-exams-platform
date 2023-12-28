using System.ComponentModel.DataAnnotations;

namespace E_Exam.Dto
{
    public class RequestRegisterDto
    {
        public int ReqCollegeID { get; set; }
        public int ReqDepartmentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int internationalID { get; set; }
        public string PhoneNumber { get; set; }      
        public string Password { get; set; }

        public string Role { get; set; }
        public int Grade { get; set; }
    }
}
