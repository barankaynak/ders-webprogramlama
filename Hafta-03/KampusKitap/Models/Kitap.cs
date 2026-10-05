namespace KampusKitap.Models;

/// <summary>
/// Hafta 3 — görünümlere taşınan model.
/// Model "veri neye benziyor?" sorusunu cevaplar (Hafta 2 bölüm 1.1).
/// </summary>
public class Kitap
{
    public int Id { get; set; }
    public string Baslik { get; set; } = "";
    public string Yazar { get; set; } = "";
    public decimal Fiyat { get; set; }
    public int KategoriId { get; set; }
}
