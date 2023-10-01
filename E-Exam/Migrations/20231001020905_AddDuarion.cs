using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class AddDuarion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpenForStudent",
                table: "exams");

            migrationBuilder.AddColumn<decimal>(
                name: "duration",
                table: "exams",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "end",
                table: "exams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "start",
                table: "exams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duration",
                table: "exams");

            migrationBuilder.DropColumn(
                name: "end",
                table: "exams");

            migrationBuilder.DropColumn(
                name: "start",
                table: "exams");

            migrationBuilder.AddColumn<bool>(
                name: "IsOpenForStudent",
                table: "exams",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
