﻿namespace E_Exam.Models
{
    public class ReqRegister
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public int internationalID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string roleID { get; set; }
        public string role { get; set; }
        public string status { get; set; }

        public string UserID { get; set; }

        public int Grade { get; set; }

        public int FaculityID { get; set; }
        public string FaculityName { get; set; }

        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }
}
