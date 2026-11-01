using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KampusKitap.TagHelpers;

/// <summary>
/// HAFTA 5 — Learn'deki BoldTagHelper (Hafta 5 ders notu bölüm 5.3).
///
/// Yeni bir etiket İCAT ETMEZ: "bold" attribute'ü taşıyan HER etikete bağlanır.
///   &lt;p bold&gt;Kalın&lt;/p&gt;  →  &lt;p&gt;&lt;strong&gt;Kalın&lt;/strong&gt;&lt;/p&gt;
///
/// RemoveAll("bold") satırına dikkat: çalışan Tag Helper KENDİ İZİNİ SİLER.
/// Makine slaytındaki üçüncü istasyonun ta kendisi.
/// </summary>
[HtmlTargetElement(Attributes = "bold")]
public class BoldTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll("bold");
        output.PreContent.SetHtmlContent("<strong>");
        output.PostContent.SetHtmlContent("</strong>");
    }
}
