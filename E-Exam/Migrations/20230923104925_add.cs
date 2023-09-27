using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "facultyId",
                table: "students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_students_facultyId",
                table: "students",
                column: "facultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_students_faculties_facultyId",
                table: "students",
                column: "facultyId",
                principalTable: "faculties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_faculties_facultyId",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_students_facultyId",
                table: "students");

            migrationBuilder.DropColumn(
                name: "facultyId",
                table: "students");
        }
    }
}
