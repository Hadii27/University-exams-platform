using E_Exam.Models;

namespace E_Exam.Services
{
    public interface IMasterService
    {
        public Task<FacultyModel> AddFaculty(FacultyModel model);
        public Task<FacultyAdmin> AssignFacultyAdmin(FacultyAdmin model);
        public Task<IEnumerable<FacultyAdmin>> GetAllAssignFacultyAdmin();
        public Task<IEnumerable<FacultyModel>> GetFaculties();
        public  Task<string> UnAssignAdmin(string userID);

        public string GetCurrentUser();



    }
}
