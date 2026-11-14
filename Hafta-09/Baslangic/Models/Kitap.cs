namespace KampusKitap.Models;

/// <summary>
/// Hafta 3 — view'lara taşınan model.
/// Model "veri neye benziyor?" sorusunu cevaplar (Hafta 2 bölüm 1.1).
///
/// HAFTA 9 — artık bir ENTITY: Kitaplar tablosunun bir satırı (Hafta 9 ders notu bölüm 3).
/// Sınıfa hiçbir attribute eklemedik; EF Core şemayı convention'la çıkarıyor:
///   Id          → primary key (adı "Id" olduğu için), SQLite'ta AUTOINCREMENT
///   string      → TEXT NOT NULL (Nullable açık, string? değil)
///   decimal     → TEXT (SQLite'ın decimal türü yok — ölçüldü)
///   bool        → INTEGER (0/1)
/// </summary>
public class Kitap
{
    public int Id { get; set; }
    public string Baslik { get; set; } = "";
    public string Yazar { get; set; } = "";
    public decimal Fiyat { get; set; }

    // Foreign key. Adı convention'a uyuyor: navigation'ın adı (Kategori) + "Id".
    public int KategoriId { get; set; }

    // HAFTA 9 — bire-çok ilişkinin "bir" tarafı (Hafta 9 ders notu bölüm 5.1).
    // Bilerek nullable: Include yazılmadıysa null gelir (hata vermez) ve entity bir
    // forma bağlanırsa [Required] gibi davranıp IsValid'i düşürmez (Hafta 6 Hata Köşesi — ölçüldü).
    // TODO 9.7 · Bire-çok ilişkiyi kurun: Kitap'a Kategori, Kategori'ye Kitaplar navigation property'si; sonra Add-Migration KategoriIliskisi ve Update-Database
    //      ipucu: Hafta 9 ders notu bölüm 5.1 — migration dosyasında AddForeignKey ve CreateIndex görünür; /Kategori sayfası her kategorinin kitaplarını sayar (Include için TODO 9.8)

    // HAFTA 4 — over-posting gösterimi için.
    // Kullanıcı formda OLMAYAN bu alanı el ile gönderebilir; entity doğrudan
    // action parametresi yapılırsa model binding onu seve seve doldurur.
    public bool Onayli { get; set; }

    // HAFTA 9 — çoka-çok ilişkinin bu tarafı (Hafta 9 ders notu bölüm 5.3).
    // Ara tablo için sınıf yazmıyoruz: EF Core "DersKitap" tablosunu kendisi kurar.
    // TODO 9.9 · Çoka-çok ilişkiyi kurun: Kitap'a Dersler, Ders'e Kitaplar; sonra Add-Migration DersKitap ve Update-Database
    //      ipucu: Hafta 9 ders notu bölüm 5.3 — migration'da DersKitap tablosu (DerslerId + KitaplarId) oluşur; /Ders sayfası her dersin kitaplarını listeler
}
