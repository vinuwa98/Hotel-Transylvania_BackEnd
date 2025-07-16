using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hms_backend.Migrations
{
    /// <inheritdoc />
    public partial class name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "ComplaintCode",
                table: "Complaints",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImgBlob",
                table: "Complaints",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComplaintCode",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "ImgBlob",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
