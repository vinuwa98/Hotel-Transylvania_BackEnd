using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HmsBackend.Migrations
{
    /// <inheritdoc />
    public partial class fixedFKeyIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_Accounts_UserId",
                table: "Complaint");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_Rooms_RoomId",
                table: "Complaint");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_Accounts_UserId",
                table: "Complaint",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_Rooms_RoomId",
                table: "Complaint",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_Accounts_UserId",
                table: "Complaint");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaint_Rooms_RoomId",
                table: "Complaint");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_Accounts_UserId",
                table: "Complaint",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaint_Rooms_RoomId",
                table: "Complaint",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
