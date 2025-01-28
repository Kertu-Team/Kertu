using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kertu.InteractiveServer.Migrations
{
    /// <inheritdoc />
    public partial class ListidBoardid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elements_Elements_BoardId",
                table: "Elements");

            migrationBuilder.DropForeignKey(
                name: "FK_Elements_Elements_ListId",
                table: "Elements");

            migrationBuilder.RenameColumn(
                name: "ListId",
                table: "Elements",
                newName: "ListID");

            migrationBuilder.RenameColumn(
                name: "BoardId",
                table: "Elements",
                newName: "BoardID");

            migrationBuilder.RenameIndex(
                name: "IX_Elements_ListId",
                table: "Elements",
                newName: "IX_Elements_ListID");

            migrationBuilder.RenameIndex(
                name: "IX_Elements_BoardId",
                table: "Elements",
                newName: "IX_Elements_BoardID");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_Elements_BoardID",
                table: "Elements",
                column: "BoardID",
                principalTable: "Elements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_Elements_ListID",
                table: "Elements",
                column: "ListID",
                principalTable: "Elements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elements_Elements_BoardID",
                table: "Elements");

            migrationBuilder.DropForeignKey(
                name: "FK_Elements_Elements_ListID",
                table: "Elements");

            migrationBuilder.RenameColumn(
                name: "ListID",
                table: "Elements",
                newName: "ListId");

            migrationBuilder.RenameColumn(
                name: "BoardID",
                table: "Elements",
                newName: "BoardId");

            migrationBuilder.RenameIndex(
                name: "IX_Elements_ListID",
                table: "Elements",
                newName: "IX_Elements_ListId");

            migrationBuilder.RenameIndex(
                name: "IX_Elements_BoardID",
                table: "Elements",
                newName: "IX_Elements_BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_Elements_BoardId",
                table: "Elements",
                column: "BoardId",
                principalTable: "Elements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_Elements_ListId",
                table: "Elements",
                column: "ListId",
                principalTable: "Elements",
                principalColumn: "Id");
        }
    }
}
