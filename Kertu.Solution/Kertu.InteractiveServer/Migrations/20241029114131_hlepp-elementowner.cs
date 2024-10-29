using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kertu.InteractiveServer.Migrations
{
    /// <inheritdoc />
    public partial class hleppelementowner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KertuElements_AspNetUsers_ApplicationUserId",
                table: "KertuElements");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "KertuElements",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_KertuElements_ApplicationUserId",
                table: "KertuElements",
                newName: "IX_KertuElements_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_KertuElements_AspNetUsers_OwnerId",
                table: "KertuElements",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KertuElements_AspNetUsers_OwnerId",
                table: "KertuElements");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "KertuElements",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_KertuElements_OwnerId",
                table: "KertuElements",
                newName: "IX_KertuElements_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_KertuElements_AspNetUsers_ApplicationUserId",
                table: "KertuElements",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
