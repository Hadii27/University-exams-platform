using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class removenaswe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_answer_questions_QuestioniD",
                table: "answer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_answer",
                table: "answer");

            migrationBuilder.RenameTable(
                name: "answer",
                newName: "Answer");

            migrationBuilder.RenameIndex(
                name: "IX_answer_QuestioniD",
                table: "Answer",
                newName: "IX_Answer_QuestioniD");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Answer",
                table: "Answer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_questions_QuestioniD",
                table: "Answer",
                column: "QuestioniD",
                principalTable: "questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_questions_QuestioniD",
                table: "Answer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Answer",
                table: "Answer");

            migrationBuilder.RenameTable(
                name: "Answer",
                newName: "answer");

            migrationBuilder.RenameIndex(
                name: "IX_Answer_QuestioniD",
                table: "answer",
                newName: "IX_answer_QuestioniD");

            migrationBuilder.AddPrimaryKey(
                name: "PK_answer",
                table: "answer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_answer_questions_QuestioniD",
                table: "answer",
                column: "QuestioniD",
                principalTable: "questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
