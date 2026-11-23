namespace KampusKitap.ViewModels;

/// <summary>
/// HAFTA 9 — /Kitap/Istatistik sayfasının modeli (Hafta 9 ders notu bölüm 4 ve 5).
/// Her alan tek bir LINQ ifadesinin sonucudur; view hesap yapmaz, yalnızca gösterir.
/// </summary>
public class IstatistikVM
{
    // TODO 9.5 — toplama (aggregate) metotları: her biri listeyi TEK bir değere indirir
    public int KitapSayisi { get; set; }
    public decimal ToplamFiyat { get; set; }
    public decimal OrtalamaFiyat { get; set; }
    public decimal EnUcuz { get; set; }
    public decimal EnPahali { get; set; }
    public int UcuzKitapSayisi { get; set; }      // Count(koşul)
    public bool PahaliVarMi { get; set; }         // Any(koşul)
    public bool HepsiOnayli { get; set; }         // All(koşul)

    // TODO 9.6 — GroupBy: kategori başına bir satır
    public List<KategoriOzetVM> Kategoriler { get; set; } = [];

    // TODO 9.7 — Select + Distinct + OrderBy: tekrarsız, sıralı yazar adları
    public List<string> Yazarlar { get; set; } = [];
}

/// <summary>Gruplamanın bir satırı: bir kategori, kaç kitabı olduğu ve ortalama fiyatı.</summary>
public class KategoriOzetVM
{
    public string Ad { get; set; } = "";
    public int Adet { get; set; }
    public decimal OrtalamaFiyat { get; set; }
}
