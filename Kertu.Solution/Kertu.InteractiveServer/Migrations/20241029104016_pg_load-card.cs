using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kertu.InteractiveServer.Migrations
{
    /// <inheritdoc />
    public partial class pg_loadcard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "KertuElements",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "KertuElements");
        }
    }
}
