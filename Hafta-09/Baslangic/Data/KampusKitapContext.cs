using KampusKitap.Models;
using Microsoft.EntityFrameworkCore;

namespace KampusKitap.Data;

/// <summary>
/// HAFTA 9 — uygulamanın veritabanına açılan tek kapısı (Hafta 9 ders notu bölüm 2).
///
/// DbContext üç iş yapar: veritabanı bağlantısını tutar, okuduğu nesneleri İZLER
/// (change tracking) ve SaveChanges() çağrılınca biriken değişiklikleri SQL'e çevirir.
/// Her DbSet bir tabloya karşılık gelir.
///
/// Bellekteki KitapDeposu'nun yerini aldı. Veri artık uygulama kapanınca kaybolmuyor:
/// kampuskitap.db dosyasında duruyor.
/// </summary>
public class KampusKitapContext : DbContext
{
    // Hangi provider, hangi bağlantı dizesi? Bunları sınıf bilmez; DI verir
    // (Program.cs'teki AddDbContext — TODO 9.1). Hafta 2'nin kurucu enjeksiyonu.
    public KampusKitapContext(DbContextOptions<KampusKitapContext> options) : base(options) { }

    public DbSet<Kitap> Kitaplar => Set<Kitap>();
    public DbSet<Kategori> Kategoriler => Set<Kategori>();
    public DbSet<Ders> Dersler => Set<Ders>();

    // Bu sınıf ve entity'ler değiştikçe şema da değişir. Değişikliği veritabanına
    // taşıyan şey migration'dır: önce Add-Migration (kodu üretir), sonra Update-Database (uygular).
    // TODO 9.2 · İlk migration'ı üretin ve veritabanına uygulayın
    //      ipucu: Hafta 9 ders notu bölüm 4.1 — Package Manager Console'da `Add-Migration Ilk`, sonra `Update-Database` (CLI: `dotnet ef migrations add Ilk`, `dotnet ef database update`). Migrations klasörü ve kampuskitap.db oluşur; /Kategori üç kategoriyi listeler

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seeding: başlangıç verisi (Hafta 9 ders notu bölüm 4.4).
        // HasData'daki satırlar migration'a InsertData olarak yazılır; veritabanı
        // ilk kurulduğunda tablolar boş kalmaz. Id'leri biz veririz.
        modelBuilder.Entity<Kategori>().HasData(
            new Kategori { Id = 1, Ad = "Edebiyat" },
            new Kategori { Id = 2, Ad = "Bilgisayar" },
            new Kategori { Id = 3, Ad = "Bilim kurgu" });

        modelBuilder.Entity<Kitap>().HasData(
            new Kitap { Id = 1, Baslik = "Suç ve Ceza", Yazar = "Dostoyevski", Fiyat = 120, KategoriId = 1 },
            new Kitap { Id = 2, Baslik = "Tutunamayanlar", Yazar = "Oğuz Atay", Fiyat = 180, KategoriId = 1 },
            new Kitap { Id = 3, Baslik = "Veri Yapıları", Yazar = "A. Yılmaz", Fiyat = 240, KategoriId = 2 },
            new Kitap { Id = 4, Baslik = "Algoritmalara Giriş", Yazar = "Cormen", Fiyat = 520, KategoriId = 2 },
            new Kitap { Id = 42, Baslik = "Otostopçunun Galaksi Rehberi", Yazar = "Douglas Adams", Fiyat = 95, KategoriId = 3 });

        modelBuilder.Entity<Ders>().HasData(
            new Ders { Id = 1, Kod = "BSM201", Ad = "Veri Yapıları" },
            new Ders { Id = 2, Kod = "BSM305", Ad = "Web Programlama" },
            new Ders { Id = 3, Kod = "BSM302", Ad = "Algoritma Analizi" });

        // Çoka-çok ilişkinin başlangıç verisi: ara tablonun (DersKitap) satırları.
        // Ara tablonun sınıfı olmadığı için satırlar anonim nesneyle yazılır.
        // TODO 9.9 · Çoka-çok ilişkiyi kurun: Kitap'a Dersler, Ders'e Kitaplar; sonra Add-Migration DersKitap ve Update-Database
    }
}
