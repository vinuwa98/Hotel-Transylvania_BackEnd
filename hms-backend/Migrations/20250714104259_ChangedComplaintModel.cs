using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HmsBackend.Migrations
{
    /// <inheritdoc />
    public partial class ChangedComplaintModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComplaintNumber",
                table: "Complaint",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComplaintNumber",
                table: "Complaint");
        }
    }
}
