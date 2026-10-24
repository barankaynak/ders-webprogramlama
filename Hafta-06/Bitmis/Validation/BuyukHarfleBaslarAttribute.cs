using System.ComponentModel.DataAnnotations;

namespace KampusKitap.Validation;

/// <summary>
/// HAFTA 6 — projenin KENDİ doğrulama attribute'ü (Hafta 6 ders notu bölüm 5.2).
///
/// Kullanımı:  [BuyukHarfleBaslar] public string Baslik { get; set; }
///
/// Hazır attribute'ler ([Required], [Range] …) gibi ValidationAttribute'ten
/// türer; çatı onu da aynı anda, aynı yerde çalıştırır.
///
/// DİKKAT: bu attribute YALNIZCA SUNUCUDA çalışır. Tarayıcıya data-val-*
/// attribute'ü yazılmaz (IClientModelValidator uygulanmadığı için) — istemci
/// doğrulaması onu hiç görmez. /Kitap/EkleTH'de Ctrl+U ile bakın.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class BuyukHarfleBaslarAttribute : ValidationAttribute
{
    // Varsayılan mesaj; {0} alanın görünen adıyla dolar.
    public BuyukHarfleBaslarAttribute() : base("{0} büyük harfle başlamalıdır.") { }

    public override bool IsValid(object? value)
    {
        // ✔ TODO 6.6 · `IsValid` gövdesini yazın: metin büyük harfle başlamıyorsa `false`
        if (value is not string metin || metin.Length == 0)
            return true;                  // boşluğu [Required] yakalar
        return char.IsUpper(metin[0]);
    }
}
