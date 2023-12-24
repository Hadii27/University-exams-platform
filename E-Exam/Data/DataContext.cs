using E_Exam.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Exam.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
            public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<FacultyModel> faculties { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<SubjectModel> subject { get; set; }

        public DbSet<SubjectDepartmentModel> subjectDepartments { get; set; }
        public DbSet<LecturerModel> lecturers { get; set; }

        public DbSet<Exam> exams { get; set; }
        public DbSet<Questions> questions { get; set; }

        public DbSet<AnswersModel> answers { get; set; }

        public DbSet<StudentModel> students { get; set; }

        public DbSet<StudentExams> studentExams { get; set; }

        public DbSet<SubmitedExams> submitedExams { get; set; }

        public DbSet<ChoosenAnswers> choosenAnswers { get; set; }

        public DbSet<ChooseSubjects> chooseSubjects { get; set; }

        public DbSet<ReqRegister> reqRegisters { get; set; }

        public DbSet<FacultyAdmin> facultyAdmins { get; set; }
    }
}
