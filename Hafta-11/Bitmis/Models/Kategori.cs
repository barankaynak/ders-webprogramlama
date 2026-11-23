namespace KampusKitap.Models;

/// <summary>
/// HAFTA 11 — Kategoriler tablosu (Hafta 11 ders notu bölüm 1).
/// Hafta 10'a kadar kategoriler KitapDeposu'nda İKİ kez yazılıydı: bir sözlükte ve bir
/// switch'te. Biri değişip öteki unutulsa sayfalar birbirini tutmazdı. Artık tek yer: bu tablo.
/// </summary>
public class Kategori
{
    public int Id { get; set; }
    public string Ad { get; set; } = "";

    // Bire-çok ilişkinin "çok" tarafı: bir kategorinin birden çok kitabı olur.
    // Koleksiyon navigation'ı: sütun DEĞİLDİR, tabloda karşılığı yok (Hafta 11 ders notu bölüm 1.3).
    // ✔ TODO 11.1 · Bire-çok ilişkiyi kurun: Kitap'a Kategori, Kategori'ye Kitaplar navigation property'si
    public List<Kitap> Kitaplar { get; set; } = [];
}
