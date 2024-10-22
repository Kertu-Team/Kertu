using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kertu.InteractiveServer.Migrations
{
    /// <inheritdoc />
    public partial class deviance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "KertuElements",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "taskname",
                table: "KertuElements",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "KertuElements");

            migrationBuilder.DropColumn(
                name: "taskname",
                table: "KertuElements");
        }
    }
}
