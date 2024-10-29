using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kertu.InteractiveServer.Migrations
{
    /// <inheritdoc />
    public partial class hleppelementownerfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "KertuElements",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KertuElements_ApplicationUserId",
                table: "KertuElements",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_KertuElements_AspNetUsers_ApplicationUserId",
                table: "KertuElements",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KertuElements_AspNetUsers_ApplicationUserId",
                table: "KertuElements");

            migrationBuilder.DropIndex(
                name: "IX_KertuElements_ApplicationUserId",
                table: "KertuElements");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "KertuElements");
        }
    }
}
