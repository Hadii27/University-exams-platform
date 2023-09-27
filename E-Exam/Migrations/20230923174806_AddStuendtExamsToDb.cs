using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class AddStuendtExamsToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "studentExams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    studentId1 = table.Column<int>(type: "int", nullable: false),
                    studentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    examId = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentExams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_studentExams_exams_examId",
                        column: x => x.examId,
                        principalTable: "exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_studentExams_students_studentId1",
                        column: x => x.studentId1,
                        principalTable: "students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_studentExams_examId",
                table: "studentExams",
                column: "examId");

            migrationBuilder.CreateIndex(
                name: "IX_studentExams_studentId1",
                table: "studentExams",
                column: "studentId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "studentExams");
        }
    }
}
