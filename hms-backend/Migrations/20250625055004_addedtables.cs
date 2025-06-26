using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HmsBackend.Migrations
{
    /// <inheritdoc />
    public partial class addedtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Accounts_UserId",
                table: "Job");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Job",
                newName: "CreatedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Job_UserId",
                table: "Job",
                newName: "IX_Job_CreatedUserId");

            migrationBuilder.AddColumn<int>(
                name: "AssignedMaintenanceUserId",
                table: "Job",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SupervisorID",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Job_AssignedMaintenanceUserId",
                table: "Job",
                column: "AssignedMaintenanceUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Accounts_AssignedMaintenanceUserId",
                table: "Job",
                column: "AssignedMaintenanceUserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Accounts_CreatedUserId",
                table: "Job",
                column: "CreatedUserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Accounts_AssignedMaintenanceUserId",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_Job_Accounts_CreatedUserId",
                table: "Job");

            migrationBuilder.DropIndex(
                name: "IX_Job_AssignedMaintenanceUserId",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "AssignedMaintenanceUserId",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "SupervisorID",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserId",
                table: "Job",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Job_CreatedUserId",
                table: "Job",
                newName: "IX_Job_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Accounts_UserId",
                table: "Job",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
