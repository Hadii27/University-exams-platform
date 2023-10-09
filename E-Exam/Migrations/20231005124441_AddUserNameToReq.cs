﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Exam.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameToReq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "reqRegisters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "reqRegisters");
        }
    }
}
