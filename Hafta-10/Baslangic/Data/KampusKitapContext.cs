using KampusKitap.Models;
using Microsoft.EntityFrameworkCore;

namespace KampusKitap.Data;

/// <summary>
/// HAFTA 10 — uygulamanın veritabanına açılan tek kapısı (Hafta 10 ders notu bölüm 4.3).
///
/// DbContext üç iş yapar: veritabanı bağlantısını tutar, okuduğu nesneleri İZLER
/// (change tracking) ve SaveChanges() çağrılınca biriken değişiklikleri SQL'e çevirir.
/// Her DbSet bir tabloya karşılık gelir.
///
/// KitapDeposu.Hepsi listesinin yerini aldı. Veri artık uygulama kapanınca kaybolmuyor:
/// kampuskitap.db dosyasında duruyor.
/// </summary>
public class KampusKitapContext : DbContext
{
    // Hangi provider, hangi bağlantı dizesi? Bunları sınıf bilmez; DI verir
    // (Program.cs'teki AddDbContext — TODO 10.1). Hafta 2'nin kurucu enjeksiyonu.
    public KampusKitapContext(DbContextOptions<KampusKitapContext> options) : base(options) { }

    // Kitap sınıfı → Kitaplar tablosu. Tablonun adını bu özelliğin adı verir.
    public DbSet<Kitap> Kitaplar => Set<Kitap>();

    // Kategoriler bu hafta tabloya geçmiyor: KitapDeposu.Kategoriler'de duruyorlar.
    // Kategori tablosu ve kitapla ilişkisi Hafta 11'in konusu.

    // Bu sınıf ve entity değiştikçe şema da değişir. Değişikliği veritabanına
    // taşıyan şey migration'dır: önce Add-Migration (kodu üretir), sonra Update-Database (uygular).
    // TODO 10.2 · İlk migration'ı üretin ve veritabanına uygulayın
    //      ipucu: Hafta 10 ders notu bölüm 5.2 — Package Manager Console'da `Add-Migration Ilk`, sonra `Update-Database` (CLI: `dotnet ef migrations add Ilk`, `dotnet ef database update`). Migrations klasörü ve kampuskitap.db oluşur; /Kitap/Istatistik 11 kitabı sayar

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seeding: başlangıç verisi (Hafta 10 ders notu bölüm 5.5).
        // Hafta 9'daki KitapDeposu.Hepsi listesinin AYNI 11 kitabı.
        // HasData'daki satırlar migration'a InsertData olarak yazılır; veritabanı
        // ilk kurulduğunda tablo boş kalmaz. Id'leri biz veririz.
        modelBuilder.Entity<Kitap>().HasData(
            new Kitap { Id = 1,  Baslik = "Suç ve Ceza",          Yazar = "Dostoyevski",      Fiyat = 120, KategoriId = 1 },
            new Kitap { Id = 2,  Baslik = "Tutunamayanlar",       Yazar = "Oğuz Atay",        Fiyat = 180, KategoriId = 1 },
            new Kitap { Id = 3,  Baslik = "Veri Yapıları",        Yazar = "A. Yılmaz",        Fiyat = 240, KategoriId = 2 },
            new Kitap { Id = 4,  Baslik = "Algoritmalara Giriş",  Yazar = "Cormen",           Fiyat = 520, KategoriId = 2 },
            new Kitap { Id = 42, Baslik = "Otostopçunun Galaksi Rehberi", Yazar = "Douglas Adams", Fiyat = 95, KategoriId = 3 },
            new Kitap { Id = 5,  Baslik = "Karamazov Kardeşler",  Yazar = "Dostoyevski",      Fiyat = 210, KategoriId = 1 },
            new Kitap { Id = 6,  Baslik = "Tehlikeli Oyunlar",    Yazar = "Oğuz Atay",        Fiyat = 165, KategoriId = 1 },
            new Kitap { Id = 7,  Baslik = "İnce Memed",           Yazar = "Yaşar Kemal",      Fiyat = 145, KategoriId = 1 },
            new Kitap { Id = 8,  Baslik = "Temiz Kod",            Yazar = "Robert C. Martin", Fiyat = 340, KategoriId = 2 },
            new Kitap { Id = 9,  Baslik = "Vakıf",                Yazar = "Isaac Asimov",     Fiyat = 150, KategoriId = 3 },
            new Kitap { Id = 10, Baslik = "Ben, Robot",           Yazar = "Isaac Asimov",     Fiyat = 110, KategoriId = 3 });
    }
}
