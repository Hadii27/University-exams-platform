using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class Addddd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studentExams_exams_examId",
                table: "studentExams");

            migrationBuilder.DropForeignKey(
                name: "FK_studentExams_students_studentId1",
                table: "studentExams");

            migrationBuilder.DropIndex(
                name: "IX_studentExams_examId",
                table: "studentExams");

            migrationBuilder.DropIndex(
                name: "IX_studentExams_studentId1",
                table: "studentExams");

            migrationBuilder.DropColumn(
                name: "studentId1",
                table: "studentExams");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "studentId1",
                table: "studentExams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_studentExams_examId",
                table: "studentExams",
                column: "examId");

            migrationBuilder.CreateIndex(
                name: "IX_studentExams_studentId1",
                table: "studentExams",
                column: "studentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_studentExams_exams_examId",
                table: "studentExams",
                column: "examId",
                principalTable: "exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studentExams_students_studentId1",
                table: "studentExams",
                column: "studentId1",
                principalTable: "students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
