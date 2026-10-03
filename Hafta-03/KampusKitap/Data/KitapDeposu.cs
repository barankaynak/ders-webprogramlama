using KampusKitap.Models;

namespace KampusKitap.Data;

/// <summary>
/// Bellekte duran basit veri kaynağı.
/// Gerçek veritabanı Hafta 8'de (EF Core) gelecek; şimdilik görünümlere
/// veri taşıyabilmek için bu yeter.
/// </summary>
public static class KitapDeposu
{
    public static readonly List<Kitap> Hepsi =
    [
        new() { Id = 1, Baslik = "Suç ve Ceza",        Yazar = "Dostoyevski", Fiyat = 120, KategoriId = 1 },
        new() { Id = 2, Baslik = "Tutunamayanlar",     Yazar = "Oğuz Atay",   Fiyat = 180, KategoriId = 1 },
        new() { Id = 3, Baslik = "Veri Yapıları",      Yazar = "A. Yılmaz",   Fiyat = 240, KategoriId = 2 },
        new() { Id = 4, Baslik = "Algoritmalara Giriş", Yazar = "Cormen",     Fiyat = 520, KategoriId = 2 },
        new() { Id = 42, Baslik = "Otostopçunun Galaksi Rehberi", Yazar = "Douglas Adams", Fiyat = 95, KategoriId = 3 }
    ];

    public static Kitap? Bul(int id) => Hepsi.FirstOrDefault(k => k.Id == id);

    public static List<Kitap> Ara(string kelime) =>
        Hepsi.Where(k => k.Baslik.Contains(kelime, StringComparison.OrdinalIgnoreCase)
                      || k.Yazar.Contains(kelime, StringComparison.OrdinalIgnoreCase))
             .ToList();

    public static string KategoriAdi(int kategoriId) => kategoriId switch
    {
        1 => "Edebiyat",
        2 => "Bilgisayar",
        3 => "Bilim kurgu",
        _ => "Diğer"
    };
}
