using System.ComponentModel.DataAnnotations;
using KampusKitap.Validation;

namespace KampusKitap.ViewModels;

/// <summary>
/// HAFTA 4 — ViewModel (ViewModel), Hafta 4 ders notu bölüm 4.4.
///
/// Formun alabileceği alanlar BURADA, açıkça yazılıdır. Entity'yi (Kitap)
/// doğrudan action parametresi yapsaydık kullanıcı formda olmayan alanları da
/// gönderebilirdi — over-posting (Hafta 4 ders notu bölüm 4.3).
///
/// HAFTA 6 — kurallar da burada. İki form (/Kitap/Ekle ve /Kitap/EkleTH) aynı
/// ViewModel'i kullandığı için aynı kurallara tabidir: kural GÖRÜNÜMDE değil,
/// MODELDE yazılır (Hafta 6 ders notu bölüm 2).
/// </summary>
public class KitapEkleVM : IValidatableObject
{
    // [Display] bir DOĞRULAMA attribute'ü değildir; yalnızca adı belirler.
    // {0} yer tutucusu o adla dolar: "Başlık boş bırakılamaz."
    // TODO 6.1 · Başlık ve Yazar için doğrulama attribute'lerini ekleyin: `[Required]` ve `[StringLength]`, Türkçe `ErrorMessage` ile
    //      ipucu: Hafta 6 ders notu bölüm 2.2 — `{0}` alanın adı, `{1}` üst sınır, `{2}` alt sınırdır. Boş gönderin; `/Kitap/EkleTH`'deki hata listesinde mesajınız görünmeli
    [BuyukHarfleBaslar]
    [Display(Name = "Başlık")]
    public string Baslik { get; set; } = "";

    // TODO 6.1 · Başlık ve Yazar için doğrulama attribute'lerini ekleyin: `[Required]` ve `[StringLength]`, Türkçe `ErrorMessage` ile
    [Display(Name = "Yazar")]
    public string Yazar { get; set; } = "";

    // HAFTA 6 — Fiyat ve KategoriId bilerek NULLABLE (decimal?, int?).
    // Boş kutu ve "— seçiniz —" null olarak bağlanır; [Required] ancak null'u
    // yakalayabilir. decimal/int olsaydı boş değer BAĞLAMADA düşer, bizim
    // mesajımız hiç görünmezdi: "The value '' is invalid." (bölüm 1.3)
    // TODO 6.2 · Fiyat ve kategori için `[Required]` ve `[Range]` ekleyin
    //      ipucu: Hafta 6 ders notu bölüm 2.2 — `decimal?` için `[Range(1.0, 10000.0, …)]`; kategori 1, 2 ya da 3 olmalı. TODO'dan önce boş fiyatla gönderin: 0 TL'lik kitap eklenir
    [Display(Name = "Fiyat (TL)")]
    public decimal? Fiyat { get; set; }

    // TODO 6.2 · Fiyat ve kategori için `[Required]` ve `[Range]` ekleyin
    [Display(Name = "Kategori")]
    public int? KategoriId { get; set; }

    // HAFTA 6 — İKİ ALANA BİRDEN bakan kural. Attribute'ler tek bir özelliği
    // görür; birden çok özelliği birlikte denetlemek IValidatableObject'in işidir.
    //
    // Çatı bu metodu YALNIZCA özellik düzeyindeki bütün kurallar geçtiyse çağırır
    // (Hafta 6 ders notu bölüm 5.3) — /Dogrula/Sira sayfasında ölçüldü.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // TODO 6.7 · Başlık ile yazar aynı olamaz: `Validate` gövdesini yazın
        //      ipucu: Hafta 6 ders notu bölüm 5.3 — `yield return new ValidationResult("…", [nameof(Baslik), nameof(Yazar)])`. Büyük-küçük harf farkını yok sayın
        yield break;   // şimdilik hiçbir kural yok
    }
}
