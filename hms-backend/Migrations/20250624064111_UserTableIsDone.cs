using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HmsBackend.Migrations
{
    /// <inheritdoc />
    public partial class UserTableIsDone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Accounts",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "DOB",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Accounts",
                newName: "AccountId");
        }
    }
}
