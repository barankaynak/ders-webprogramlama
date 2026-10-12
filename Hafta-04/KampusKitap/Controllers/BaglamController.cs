using Microsoft.AspNetCore.Mvc;

namespace KampusKitap.Controllers;

/// <summary>
/// HAFTA 4 — isteğin tamamına `HttpContext` üzerinden erişmek
/// (Hafta 4 ders notu bölüm 2.6).
///
/// Model Binding sihir değildir: altında duran şey bu nesnedir.
/// Burada aynı form verisine ÜÇ ayrı yoldan ulaşıp hepsinin aynı
/// yere baktığını gösteriyoruz.
/// </summary>
public class BaglamController : Controller
{
    // GET /Baglam → formu gösterir
    public IActionResult Index()
    {
        ViewData["Title"] = "HttpContext ile forma erişmek";
        return View();
    }

    // POST /Baglam
    //
    // Üç erişim yolu, tek kaynak:
    //   1. HttpContext.Request.Form["Baslik"]   → en uzun yazım
    //   2. Request.Form["Baslik"]               → Controller'ın kısayolu
    //   3. IFormCollection form parametresi     → çatının enjekte ettiği hâli
    [HttpPost]
    public IActionResult Index(IFormCollection form)
    {
        ViewData["Title"] = "HttpContext ile forma erişmek";

        // Controller içinde `Request`, `HttpContext.Request`in KISAYOLUDUR.
        // Aşağıdaki iki satır birbirinin aynısıdır.
        ViewData["Uzun"] = HttpContext.Request.Form["Baslik"].ToString();
        ViewData["Kisa"] = Request.Form["Baslik"].ToString();
        ViewData["Parametre"] = form["Baslik"].ToString();

        // Aynı nesne mi? Referans karşılaştırması cevabı veriyor.
        ViewData["AyniMi"] = ReferenceEquals(HttpContext.Request.Form, form);

        // HttpContext yalnızca formu değil, isteğin TAMAMINI taşır.
        ViewData["Metot"] = Request.Method;
        ViewData["Yol"] = Request.Path.Value ?? "";
        ViewData["Tip"] = Request.ContentType ?? "(yok)";
        ViewData["Sorgu"] = Request.Query["kaynak"].ToString();
        ViewData["Tarayici"] = Request.Headers.UserAgent.ToString();
        ViewData["Cerez"] = Request.Cookies.Count;

        // Olmayan alan HATA VERMEZ; boş StringValues döner.
        ViewData["Olmayan"] = Request.Form["Balsik"].ToString();
        ViewData["OlmayanBos"] = Request.Form["Balsik"].Count == 0;

        return View();
    }

    // GET /Baglam/Tehlike
    //
    // 🐛 GET isteğinde form gövdesi YOKTUR. Yine de Request.Form okumak
    // hata fırlatır. Doğru yazım önce HasFormContentType sorar.
    public IActionResult Tehlike(bool korumali = true)
    {
        ViewData["Title"] = "GET isteğinde Request.Form";
        ViewData["Korumali"] = korumali;

        if (korumali)
        {
            // Doğru yazım: önce sor.
            ViewData["Sonuc"] = Request.HasFormContentType
                ? Request.Form["Baslik"].ToString()
                : "(istekte form yok — okumaya kalkışmadık)";
        }
        else
        {
            // Bilerek korumasız: ne olduğunu görmek için.
            ViewData["Sonuc"] = Request.Form["Baslik"].ToString();
        }

        return View();
    }
}
