using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KampusKitap.TagHelpers;

/// <summary>
/// HAFTA 5 — projenin KENDİ Tag Helper'ı (Hafta 5 ders notu bölüm 5.2).
///
/// Kullanımı:  &lt;fiyat deger="120.5" /&gt;
/// Ürettiği:   &lt;span class="fiyat"&gt;120,50 ₺&lt;/span&gt;
///
/// Sınıf adının sonundaki "TagHelper" eki, etiket adını belirler:
/// FiyatTagHelper → &lt;fiyat&gt;. Attribute adları da aynı şekilde
/// özellik adlarından türetilir (PascalCase → kebab-case).
///
/// Bu sınıfın görünümlerde tanınması için _ViewImports.cshtml'deki
/// "@addTagHelper *, KampusKitap" satırı gerekir (ders notu bölüm 1.3).
///
/// TagMode satırı neden var? Görünümde kendini kapatan yazım (&lt;fiyat … /&gt;)
/// kullanılıyor. TagMode verilmezse çıktı da kendini kapatır ve içerik HİÇ
/// yazılmaz: &lt;span class="fiyat" /&gt;. /Etiket/Ozel'deki &lt;email … /&gt;
/// satırı bu durumun canlı örneği.
/// </summary>
public class FiyatTagHelper : TagHelper
{
    /// <summary>&lt;fiyat deger="…" /&gt;</summary>
    public decimal Deger { get; set; }

    /// <summary>&lt;fiyat para-birimi="$" /&gt; — iki kelimeli ad kebab-case olur.</summary>
    public string ParaBirimi { get; set; } = "₺";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";                       // <fiyat> → <span>
        output.TagMode = TagMode.StartTagAndEndTag;    // kendi kendini kapatmasın
        output.Attributes.SetAttribute("class", "fiyat");
        output.Content.SetContent($"{Deger:N2} {ParaBirimi}");
    }
}
