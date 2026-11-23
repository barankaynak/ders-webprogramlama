using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KampusKitap.Migrations
{
    /// <inheritdoc />
    public partial class Ilk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kitaplar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Baslik = table.Column<string>(type: "TEXT", nullable: false),
                    Yazar = table.Column<string>(type: "TEXT", nullable: false),
                    Fiyat = table.Column<decimal>(type: "TEXT", nullable: false),
                    KategoriId = table.Column<int>(type: "INTEGER", nullable: false),
                    Onayli = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kitaplar", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Kitaplar",
                columns: new[] { "Id", "Baslik", "Fiyat", "KategoriId", "Onayli", "Yazar" },
                values: new object[,]
                {
                    { 1, "Suç ve Ceza", 120m, 1, false, "Dostoyevski" },
                    { 2, "Tutunamayanlar", 180m, 1, false, "Oğuz Atay" },
                    { 3, "Veri Yapıları", 240m, 2, false, "A. Yılmaz" },
                    { 4, "Algoritmalara Giriş", 520m, 2, false, "Cormen" },
                    { 5, "Karamazov Kardeşler", 210m, 1, false, "Dostoyevski" },
                    { 6, "Tehlikeli Oyunlar", 165m, 1, false, "Oğuz Atay" },
                    { 7, "İnce Memed", 145m, 1, false, "Yaşar Kemal" },
                    { 8, "Temiz Kod", 340m, 2, false, "Robert C. Martin" },
                    { 9, "Vakıf", 150m, 3, false, "Isaac Asimov" },
                    { 10, "Ben, Robot", 110m, 3, false, "Isaac Asimov" },
                    { 42, "Otostopçunun Galaksi Rehberi", 95m, 3, false, "Douglas Adams" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kitaplar");
        }
    }
}
