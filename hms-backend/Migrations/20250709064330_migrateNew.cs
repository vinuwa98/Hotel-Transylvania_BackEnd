using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace hms_backend.Migrations
{
    /// <inheritdoc />
    public partial class migrateNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_UserId",
                table: "Rooms");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "RoomType", "UserId" },
                values: new object[,]
                {
                    { 1, "Single", null },
                    { 2, "Double", null },
                    { 3, "Deluxe", null },
                    { 4, "Suite", null },
                    { 5, "Family", null },
                    { 6, "Presidential", null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_UserId",
                table: "Rooms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_UserId",
                table: "Rooms");

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: 6);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_UserId",
                table: "Rooms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
