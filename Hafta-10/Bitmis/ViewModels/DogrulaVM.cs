using System.ComponentModel.DataAnnotations;

namespace KampusKitap.ViewModels;

// =====================================================================
//  HAFTA 6 — /Dogrula sayfalarının gösteri sınıfları.
//
//  Hepsi ÖLÇÜM içindir: her biri bir soruyu cevaplıyor ve sayfalar
//  ModelState'in içini tablo hâlinde basıyor. Mesajlar bilerek
//  VARSAYILAN bırakıldı — çatının kendi cümlesini görmek için.
// =====================================================================

/// <summary>
/// /Dogrula — hazır doğrulama attribute'lerinin galerisi
/// (Hafta 6 ders notu bölüm 2).
/// </summary>
public class UyeVM
{
    [Required]
    [StringLength(30, MinimumLength = 2)]
    [Display(Name = "Ad")]
    public string? Ad { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "E-posta")]
    public string? Eposta { get; set; }

    [Range(18, 99)]
    [Display(Name = "Yaş")]
    public int Yas { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Parola")]
    public string? Parola { get; set; }

    [Compare(nameof(Parola))]
    [DataType(DataType.Password)]
    [Display(Name = "Parola (tekrar)")]
    public string? ParolaTekrar { get; set; }

    [RegularExpression(@"^05\d{9}$")]
    [Display(Name = "Cep telefonu")]
    public string? Telefon { get; set; }

    [Url]
    [Display(Name = "Web sitesi")]
    public string? WebSitesi { get; set; }

    // Ondalık sayı: "1,75" — istemci ile sunucu aynı biçimi konuşmalı (bölüm 4.4)
    [Range(0.5, 2.5)]
    [Display(Name = "Boy (m)")]
    public decimal? Boy { get; set; }
}

/// <summary>
/// /Dogrula/Ortuk — hiçbir doğrulama attribute'ü YOK. Gelen hatalar tipten
/// ve nullable işaretinden geliyor (Hafta 6 ders notu bölüm 1.3).
/// </summary>
public class OrtukVM
{
    public int Sayi { get; set; }
    public int? SayiNullable { get; set; }
    public string Metin { get; set; } = "";
    public string? MetinNullable { get; set; }

    // İkisi de e-posta gibi görünür; yalnızca biri DOĞRULAR.
    [DataType(DataType.EmailAddress)]
    public string? EpostaDataType { get; set; }

    [EmailAddress]
    public string? EpostaAttribute { get; set; }
}

/// <summary>
/// /Dogrula/Tuzak — 🐛 "ModelState.IsValid hep false" (Hafta 6 Hata Köşesi).
/// Formda yalnızca Baslik var; diğer iki özellik hiç gönderilmiyor.
/// </summary>
public class TuzakVM
{
    [Required]
    public int Id { get; set; }                    // formda YOK — int: hep 0

    public string Baslik { get; set; } = "";       // formda VAR

    public YayineviVM Yayinevi { get; set; } = null!;   // formda YOK — gezinme özelliği gibi
}

public class YayineviVM
{
    public int Id { get; set; }
    public string Ad { get; set; } = "";
}

/// <summary>
/// /Dogrula/Sira — IValidatableObject NE ZAMAN çalışır?
/// İki alan da boşsa "aynı" sayılırlar; Validate çağrılsaydı hata verirdi.
/// </summary>
public class SiraVM : IValidatableObject
{
    [Required(ErrorMessage = "Ad boş bırakılamaz.")]
    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Ad == Soyad)
            yield return new ValidationResult("Validate çalıştı: ad ile soyad aynı olamaz.",
                [nameof(Ad), nameof(Soyad)]);
    }
}
