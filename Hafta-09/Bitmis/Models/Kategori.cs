namespace KampusKitap.Models;

/// <summary>
/// HAFTA 9 — Kategoriler tablosu (Hafta 9 ders notu bölüm 3).
/// Hafta 9'a kadar kategoriler İKİ yerde yaşıyordu: KitapDeposu'ndaki üç ad ve
/// KategoriController'daki beş ad. İkisi birbirini tutmuyordu. Artık tek yer: bu tablo.
/// </summary>
public class Kategori
{
    public int Id { get; set; }
    public string Ad { get; set; } = "";

    // Bire-çok ilişkinin "çok" tarafı: bir kategorinin birden çok kitabı olur.
    // ✔ TODO 9.7 · Bire-çok ilişkiyi kurun: Kitap'a Kategori, Kategori'ye Kitaplar navigation property'si; sonra Add-Migration KategoriIliskisi ve Update-Database
    public List<Kitap> Kitaplar { get; set; } = [];
}
