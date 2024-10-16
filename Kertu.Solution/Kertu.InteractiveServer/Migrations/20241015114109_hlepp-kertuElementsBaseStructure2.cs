using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kertu.InteractiveServer.Migrations
{
    /// <inheritdoc />
    public partial class hleppkertuElementsBaseStructure2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KertuBoardId",
                table: "KertuElements",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KertuBoardId1",
                table: "KertuElements",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KertuListId",
                table: "KertuElements",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KertuListId1",
                table: "KertuElements",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KertuElements_KertuBoardId",
                table: "KertuElements",
                column: "KertuBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_KertuElements_KertuBoardId1",
                table: "KertuElements",
                column: "KertuBoardId1");

            migrationBuilder.CreateIndex(
                name: "IX_KertuElements_KertuListId",
                table: "KertuElements",
                column: "KertuListId");

            migrationBuilder.CreateIndex(
                name: "IX_KertuElements_KertuListId1",
                table: "KertuElements",
                column: "KertuListId1");

            migrationBuilder.AddForeignKey(
                name: "FK_KertuElements_KertuElements_KertuBoardId",
                table: "KertuElements",
                column: "KertuBoardId",
                principalTable: "KertuElements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KertuElements_KertuElements_KertuBoardId1",
                table: "KertuElements",
                column: "KertuBoardId1",
                principalTable: "KertuElements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KertuElements_KertuElements_KertuListId",
                table: "KertuElements",
                column: "KertuListId",
                principalTable: "KertuElements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KertuElements_KertuElements_KertuListId1",
                table: "KertuElements",
                column: "KertuListId1",
                principalTable: "KertuElements",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KertuElements_KertuElements_KertuBoardId",
                table: "KertuElements");

            migrationBuilder.DropForeignKey(
                name: "FK_KertuElements_KertuElements_KertuBoardId1",
                table: "KertuElements");

            migrationBuilder.DropForeignKey(
                name: "FK_KertuElements_KertuElements_KertuListId",
                table: "KertuElements");

            migrationBuilder.DropForeignKey(
                name: "FK_KertuElements_KertuElements_KertuListId1",
                table: "KertuElements");

            migrationBuilder.DropIndex(
                name: "IX_KertuElements_KertuBoardId",
                table: "KertuElements");

            migrationBuilder.DropIndex(
                name: "IX_KertuElements_KertuBoardId1",
                table: "KertuElements");

            migrationBuilder.DropIndex(
                name: "IX_KertuElements_KertuListId",
                table: "KertuElements");

            migrationBuilder.DropIndex(
                name: "IX_KertuElements_KertuListId1",
                table: "KertuElements");

            migrationBuilder.DropColumn(
                name: "KertuBoardId",
                table: "KertuElements");

            migrationBuilder.DropColumn(
                name: "KertuBoardId1",
                table: "KertuElements");

            migrationBuilder.DropColumn(
                name: "KertuListId",
                table: "KertuElements");

            migrationBuilder.DropColumn(
                name: "KertuListId1",
                table: "KertuElements");
        }
    }
}
