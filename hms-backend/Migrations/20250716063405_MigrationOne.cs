using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hms_backend.Migrations
{
    /// <inheritdoc />
    public partial class MigrationOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgBlob",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Complaints");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImgBlob",
                table: "Complaints",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
