using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class sd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "students",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_students_UserId",
                table: "students",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_students_AspNetUsers_UserId",
                table: "students",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_AspNetUsers_UserId",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_students_UserId",
                table: "students");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
