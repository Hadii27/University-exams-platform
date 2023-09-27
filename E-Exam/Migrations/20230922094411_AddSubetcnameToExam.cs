using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class AddSubetcnameToExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubjectName",
                table: "exams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectName",
                table: "exams");
        }
    }
}
