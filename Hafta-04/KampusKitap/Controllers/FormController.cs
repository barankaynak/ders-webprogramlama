using Microsoft.AspNetCore.Mvc;
using KampusKitap.Models;

namespace KampusKitap.Controllers;

/// <summary>
/// HAFTA 4 — Web Formlar.
/// Bu controller tek bir soruyu cevaplıyor: kullanıcının doldurduğu form
/// sunucuya tam olarak NE gönderiyor ve o veri C# nesnesine NASIL dönüşüyor?
/// </summary>
public class FormController : Controller
{
    // ---------------------------------------------------------------
    // 1 · HAM GÖRÜNÜM — isteğin gövdesini olduğu gibi göster
    // ---------------------------------------------------------------

    // GET /Form  → formu gösterir
    public IActionResult Index()
    {
        ViewData["Title"] = "Formun gönderdiği şey";
        return View();
    }

    // POST /Form
    // Model binding KULLANMIYORUZ: isteğin gövdesini ham okuyup ekrana basıyoruz.
    // Amaç, "form ne gönderdi?" sorusunu tahminle değil kanıtla cevaplamak.
    [HttpPost]
    public async Task<IActionResult> Index(IFormCollection form)
    {
        Request.Body.Position = 0;
        using var okuyucu = new StreamReader(Request.Body);
        ViewData["Govde"] = await okuyucu.ReadToEndAsync();
        ViewData["Tip"] = Request.ContentType ?? "(yok)";
        ViewData["Metot"] = Request.Method;
        ViewData["Alanlar"] = form.Select(a => a.Key + " = " + a.Value).ToList();
        ViewData["Title"] = "Formun gönderdiği şey";
        return View("Index");
    }

    // ---------------------------------------------------------------
    // 2 · BAĞLAMA KAYNAKLARI — aynı ad üç yerden gelirse hangisi kazanır?
    // ---------------------------------------------------------------

    // GET  /Form/Kaynak/5?deger=sorgu        → rota ve sorgu yarışır
    // POST /Form/Kaynak/5?deger=sorgu  (gövde: deger=form)  → üçü birden yarışır
    //
    // Sonucu TAHMİN ETMEYİN; sayfa üçünü de ayrı ayrı gösteriyor.
    [HttpGet]
    [Route("Form/Kaynak/{deger?}")]
    public IActionResult Kaynak(string? deger)
        => KaynakGoster(deger);

    [HttpPost]
    [Route("Form/Kaynak/{deger?}")]
    public IActionResult KaynakPost(string? deger)
        => KaynakGoster(deger);

    // Aynı istek, tek fark [FromQuery]: öncelik sırası devre dışı kalır,
    // değer YALNIZCA sorgu dizesinden okunur.
    [HttpPost]
    [Route("Form/KaynakZorla/{deger?}")]
    public IActionResult KaynakZorla([FromQuery] string? deger)
        => KaynakGoster(deger);

    private IActionResult KaynakGoster(string? deger)
    {
        ViewData["Title"] = "Bağlama kaynakları";
        ViewData["Kazanan"] = deger ?? "(null)";
        ViewData["Rota"] = RouteData.Values.TryGetValue("deger", out var r) ? r?.ToString() : "(yok)";
        ViewData["Sorgu"] = Request.Query.ContainsKey("deger") ? Request.Query["deger"].ToString() : "(yok)";
        ViewData["Form"] = Request.HasFormContentType && Request.Form.ContainsKey("deger")
            ? Request.Form["deger"].ToString() : "(yok)";
        return View("Kaynak");
    }

    // ---------------------------------------------------------------
    // 3 · BAĞLAMA BAŞARISIZ OLURSA — sessizce varsayılan değer
    // ---------------------------------------------------------------

    // GET /Form/Bagla?sayi=abc&tarih=xyz
    public IActionResult Bagla(int sayi, int? sayiNullable, DateTime tarih)
    {
        ViewData["Title"] = "Bağlama başarısız olduğunda";
        ViewData["sayi"] = sayi;                       // int     → 0
        ViewData["sayiNullable"] = sayiNullable?.ToString() ?? "null";
        ViewData["tarih"] = tarih.ToString("O");       // DateTime → 0001-01-01
        ViewData["Gecerli"] = ModelState.IsValid;
        ViewData["Hatalar"] = ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .Select(x => x.Key + ": " + string.Join(" | ", x.Value!.Errors.Select(e => e.ErrorMessage)))
            .ToList();
        return View();
    }

    // ---------------------------------------------------------------
    // 4 · OVER-POSTING — kullanıcı formda olmayan bir alanı gönderirse
    // ---------------------------------------------------------------

    // Entity'yi doğrudan parametre yapmak: 🕳️ bu haftanın Klasik Tuzağı.
    [HttpPost]
    public IActionResult Tuzak(Kitap kitap)
    {
        ViewData["Title"] = "Over-posting";
        ViewData["Gelen"] = kitap;
        return View();
    }
}
