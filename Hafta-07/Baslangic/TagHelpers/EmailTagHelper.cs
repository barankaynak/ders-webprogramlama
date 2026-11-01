using Microsoft.AspNetCore.Razor.TagHelpers;

namespace KampusKitap.TagHelpers;

/// <summary>
/// HAFTA 5 — Microsoft Learn "Author Tag Helpers" sayfasındaki EmailTagHelper
/// (Hafta 5 ders notu bölüm 5.1).
///
///   &lt;email mail-to="Support"&gt;&lt;/email&gt;
///   → &lt;a href="mailto:Support@contoso.com"&gt;Support@contoso.com&lt;/a&gt;
///
/// Üç kural tek sınıfta:
///   · sınıf adı "EmailTagHelper" → hedef etiket &lt;email&gt;
///   · özellik "MailTo" (PascalCase) → attribute "mail-to" (kebab-case)
///   · Process, etiketi istediğimiz HTML'e dönüştürür
/// </summary>
public class EmailTagHelper : TagHelper
{
    private const string EmailDomain = "contoso.com";

    // <email mail-to="..." /> ile geçirilir. PascalCase → kebab-case.
    public string MailTo { get; set; } = "";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";                       // <email> → <a>

        var address = MailTo + "@" + EmailDomain;
        output.Attributes.SetAttribute("href", "mailto:" + address);
        output.Content.SetContent(address);
    }
}
