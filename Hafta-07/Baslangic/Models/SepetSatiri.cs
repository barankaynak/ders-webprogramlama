namespace KampusKitap.Models;

/// <summary>
/// HAFTA 7 — sepetteki bir satır. Sepetin tamamı session'da
/// List&lt;SepetSatiri&gt; olarak, JSON metni hâlinde durur (Hafta 7 ders notu bölüm 3.5).
///
/// Neden Kitap'ın kendisi değil? Session'a yalnızca gerekeni koyarız: her istekte
/// sunucu belleğinden okunur, yazılır. Başlık ve fiyat eklenme anındaki hâliyle saklanır.
/// </summary>
public class SepetSatiri
{
    public int KitapId { get; set; }
    public string Baslik { get; set; } = "";
    public decimal Fiyat { get; set; }
    public int Adet { get; set; }
}
