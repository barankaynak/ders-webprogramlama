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
    // {0} placeholder'ı o adla dolar: "Başlık boş bırakılamaz."
    // ✔ TODO 6.1 · Başlık ve Yazar için doğrulama attribute'lerini ekleyin: `[Required]` ve `[StringLength]`, Türkçe `ErrorMessage` ile
    [Required(ErrorMessage = "{0} boş bırakılamaz.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "{0} {2} ile {1} karakter arasında olmalıdır.")]
    [BuyukHarfleBaslar]
    [Display(Name = "Başlık")]
    public string Baslik { get; set; } = "";

    // ✔ TODO 6.1 · Başlık ve Yazar için doğrulama attribute'lerini ekleyin: `[Required]` ve `[StringLength]`, Türkçe `ErrorMessage` ile
    [Required(ErrorMessage = "{0} boş bırakılamaz.")]
    [StringLength(60, ErrorMessage = "{0} en fazla {1} karakter olabilir.")]
    [Display(Name = "Yazar")]
    public string Yazar { get; set; } = "";

    // HAFTA 6 — Fiyat ve KategoriId bilerek NULLABLE (decimal?, int?).
    // Boş kutu ve "— seçiniz —" null olarak bağlanır; [Required] ancak null'u
    // yakalayabilir. decimal/int olsaydı boş değer BAĞLAMADA düşer, bizim
    // mesajımız hiç görünmezdi: "The value '' is invalid." (bölüm 1.3)
    // ✔ TODO 6.2 · Fiyat ve kategori için `[Required]` ve `[Range]` ekleyin
    [Required(ErrorMessage = "{0} boş bırakılamaz.")]
    [Range(1.0, 10000.0, ErrorMessage = "{0} {1} ile {2} arasında olmalıdır.")]
    [Display(Name = "Fiyat (TL)")]
    public decimal? Fiyat { get; set; }

    // ✔ TODO 6.2 · Fiyat ve kategori için `[Required]` ve `[Range]` ekleyin
    [Required(ErrorMessage = "Bir kategori seçin.")]
    [Range(1, 3, ErrorMessage = "Geçerli bir kategori seçin.")]
    [Display(Name = "Kategori")]
    public int? KategoriId { get; set; }

    // HAFTA 6 — İKİ ALANA BİRDEN bakan kural. Attribute'ler tek bir özelliği
    // görür; birden çok özelliği birlikte denetlemek IValidatableObject'in işidir.
    //
    // Çatı bu metodu YALNIZCA özellik düzeyindeki bütün kurallar geçtiyse çağırır
    // (Hafta 6 ders notu bölüm 5.3) — /Dogrula/Sira sayfasında ölçüldü.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // ✔ TODO 6.7 · Başlık ile yazar aynı olamaz: `Validate` gövdesini yazın
        if (string.Equals((Baslik ?? "").Trim(), (Yazar ?? "").Trim(),
                StringComparison.CurrentCultureIgnoreCase))
            yield return new ValidationResult("Başlık ile yazar aynı olamaz.",
                [nameof(Baslik), nameof(Yazar)]);
    }
}
