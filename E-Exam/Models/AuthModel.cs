namespace E_Exam.Models
{
    public class AuthModel
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime ExpireOn { get; set; }
        public bool isAuthenticated { get; set; }

        public UserDataModel UserData { get; set; }

    }
}
