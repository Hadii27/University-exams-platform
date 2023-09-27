using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class zz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_choosenAnswers_answers_answerId",
                table: "choosenAnswers");

            migrationBuilder.DropIndex(
                name: "IX_choosenAnswers_answerId",
                table: "choosenAnswers");

            migrationBuilder.AddColumn<int>(
                name: "ExamID",
                table: "choosenAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamID",
                table: "choosenAnswers");

            migrationBuilder.CreateIndex(
                name: "IX_choosenAnswers_answerId",
                table: "choosenAnswers",
                column: "answerId");

            migrationBuilder.AddForeignKey(
                name: "FK_choosenAnswers_answers_answerId",
                table: "choosenAnswers",
                column: "answerId",
                principalTable: "answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
