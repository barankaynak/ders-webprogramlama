namespace KampusKitap.Models;

/// <summary>
/// HAFTA 9 — Dersler tablosu (Hafta 9 ders notu bölüm 5.3).
/// Bir derste birden çok kitap okutulur, bir kitap birden çok derste okutulur:
/// çoka-çok (N-N). İlişkiyi iki taraftaki liste kurar (TODO 9.9).
/// </summary>
public class Ders
{
    public int Id { get; set; }
    public string Kod { get; set; } = "";
    public string Ad { get; set; } = "";

    // TODO 9.9 · Çoka-çok ilişkiyi kurun: Kitap'a Dersler, Ders'e Kitaplar; sonra Add-Migration DersKitap ve Update-Database
}
