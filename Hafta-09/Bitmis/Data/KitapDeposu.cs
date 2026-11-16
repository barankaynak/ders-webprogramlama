using KampusKitap.Models;

namespace KampusKitap.Data;

/// <summary>
/// Bellekte duran basit veri kaynağı.
/// Gerçek veritabanı Hafta 10'da (EF Core) gelecek.
///
/// HAFTA 9 — bu sınıfın üç metodu Hafta 3'ten beri LINQ kullanıyor: Max, FirstOrDefault,
/// Where. Bu hafta onları okuyabilir hâle geliyoruz (Hafta 9 ders notu bölüm 1.3).
/// Liste beş kitaptan on bire çıktı: sıralama, gruplama ve sayfalama beş kitapla görünmez.
/// </summary>
public static class KitapDeposu
{
    public static readonly List<Kitap> Hepsi =
    [
        new() { Id = 1, Baslik = "Suç ve Ceza",        Yazar = "Dostoyevski", Fiyat = 120, KategoriId = 1 },
        new() { Id = 2, Baslik = "Tutunamayanlar",     Yazar = "Oğuz Atay",   Fiyat = 180, KategoriId = 1 },
        new() { Id = 3, Baslik = "Veri Yapıları",      Yazar = "A. Yılmaz",   Fiyat = 240, KategoriId = 2 },
        new() { Id = 4, Baslik = "Algoritmalara Giriş", Yazar = "Cormen",     Fiyat = 520, KategoriId = 2 },
        new() { Id = 42, Baslik = "Otostopçunun Galaksi Rehberi", Yazar = "Douglas Adams", Fiyat = 95, KategoriId = 3 },
        // HAFTA 9 — aynı yazardan ikinci kitaplar (Distinct, GroupBy) ve "İ" ile başlayan bir başlık (arama)
        new() { Id = 5, Baslik = "Karamazov Kardeşler",  Yazar = "Dostoyevski",      Fiyat = 210, KategoriId = 1 },
        new() { Id = 6, Baslik = "Tehlikeli Oyunlar",    Yazar = "Oğuz Atay",        Fiyat = 165, KategoriId = 1 },
        new() { Id = 7, Baslik = "İnce Memed",           Yazar = "Yaşar Kemal",      Fiyat = 145, KategoriId = 1 },
        new() { Id = 8, Baslik = "Temiz Kod",            Yazar = "Robert C. Martin", Fiyat = 340, KategoriId = 2 },
        new() { Id = 9, Baslik = "Vakıf",                Yazar = "Isaac Asimov",     Fiyat = 150, KategoriId = 3 },
        new() { Id = 10, Baslik = "Ben, Robot",          Yazar = "Isaac Asimov",     Fiyat = 110, KategoriId = 3 }
    ];

    /// <summary>HAFTA 4 — form verisinin gideceği yer. Hafta 10'da (EF Core) veritabanı olacak.</summary>
    public static Kitap Ekle(Kitap kitap)
    {
        kitap.Id = Hepsi.Count == 0 ? 1 : Hepsi.Max(k => k.Id) + 1;
        Hepsi.Add(kitap);
        return kitap;
    }

    public static Kitap? Bul(int id) => Hepsi.FirstOrDefault(k => k.Id == id);

    // HAFTA 9 — karşılaştırma Ordinal'den CurrentCulture'a geçti (Hafta 9 ders notu, Sorun Çözme Köşesi).
    // OrdinalIgnoreCase Türkçe harfleri tanımaz: "ince" araması "İnce Memed"i BULMAZ (ölçüldü).
    // Uygulama tr-TR çalıştığı için (Program.cs, UseRequestLocalization) CurrentCulture doğru eşler.
    public static List<Kitap> Ara(string kelime) =>
        Hepsi.Where(k => k.Baslik.Contains(kelime, StringComparison.CurrentCultureIgnoreCase)
                      || k.Yazar.Contains(kelime, StringComparison.CurrentCultureIgnoreCase))
             .ToList();

    /// <summary>HAFTA 5 — &lt;select asp-items&gt; için kaynak liste.</summary>
    public static readonly Dictionary<int, string> Kategoriler = new()
    {
        [1] = "Edebiyat",
        [2] = "Bilgisayar",
        [3] = "Bilim kurgu"
    };

    public static string KategoriAdi(int kategoriId) => kategoriId switch
    {
        1 => "Edebiyat",
        2 => "Bilgisayar",
        3 => "Bilim kurgu",
        _ => "Diğer"
    };
}
