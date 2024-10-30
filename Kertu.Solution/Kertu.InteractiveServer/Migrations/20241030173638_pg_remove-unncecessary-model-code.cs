using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kertu.InteractiveServer.Migrations
{
    /// <inheritdoc />
    public partial class pg_removeunncecessarymodelcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elements_Elements_BoardId1",
                table: "Elements");

            migrationBuilder.DropForeignKey(
                name: "FK_Elements_Elements_ListId1",
                table: "Elements");

            migrationBuilder.DropIndex(
                name: "IX_Elements_BoardId1",
                table: "Elements");

            migrationBuilder.DropIndex(
                name: "IX_Elements_ListId1",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "BoardId1",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "ListId1",
                table: "Elements");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Elements",
                newName: "ElementType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ElementType",
                table: "Elements",
                newName: "type");

            migrationBuilder.AddColumn<int>(
                name: "BoardId1",
                table: "Elements",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ListId1",
                table: "Elements",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Elements_BoardId1",
                table: "Elements",
                column: "BoardId1");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_ListId1",
                table: "Elements",
                column: "ListId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_Elements_BoardId1",
                table: "Elements",
                column: "BoardId1",
                principalTable: "Elements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_Elements_ListId1",
                table: "Elements",
                column: "ListId1",
                principalTable: "Elements",
                principalColumn: "Id");
        }
    }
}
