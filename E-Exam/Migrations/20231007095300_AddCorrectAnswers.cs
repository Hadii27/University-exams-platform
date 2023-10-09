using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrectAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "answers");

            migrationBuilder.AddColumn<string>(
                name: "correctAnswer",
                table: "questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "correctAnswer",
                table: "questions");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "answers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
