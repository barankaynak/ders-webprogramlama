using System.ComponentModel.DataAnnotations;

namespace KampusKitap.ViewModels;

/// <summary>
/// HAFTA 5 — <c>asp-for</c>'un ÜRETTİĞİ HTML'i görmek için kurulmuş gösteri sınıfı
/// (Hafta 5 ders notu bölüm 3).
///
/// Buradaki her özellik farklı bir soruyu cevaplıyor:
/// tip nasıl <c>type</c>'a dönüşüyor, attribute'ler HTML'e nasıl yansıyor,
/// iç içe nesne hangi <c>name</c>'i alıyor?
///
/// NOT: DataAnnotations'ın DOĞRULAMA tarafı Hafta 6'nın konusudur. Bu hafta
/// yalnızca ÜRETİLEN HTML'e bakıyoruz.
/// </summary>
public class EtiketVM
{
    // string (nullable DEĞİL) → type="text" + data-val-required
    public string Metin { get; set; } = "";

    // string? → aynı type="text", ama data-val YOK.
    // Farkı yaratan şey nullable reference types; bir attribute değil.
    public string? MetinNullable { get; set; }

    public int Sayi { get; set; }            // → type="number"
    public int? SayiNullable { get; set; }   // → type="number", data-val yok
    public decimal Fiyat { get; set; }       // → type="text" (!) — sebebi kültür
    public DateTime Tarih { get; set; }      // → type="datetime-local"
    public bool Onayli { get; set; }         // → checkbox + GİZLİ "false" alanı

    [Display(Name = "Gösterilecek Ad")]
    public string Gosterim { get; set; } = "";       // label metnini belirler

    [StringLength(50, MinimumLength = 3)]
    public string Sinirli { get; set; } = "";        // → maxlength="50"

    [DataType(DataType.Password)]
    public string Parola { get; set; } = "";         // → type="password", value YOK

    [DataType(DataType.EmailAddress)]
    public string Eposta { get; set; } = "";         // → type="email"

    [DataType(DataType.MultilineText)]
    public string Aciklama { get; set; } = "";       // <textarea asp-for> ile

    public KategoriTuru Kategori { get; set; }       // <select asp-items> ile

    public AdresVM Adres { get; set; } = new();      // → name="Adres.Sehir"

    // Liste tipi → <select> kendiliğinden multiple="multiple" olur
    public List<int> SecilenKategoriler { get; set; } = [1, 3];

    // optgroup gösterisi için
    public int DersId { get; set; } = 2;
}

/// <summary>
/// HAFTA 5 — /Etiket/Sayfa için: &lt;partial for="OneCikan"&gt; bir model
/// özelliği ister (Hafta 5 ders notu bölüm 1.6).
/// </summary>
public class SayfaVM
{
    public KampusKitap.Models.Kitap OneCikan { get; set; } = new();
}

/// <summary>İç içe nesnenin <c>name</c>/<c>id</c> üretimini göstermek için.</summary>
public class AdresVM
{
    public string Sehir { get; set; } = "";
}

public enum KategoriTuru { Edebiyat, Bilgisayar, BilimKurgu }
