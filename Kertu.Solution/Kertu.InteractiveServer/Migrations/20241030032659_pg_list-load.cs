using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kertu.InteractiveServer.Migrations
{
    /// <inheritdoc />
    public partial class pg_listload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTask",
                table: "Elements",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTask",
                table: "Elements");
        }
    }
}
