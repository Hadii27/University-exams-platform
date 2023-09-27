using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class ExamID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_questions_exams_examId",
                table: "questions");

            migrationBuilder.RenameColumn(
                name: "examId",
                table: "questions",
                newName: "ExamID");

            migrationBuilder.RenameIndex(
                name: "IX_questions_examId",
                table: "questions",
                newName: "IX_questions_ExamID");

            migrationBuilder.AddForeignKey(
                name: "FK_questions_exams_ExamID",
                table: "questions",
                column: "ExamID",
                principalTable: "exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_questions_exams_ExamID",
                table: "questions");

            migrationBuilder.RenameColumn(
                name: "ExamID",
                table: "questions",
                newName: "examId");

            migrationBuilder.RenameIndex(
                name: "IX_questions_ExamID",
                table: "questions",
                newName: "IX_questions_examId");

            migrationBuilder.AddForeignKey(
                name: "FK_questions_exams_examId",
                table: "questions",
                column: "examId",
                principalTable: "exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
