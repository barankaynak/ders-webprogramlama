using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KampusKitap.TagHelpers;

/// <summary>
/// HAFTA 5 — Learn'deki ConditionTagHelper (Hafta 5 ders notu bölüm 5.3).
///
///   &lt;div condition="@Model.Onayli"&gt;…&lt;/div&gt;
///
/// Koşul false ise etiket İÇERİĞİYLE BİRLİKTE hiç üretilmez. View'da
/// @if yazmanın attribute'le yapılmış hâli.
/// </summary>
[HtmlTargetElement(Attributes = nameof(Condition))]
public class ConditionTagHelper : TagHelper
{
    public bool Condition { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!Condition)
        {
            output.SuppressOutput();
        }
    }
}
