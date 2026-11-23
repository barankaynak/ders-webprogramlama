using KampusKitap.Models;

namespace KampusKitap.ViewModels;

/// <summary>
/// HAFTA 9 — /Kitap sayfasının modeli (Hafta 9 ders notu bölüm 3).
///
/// Liste artık tek başına yetmiyor: view'ın hangi kategorinin seçili olduğunu,
/// hangi sıralamanın açık olduğunu ve kaçıncı sayfada olduğunu da bilmesi gerekiyor.
/// Bunları ViewData'ya dağıtmak yerine tek bir ViewModel'de topluyoruz (Hafta 4).
/// </summary>
public class KitapListeVM
{
    /// <summary>Bu sayfada gösterilecek kitaplar — sorgunun SONUCU (ToList), sorgunun kendisi değil.</summary>
    public List<Kitap> Kitaplar { get; set; } = [];

    /// <summary>Seçili kategori; null = hepsi. Query string'den gelir: /Kitap?kategori=2</summary>
    public int? KategoriId { get; set; }

    /// <summary>"fiyat", "fiyat-azalan" ya da null (başlığa göre). /Kitap?sirala=fiyat</summary>
    public string? Sirala { get; set; }

    public int Sayfa { get; set; } = 1;
    public int ToplamSayfa { get; set; } = 1;

    /// <summary>Süzgeçten geçen kitap sayısı (sayfalamadan ÖNCE).</summary>
    public int ToplamKitap { get; set; }
}
