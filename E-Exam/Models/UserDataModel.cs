﻿namespace E_Exam.Models
{
    public class UserDataModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }

    }
}
