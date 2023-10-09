using E_Exam.Data;
using E_Exam.Migrations;
using E_Exam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace E_Exam.Services
{
    public class AdminService : IAdminServices
    {
        private readonly DataContext _context;
        public AdminService(DataContext context)
        {
            _context = context;
        }

        public async Task<FacultyModel> AddFaculty(FacultyModel model)
        {
            var faculty = new FacultyModel
            {
                Name = model.Name,
                Description = model.Description,
            };
            _context.faculties.Add(faculty);
            await _context.SaveChangesAsync();
            return faculty;
        }

        public async Task<Departments> AddDepartmentToFaculty(int facultyId, Departments model)
        {
            var Faculty = await _context.faculties.FindAsync(facultyId);


            var department = new Departments
            {
                Name = model.Name,
                Description = model.Description,
                FacultyId = facultyId
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            await IncrementDepartmentCount(facultyId);
            return department;

        }

        public async Task<SubjectModel> AddSubject(SubjectModel subject, int departmentID)
        {
            var departmentId = await GetDepartmentByID(departmentID); 

            var subjectModel = new SubjectModel
            {
                Name = subject.Name,
                Description = subject.Description,
                Grade = subject.Grade,
            };

            _context.subject.Add(subjectModel);
            await _context.SaveChangesAsync();
            var subjectDepartment = new SubjectDepartmentModel
            {
                SubjectId = subjectModel.Id,
                DepartmentId = departmentID,
                
            };
            _context.subjectDepartments.Add(subjectDepartment);
            await _context.SaveChangesAsync();
            return subjectModel;
        }
        public async Task<FacultyModel> GetFacultyByID(int id)
        {
            var faculty = await _context.faculties.FindAsync(id);
            return faculty;
        }

        public async Task<Departments> GetDepartmentByID(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            return department;
        }

        public async Task IncrementDepartmentCount(int id)
        {
            var faculty = await GetFacultyByID(id);
            if (faculty is not null)
            {
                faculty.DepartmentsCount++;
                await _context.SaveChangesAsync();
            };            
        }

        public async Task<ApplicationUser> GetUserByID(string id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }
        public async Task<SubjectModel> GetSubjectByID(int id)
        {
            var subject = await _context.subject.FindAsync(id);
            return subject;
        }
        public async Task<bool> IsLecturerAssignedToSubject(string userID, int subID)
        {
            return await _context.lecturers.AnyAsync(l => l.UserID == userID && l.SubjectId == subID);
        }
        public async Task<LecturerModel> AssignLecturerSubject(string userID, int SubID)
        {
           
            var lecturerModel = new LecturerModel
            {
                UserID = userID,
                SubjectId = SubID,
            };
            _context.lecturers.Add(lecturerModel);
            await _context.SaveChangesAsync();
            return lecturerModel;
        }
        public async Task<string> DeleteLecturer(string userID)
        {
            
            var lecturer = await _context.lecturers.Where(l => l.UserID == userID).FirstOrDefaultAsync();       
            _context.lecturers.Remove(lecturer);
            await _context.SaveChangesAsync();
            return $"Lectrer that has ID {lecturer.UserID} Deleted Succesfully";
        }

        public async Task<List<SubjectModel>> GetAllSubjects()
        {
            var results = await _context.subject.ToListAsync();
            return results;
        }

        public async Task<List<Departments>> GetAllDepartments()
        {
            var results = await _context.Departments.ToListAsync();
            return results;
        }

        public async Task<List<FacultyModel>> GettAllFaculties()
        {
            var faculties = await _context.faculties.ToListAsync();
            return faculties;
        }


        public async Task<StudentModel> AssignStudent(StudentModel student)
        {
            var user = await GetUserByID(student.UserId);
            var department =  await GetDepartmentByID(student.DepartmentId);
            var departmentName = department.Name;
            var username = user.UserName;
            var faculty = await GetFacultyByID(department.FacultyId);
            var nationalID = await _context.reqRegisters.Where(i => i.internationalID == student.internationalID).FirstOrDefaultAsync();
            if (nationalID is not null)
                return null;
            var Student = new StudentModel
            {
                UserId = student.UserId,
                Username = username,
                FacultyName = faculty.Name,
                DepartmentId = student.DepartmentId,
                DepartmentName = departmentName,
                Grade = student.Grade,
                internationalID = student.internationalID,
            };
            _context.students.Add(Student);
            await _context.SaveChangesAsync();
            await ChangeStatusOfReq(Student.internationalID);
            return Student;
        }


        public async Task<string> ChangeStatusOfReq(int internationalID)
        {
            var request = await _context.reqRegisters.FirstOrDefaultAsync(i => i.internationalID == internationalID);

            if (request == null)           
                return "Request not found"; 
                        
            request.status = "Success"; 

            _context.Update(request);
            await _context.SaveChangesAsync();

            return "Register succeeded";
        }


    }
}
