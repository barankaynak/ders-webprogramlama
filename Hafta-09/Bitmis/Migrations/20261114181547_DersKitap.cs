using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KampusKitap.Migrations
{
    /// <inheritdoc />
    public partial class DersKitap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DersKitap",
                columns: table => new
                {
                    DerslerId = table.Column<int>(type: "INTEGER", nullable: false),
                    KitaplarId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DersKitap", x => new { x.DerslerId, x.KitaplarId });
                    table.ForeignKey(
                        name: "FK_DersKitap_Dersler_DerslerId",
                        column: x => x.DerslerId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DersKitap_Kitaplar_KitaplarId",
                        column: x => x.KitaplarId,
                        principalTable: "Kitaplar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DersKitap",
                columns: new[] { "DerslerId", "KitaplarId" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 1, 4 },
                    { 3, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DersKitap_KitaplarId",
                table: "DersKitap",
                column: "KitaplarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DersKitap");
        }
    }
}
