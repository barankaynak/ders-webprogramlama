using Microsoft.AspNetCore.Mvc;

namespace KampusKitap.Controllers;

/// <summary>
/// HAFTA 5 — Microsoft Learn'deki Anchor Tag Helper örneğinin BİREBİR aynısı
/// (Hafta 5 ders notu bölüm 2.5).
///
/// Adlar belgeyle aynı bırakıldı: Learn sayfasını açıp buradaki çıktıyla
/// karşılaştırabilesiniz diye. Kaynak:
/// https://learn.microsoft.com/aspnet/core/mvc/views/tag-helpers/built-in/anchor-tag-helper?view=aspnetcore-10.0
///
/// Bütün örnekler tek sayfada: /Speaker
/// </summary>
public class SpeakerController : Controller
{
    private readonly List<Speaker> Speakers =
    [
        new Speaker { SpeakerId = 10 },
        new Speaker { SpeakerId = 11 },
        new Speaker { SpeakerId = 12 }
    ];

    // Attribute route: bu action'a conventional route ile ULAŞILMAZ.
    // asp-action="Detail" bu şablonu kullanarak adres üretir.
    [Route("Speaker/{id:int}")]
    public IActionResult Detail(int id)
    {
        ViewData["Title"] = "Speaker " + id;
        return View(Speakers.FirstOrDefault(a => a.SpeakerId == id));
    }

    // ADLI route: <a asp-route="speakerevals"> bu ada göre adres üretir.
    [Route("/Speaker/Evaluations", Name = "speakerevals")]
    public IActionResult Evaluations()
    {
        ViewData["Title"] = "Evaluations";
        return View();
    }

    [Route("/Speaker/EvaluationsCurrent", Name = "speakerevalscurrent")]
    public IActionResult Evaluations(int speakerId, bool currentYear)
    {
        ViewData["Title"] = "Evaluations (current)";
        ViewData["Parametreler"] = $"speakerId = {speakerId} · currentYear = {currentYear}";
        return View();
    }

    // Attribute route'u yok → conventional route: /Speaker
    public IActionResult Index()
    {
        ViewData["Title"] = "Anchor Tag Helper — Microsoft Learn örnekleri";
        return View(Speakers);
    }
}

public class Speaker
{
    public int SpeakerId { get; set; }
}
