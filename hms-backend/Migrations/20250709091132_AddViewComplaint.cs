using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hms_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddViewComplaint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_AspNetUsers_UserId",
                table: "Complaint");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_Rooms_RoomId",
                table: "Complaint");

            migrationBuilder.DropForeignKey(
                name: "FK_Job_Complaint_ComplaintId",
                table: "Job");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Complaint",
                table: "Complaint");

            migrationBuilder.RenameTable(
                name: "Complaint",
                newName: "Complaints");

            migrationBuilder.RenameIndex(
                name: "IX_Complaint_UserId",
                table: "Complaints",
                newName: "IX_Complaints_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Complaint_RoomId",
                table: "Complaints",
                newName: "IX_Complaints_RoomId");

            migrationBuilder.AddColumn<string>(
                name: "CleanerId",
                table: "Job",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Complaints",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Complaints",
                table: "Complaints",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Job_CleanerId",
                table: "Job",
                column: "CleanerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_UserId",
                table: "Complaints",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Rooms_RoomId",
                table: "Complaints",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Job_AspNetUsers_CleanerId",
                table: "Job",
                column: "CleanerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Complaints_ComplaintId",
                table: "Job",
                column: "ComplaintId",
                principalTable: "Complaints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_AspNetUsers_UserId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Rooms_RoomId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Job_AspNetUsers_CleanerId",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_Job_Complaints_ComplaintId",
                table: "Job");

            migrationBuilder.DropIndex(
                name: "IX_Job_CleanerId",
                table: "Job");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Complaints",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "CleanerId",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Complaints");

            migrationBuilder.RenameTable(
                name: "Complaints",
                newName: "Complaint");

            migrationBuilder.RenameIndex(
                name: "IX_Complaints_UserId",
                table: "Complaint",
                newName: "IX_Complaint_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Complaints_RoomId",
                table: "Complaint",
                newName: "IX_Complaint_RoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Complaint",
                table: "Complaint",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_AspNetUsers_UserId",
                table: "Complaint",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_Rooms_RoomId",
                table: "Complaint",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Complaint_ComplaintId",
                table: "Job",
                column: "ComplaintId",
                principalTable: "Complaint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
