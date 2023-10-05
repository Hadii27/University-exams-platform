using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chooseSubjects_AspNetUsers_UserId1",
                table: "chooseSubjects");

            migrationBuilder.DropIndex(
                name: "IX_chooseSubjects_UserId1",
                table: "chooseSubjects");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "chooseSubjects");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "chooseSubjects",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_chooseSubjects_UserId",
                table: "chooseSubjects",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_chooseSubjects_AspNetUsers_UserId",
                table: "chooseSubjects",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chooseSubjects_AspNetUsers_UserId",
                table: "chooseSubjects");

            migrationBuilder.DropIndex(
                name: "IX_chooseSubjects_UserId",
                table: "chooseSubjects");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "chooseSubjects",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "chooseSubjects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_chooseSubjects_UserId1",
                table: "chooseSubjects",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_chooseSubjects_AspNetUsers_UserId1",
                table: "chooseSubjects",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
