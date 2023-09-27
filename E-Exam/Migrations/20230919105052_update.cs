using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_subject_SubjectModelId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_SubjectModelId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "SubjectModelId",
                table: "Departments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectModelId",
                table: "Departments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SubjectModelId",
                table: "Departments",
                column: "SubjectModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_subject_SubjectModelId",
                table: "Departments",
                column: "SubjectModelId",
                principalTable: "subject",
                principalColumn: "Id");
        }
    }
}
