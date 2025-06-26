using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HmsBackend.Migrations
{
    /// <inheritdoc />
    public partial class fixedErros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Complaint_ComplaintId",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_JobUser_Accounts_UserId",
                table: "JobUser");

            migrationBuilder.DropForeignKey(
                name: "FK_JobUser_Job_JobId",
                table: "JobUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Complaint_ComplaintId",
                table: "Job",
                column: "ComplaintId",
                principalTable: "Complaint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobUser_Accounts_UserId",
                table: "JobUser",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobUser_Job_JobId",
                table: "JobUser",
                column: "JobId",
                principalTable: "Job",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Complaint_ComplaintId",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_JobUser_Accounts_UserId",
                table: "JobUser");

            migrationBuilder.DropForeignKey(
                name: "FK_JobUser_Job_JobId",
                table: "JobUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Complaint_ComplaintId",
                table: "Job",
                column: "ComplaintId",
                principalTable: "Complaint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobUser_Accounts_UserId",
                table: "JobUser",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobUser_Job_JobId",
                table: "JobUser",
                column: "JobId",
                principalTable: "Job",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
