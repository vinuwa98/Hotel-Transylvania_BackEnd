using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hms_backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangedJobUsersModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobUser_AspNetUsers_UserId",
                table: "JobUser");

            migrationBuilder.DropForeignKey(
                name: "FK_JobUser_Job_JobId",
                table: "JobUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobUser",
                table: "JobUser");

            migrationBuilder.RenameTable(
                name: "JobUser",
                newName: "JobUsers");

            migrationBuilder.RenameIndex(
                name: "IX_JobUser_UserId",
                table: "JobUsers",
                newName: "IX_JobUsers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobUsers",
                table: "JobUsers",
                columns: new[] { "JobId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_JobUsers_AspNetUsers_UserId",
                table: "JobUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobUsers_Job_JobId",
                table: "JobUsers",
                column: "JobId",
                principalTable: "Job",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobUsers_AspNetUsers_UserId",
                table: "JobUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_JobUsers_Job_JobId",
                table: "JobUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobUsers",
                table: "JobUsers");

            migrationBuilder.RenameTable(
                name: "JobUsers",
                newName: "JobUser");

            migrationBuilder.RenameIndex(
                name: "IX_JobUsers_UserId",
                table: "JobUser",
                newName: "IX_JobUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobUser",
                table: "JobUser",
                columns: new[] { "JobId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_JobUser_AspNetUsers_UserId",
                table: "JobUser",
                column: "UserId",
                principalTable: "AspNetUsers",
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
