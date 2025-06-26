using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HmsBackend.Migrations
{
    /// <inheritdoc />
    public partial class addedfullrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Accounts_AssignedMaintenanceUserId",
                table: "Job");

            migrationBuilder.RenameColumn(
                name: "AssignedMaintenanceUserId",
                table: "Job",
                newName: "AssignedManagerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Job_AssignedMaintenanceUserId",
                table: "Job",
                newName: "IX_Job_AssignedManagerUserId");

            migrationBuilder.CreateTable(
                name: "JobUser",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobUser", x => new { x.JobId, x.UserId });
                    table.ForeignKey(
                        name: "FK_JobUser_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobUser_Job_JobId",
                        column: x => x.JobId,
                        principalTable: "Job",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobUser_UserId",
                table: "JobUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Accounts_AssignedManagerUserId",
                table: "Job",
                column: "AssignedManagerUserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Accounts_AssignedManagerUserId",
                table: "Job");

            migrationBuilder.DropTable(
                name: "JobUser");

            migrationBuilder.RenameColumn(
                name: "AssignedManagerUserId",
                table: "Job",
                newName: "AssignedMaintenanceUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Job_AssignedManagerUserId",
                table: "Job",
                newName: "IX_Job_AssignedMaintenanceUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Accounts_AssignedMaintenanceUserId",
                table: "Job",
                column: "AssignedMaintenanceUserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
