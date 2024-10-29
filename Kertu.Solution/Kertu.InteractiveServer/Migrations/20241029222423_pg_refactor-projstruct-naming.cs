using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Kertu.InteractiveServer.Migrations
{
    /// <inheritdoc />
    public partial class pg_refactorprojstructnaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KertuElements");

            migrationBuilder.CreateTable(
                name: "Elements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "varchar(255)", nullable: true),
                    OwnerId = table.Column<string>(type: "varchar(255)", nullable: true),
                    BoardId = table.Column<int>(type: "integer", nullable: true),
                    BoardId1 = table.Column<int>(type: "integer", nullable: true),
                    type = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ListId = table.Column<int>(type: "integer", nullable: true),
                    ListId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Elements_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Elements_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Elements_Elements_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Elements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Elements_Elements_BoardId1",
                        column: x => x.BoardId1,
                        principalTable: "Elements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Elements_Elements_ListId",
                        column: x => x.ListId,
                        principalTable: "Elements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Elements_Elements_ListId1",
                        column: x => x.ListId1,
                        principalTable: "Elements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Elements_ApplicationUserId",
                table: "Elements",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_BoardId",
                table: "Elements",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_BoardId1",
                table: "Elements",
                column: "BoardId1");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_ListId",
                table: "Elements",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_ListId1",
                table: "Elements",
                column: "ListId1");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_OwnerId",
                table: "Elements",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Elements");

            migrationBuilder.CreateTable(
                name: "KertuElements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationUserId = table.Column<string>(type: "varchar(255)", nullable: true),
                    OwnerId = table.Column<string>(type: "varchar(255)", nullable: true),
                    KertuBoardId = table.Column<int>(type: "integer", nullable: true),
                    KertuBoardId1 = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: true),
                    KertuListId = table.Column<int>(type: "integer", nullable: true),
                    KertuListId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KertuElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KertuElements_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KertuElements_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KertuElements_KertuElements_KertuBoardId",
                        column: x => x.KertuBoardId,
                        principalTable: "KertuElements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KertuElements_KertuElements_KertuBoardId1",
                        column: x => x.KertuBoardId1,
                        principalTable: "KertuElements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KertuElements_KertuElements_KertuListId",
                        column: x => x.KertuListId,
                        principalTable: "KertuElements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KertuElements_KertuElements_KertuListId1",
                        column: x => x.KertuListId1,
                        principalTable: "KertuElements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KertuElements_ApplicationUserId",
                table: "KertuElements",
                column: "ApplicationUserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_KertuElements_OwnerId",
                table: "KertuElements",
                column: "OwnerId");
        }
    }
}
