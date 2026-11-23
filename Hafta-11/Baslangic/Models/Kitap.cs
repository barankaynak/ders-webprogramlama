namespace KampusKitap.Models;

/// <summary>
/// Hafta 3 — view'lara taşınan model.
/// Model "veri neye benziyor?" sorusunu cevaplar (Hafta 2 bölüm 1.1).
///
/// HAFTA 11 — Kategori ile bire-çok ilişkisi var (KategoriId + Kategori).
/// HAFTA 10 — artık bir ENTITY: Kitaplar tablosunun bir satırı (Hafta 10 ders notu bölüm 5.1).
/// Sınıfa hiçbir attribute eklemedik; EF Core şemayı convention'la çıkarıyor:
///   Id          → primary key (adı "Id" olduğu için), SQLite'ta AUTOINCREMENT
///   string      → TEXT NOT NULL (Nullable açık, string? değil)
///   decimal     → TEXT (SQLite'ın decimal türü yok — ölçüldü)
///   int, bool   → INTEGER (bool 0/1 olarak)
/// </summary>
public class Kitap
{
    public int Id { get; set; }
    public string Baslik { get; set; } = "";
    public string Yazar { get; set; } = "";
    public decimal Fiyat { get; set; }

    // HAFTA 11 — foreign key: Kategoriler tablosundaki bir satırın Id'si (Hafta 11 ders notu bölüm 1.2).
    // Adı convention'a uyuyor: navigation'ın adı (Kategori) + "Id". Hafta 10'da düz bir sayıydı.
    public int KategoriId { get; set; }

    // HAFTA 11 — bire-çok ilişkinin "bir" tarafı: kitabın kategorisi (Hafta 11 ders notu bölüm 1.3).
    // Bilerek nullable: Include yazılmadıysa null gelir, hata vermez (bölüm 1.5).
    // TODO 11.1 · Bire-çok ilişkiyi kurun: Kitap'a Kategori, Kategori'ye Kitaplar navigation property'si

    // HAFTA 4 — over-posting gösterimi için.
    // Kullanıcı formda OLMAYAN bu alanı el ile gönderebilir; entity doğrudan
    // action parametresi yapılırsa model binding onu seve seve doldurur.
    public bool Onayli { get; set; }
}
