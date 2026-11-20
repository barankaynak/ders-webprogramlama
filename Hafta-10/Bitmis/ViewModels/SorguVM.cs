namespace KampusKitap.ViewModels;

/// <summary>
/// HAFTA 9 — /Sorgu (LINQ laboratuvarı) sayfasının bir satırı:
/// bir başlık, çalıştırılan kod ve o kodun bu request'te ürettiği sonuç.
/// </summary>
public record LabSatiri(string Baslik, string Kod, string Sonuc);

/// <summary>
/// HAFTA 9 — 🕳️ Klasik Tuzak sayfasının kartı (Hafta 9 ders notu, Klasik Tuzak).
/// Select'in ürettiği YENİ nesne: kitabın kendisi değil, ondan yapılmış bir özet.
/// </summary>
public class IndirimKartiVM
{
    public string Baslik { get; set; } = "";
    public decimal Fiyat { get; set; }
    public bool Indirimde { get; set; }
}
