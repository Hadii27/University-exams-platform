using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class upd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_questions_exams_ExamId",
                table: "questions");

            migrationBuilder.RenameColumn(
                name: "ExamId",
                table: "questions",
                newName: "examId");

            migrationBuilder.RenameIndex(
                name: "IX_questions_ExamId",
                table: "questions",
                newName: "IX_questions_examId");

            migrationBuilder.AlterColumn<int>(
                name: "examId",
                table: "questions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_questions_exams_examId",
                table: "questions",
                column: "examId",
                principalTable: "exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_questions_exams_examId",
                table: "questions");

            migrationBuilder.RenameColumn(
                name: "examId",
                table: "questions",
                newName: "ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_questions_examId",
                table: "questions",
                newName: "IX_questions_ExamId");

            migrationBuilder.AlterColumn<int>(
                name: "ExamId",
                table: "questions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_questions_exams_ExamId",
                table: "questions",
                column: "ExamId",
                principalTable: "exams",
                principalColumn: "Id");
        }
    }
}
