namespace KampusKitap.Data;

/// <summary>
/// Hafta 3'ten beri bellekte duran veri kaynağı.
///
/// HAFTA 10 — kitaplar buradan çıktı: artık veritabanında (KampusKitapContext,
/// Hafta 10 ders notu bölüm 4). Aynı 11 kitap seed verisi olarak taşındı.
/// Burada yalnızca kategoriler kaldı; onlar da Hafta 11'de tabloya geçecek.
///
/// Fark edin: aynı üç ad İKİ kez yazılı (sözlük ve switch). Biri değişip diğeri
/// unutulursa sayfalar birbirini tutmaz. Hafta 11'de tek yer olacak: Kategoriler tablosu.
/// </summary>
public static class KitapDeposu
{
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
