namespace KampusKitap.ViewModels;

/// <summary>HAFTA 9 — /Kategori listesinin bir satırı: kategori ve kaç kitabı olduğu (TODO 9.8).</summary>
public class KategoriSatiriVM
{
    public int Id { get; set; }
    public string Ad { get; set; } = "";
    public int Adet { get; set; }
}
