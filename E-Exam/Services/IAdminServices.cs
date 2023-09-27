using E_Exam.Models;

namespace E_Exam.Services
{
    public interface IAdminServices
    {
        public Task<FacultyModel> AddFaculty(FacultyModel model);

        public Task<Departments> AddDepartmentToFaculty(int facultyId, Departments model);

        public Task<SubjectModel> AddSubject(SubjectModel subject, int departmentID);

        public Task<FacultyModel> GetFacultyByID(int id);

        public Task<Departments> GetDepartmentByID(int id);

        public Task<LecturerModel> AssignLecturerSubject(string userID, int SubID);
        public Task<SubjectModel> GetSubjectByID(int id);

        public Task<ApplicationUser> GetUserByID(string id);

        public Task<bool> IsLecturerAssignedToSubject(string userID, int subID);

        public Task<string> DeleteLecturer(string userID);

        public Task<List<SubjectModel>> GetAllSubjects();
        public Task<List<Departments>> GetAllDepartments();

        public Task<List<FacultyModel>> GettAllFaculties();

        public Task<StudentModel> AssignStudent(StudentModel student);



    };
}
