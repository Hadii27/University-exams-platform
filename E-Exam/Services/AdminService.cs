﻿using E_Exam.Data;
using E_Exam.Migrations;
using E_Exam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace E_Exam.Services
{
    public class AdminService : IAdminServices
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(DataContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }


        public async Task<FacultyModel> College(string AdminID)
        {
            var Getfaculty = _context.facultyAdmins.Where(x => x.AdminID == AdminID).FirstOrDefault();
            if (Getfaculty is null)
                return null;

            var faculty = await _context.faculties.Include(f => f.departments).Where(f => f.Id == Getfaculty.FacultyId).FirstOrDefaultAsync();
            if (faculty is null)
                return null;
            return faculty;
        }


        public async Task<List<Departments>> GetAllDepartments(string AdminID)
        {
            var faculty = _context.facultyAdmins.Where(x => x.AdminID == AdminID).FirstOrDefault();
            if (faculty is null)
                return null;

            var facultyId = faculty.FacultyId;
            var departments = await _context.Departments
                .Where(d => d.FacultyId == facultyId)
                .ToListAsync();
            return departments;
        }

        public async Task<Departments> AddDepartmentToFaculty(string AdminID ,Departments model)
        {
            var faculty = _context.facultyAdmins.Where(x => x.AdminID == AdminID).FirstOrDefault();
            if (faculty is null)
                return null;
            var facultyId = faculty.FacultyId;

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
            var user = GetCurrentAdmin();
            var checkAdmin = await _context.facultyAdmins.Where(a => a.FacultyId == departmentId.FacultyId && a.AdminID == user).FirstOrDefaultAsync();
            if (checkAdmin is null)
                return null;

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

        public async Task<IEnumerable<ReqRegister>> GetRequests()
        {
            var current = GetCurrentAdmin();
            var requests = await _context.reqRegisters
                .Where(r => r.status == "Pending")
                .ToListAsync();

            var result = new List<ReqRegister>();

            foreach (var request in requests)
            {
                var facultyAdmin = await _context.facultyAdmins
                    .Where(a => a.FacultyId == request.FaculityID && a.AdminID == current)
                    .FirstOrDefaultAsync();
                if (facultyAdmin != null)
                    result.Add(request);
            }
            return result;
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
            var user = await _userManager.FindByIdAsync(userID);
            var checkRole = _userManager.IsInRoleAsync(user, "Teacher");
            if (checkRole is null)
                return null;
            var lecturerModel = new LecturerModel
            {
                UserID = userID,
                SubjectId = SubID,
            };
            await _context.lecturers.AddAsync(lecturerModel);
            await _context.SaveChangesAsync();
            return lecturerModel;
        }

        public async Task<string> DeleteLecturer(string userID)
        {
            
            var lecturer = await _context.lecturers.Where(l => l.UserID == userID).FirstOrDefaultAsync();
            if (lecturer is null)
                return "Invalid Lecturer";
            _context.lecturers.Remove(lecturer);
            await _context.SaveChangesAsync();
            return $"Lectrer that has ID {lecturer.UserID} Deleted Succesfully";
        }

        public async Task<List<SubjectModel>> GetAllSubjects()
        {
            var results = await _context.subject.ToListAsync();
            return results;
        }

        public async Task<List<FacultyModel>> GettAllFaculties()
        {
            var faculties = await _context.faculties.ToListAsync();
            return faculties;
        }
        public async Task<StudentModel> AssignStudent(string studentID, int intenationalID, int CollegeID, int DeptID, int grade)
        {
            var user = await GetUserByID(studentID);
            var department =  await GetDepartmentByID(DeptID);
            var departmentName = department.Name;
            var username = user.UserName;
            var faculty = await GetFacultyByID(department.FacultyId);
            var nationalID = await _context.students.Where(i => i.internationalID == intenationalID).FirstOrDefaultAsync();
            if (nationalID is not null)
                return null;
            var Student = new StudentModel
            {
                UserId = studentID,
                Username = username,
                FacultyName = faculty.Name,
                DepartmentId = DeptID,
                DepartmentName = departmentName,
                Grade = grade,
                internationalID = intenationalID,
            };
            _context.students.Add(Student);
            await _context.SaveChangesAsync();
            await ChangeStatusOfReq(studentID);
            return Student;
        }

        public async Task<string> ChangeStatusOfReq(string UserId)
        {
            var request = await _context.reqRegisters.FirstOrDefaultAsync(i => i.UserID == UserId);

            if (request == null)           
                return "Request not found"; 
                        
            request.status = "Success"; 

            _context.Update(request);
            await _context.SaveChangesAsync();

            return "Register succeeded";
        }

        public string GetCurrentAdmin()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");
            return userIdClaim;
        }

        public async Task<IEnumerable<LecturerModel>> GetLecturers()
        {
            var lecturers = await _context.lecturers.ToListAsync();
            return lecturers;
        }

    }
}
