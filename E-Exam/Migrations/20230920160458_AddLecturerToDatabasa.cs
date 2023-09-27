using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class AddLecturerToDatabasa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LecturerModelId",
                table: "subjectDepartments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "lecturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lecturers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lecturers_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_subjectDepartments_LecturerModelId",
                table: "subjectDepartments",
                column: "LecturerModelId");

            migrationBuilder.CreateIndex(
                name: "IX_lecturers_UserID",
                table: "lecturers",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_subjectDepartments_lecturers_LecturerModelId",
                table: "subjectDepartments",
                column: "LecturerModelId",
                principalTable: "lecturers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subjectDepartments_lecturers_LecturerModelId",
                table: "subjectDepartments");

            migrationBuilder.DropTable(
                name: "lecturers");

            migrationBuilder.DropIndex(
                name: "IX_subjectDepartments_LecturerModelId",
                table: "subjectDepartments");

            migrationBuilder.DropColumn(
                name: "LecturerModelId",
                table: "subjectDepartments");
        }
    }
}
