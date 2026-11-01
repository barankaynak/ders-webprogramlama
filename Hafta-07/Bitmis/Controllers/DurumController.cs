using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace KampusKitap.Controllers;

/// <summary>
/// HAFTA 7 — State Management ölçüm sayfaları.
///
/// Hafta 6'daki /Dogrula ModelState'in içini gösteriyordu; bu controller durumun
/// NEREDE tutulduğunu gösteriyor: tarayıcıda (cookie) mı, sunucuda (session) mı?
///
///   /Durum              sunucunun gördüğü cookie'ler ve session
///   /Durum/Cerez        cookie çeşitleri: geçici, kalıcı, HttpOnly, 5000 bayt
///   /Durum/AyniIstek    yazılan cookie aynı istekte okunabilir mi?
///   /Durum/Panel        🕳️ Klasik Tuzak: rol cookie'de (TODO 7.8)
///   /Durum/SessionSayaci  session sayacı: zaman aşımı, yeniden başlatma
/// </summary>
public class DurumController : Controller
{
    // Demo cookie'lerinin adları — Temizle hepsini siler.
    private static readonly string[] DenemeCookieleri =
        ["Deneme-Gecici", "Deneme-Kalici", "Deneme-HttpOnly", "Deneme-Buyuk", "Deneme-Ayni"];

    // GET /Durum
    public IActionResult Index()
    {
        ViewData["Title"] = "Durum nerede?";
        return View();
    }

    // GET /Durum/Cerez
    [HttpGet]
    public IActionResult Cerez()
    {
        ViewData["Title"] = "Cookie çeşitleri";
        return View();
    }

    // POST /Durum/Cerez — seçilen cookie'yi yaz, sonra PRG ile aynı sayfaya dön.
    // Yönlendirmeden sonraki GET isteği cookie'yi geri getirir; liste orada görünür.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cerez(string tur)
    {
        switch (tur)
        {
            case "gecici":      // Expires yok → tarayıcı kapanınca silinir
                Response.Cookies.Append("Deneme-Gecici", "1");
                break;
            case "kalici":      // Expires var → tarayıcı kapansa da 7 gün durur
                Response.Cookies.Append("Deneme-Kalici", "1",
                    new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) });
                break;
            case "httponly":    // JavaScript (document.cookie) göremez
                Response.Cookies.Append("Deneme-HttpOnly", "1",
                    new CookieOptions { HttpOnly = true });
                break;
            case "buyuk":       // 5000 bayt: tarayıcının sınırı ~4096 → SAKLANMAZ (ölçüldü)
                Response.Cookies.Append("Deneme-Buyuk", new string('x', 5000));
                break;
            case "temizle":
                foreach (var ad in DenemeCookieleri)
                    Response.Cookies.Delete(ad);
                break;
        }
        return RedirectToAction(nameof(Cerez));
    }

    // GET /Durum/AyniIstek
    // Yazılan cookie bu yanıtın Set-Cookie başlığında gider; Request.Cookies ise
    // tarayıcının BU İSTEKLE getirdiklerini gösterir. Okunan, bir önceki yazılandır.
    public IActionResult AyniIstek()
    {
        ViewData["Title"] = "Aynı istekte okumak";
        var yazilan = DateTime.Now.ToString("HH:mm:ss");
        Response.Cookies.Append("Deneme-Ayni", yazilan);
        ViewData["Yazilan"] = yazilan;
        ViewData["Okunan"] = Request.Cookies["Deneme-Ayni"];
        return View();
    }

    // ---------------------------------------------------------------
    // 🕳️ KLASİK TUZAK — kullanıcının rolünü cookie'de düz metin tutmak
    // (Hafta 7 ders notu, Klasik Tuzak). Cookie istemcidedir: kullanıcı
    // F12 → Application → Cookies'ten "ogrenci"yi "yonetici" yapabilir.
    // ---------------------------------------------------------------

    // GET /Durum/Panel
    public IActionResult Panel()
    {
        ViewData["Title"] = "Panel";
        // ✔ TODO 7.8 · Rolü cookie'de değil session'da tutun: girişte session'a yazın, panelde session'dan okuyun
        var rol = HttpContext.Session.GetString("Rol") ?? "misafir";
        return View(model: rol);
    }

    // POST /Durum/Giris — gerçek giriş Hafta 11'de; burada yalnızca "öğrenci olarak giriş yaptı" deriz.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Giris()
    {
        // ✔ TODO 7.8 · Rolü cookie'de değil session'da tutun: girişte session'a yazın, panelde session'dan okuyun
        HttpContext.Session.SetString("Rol", "ogrenci");
        return RedirectToAction(nameof(Panel));
    }

    // POST /Durum/Cikis
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cikis()
    {
        // ✔ TODO 7.8 · Rolü cookie'de değil session'da tutun: girişte session'a yazın, panelde session'dan okuyun
        HttpContext.Session.Remove("Rol");
        return RedirectToAction(nameof(Panel));
    }

    // ---------------------------------------------------------------
    // Session sayacı — TODO 7.3 ve 7.4 yapılmadan 500 döner (Hata Köşesi).
    // ---------------------------------------------------------------

    // GET /Durum/SessionSayaci
    public IActionResult SessionSayaci()
    {
        ViewData["Title"] = "Session sayacı";
        var sayac = (HttpContext.Session.GetInt32("Sayac") ?? 0) + 1;
        HttpContext.Session.SetInt32("Sayac", sayac);
        return View(sayac);
    }

    // POST /Durum/SessionTemizle
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SessionTemizle()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(SessionSayaci));
    }

    // Session yapılandırılmamışsa (TODO 7.3/7.4 öncesi) Index sayfası 500 vermesin diye:
    // UseSession yoksa ISessionFeature da yoktur.
    internal static ISession? SessionVarsa(HttpContext ctx) =>
        ctx.Features.Get<ISessionFeature>()?.Session;
}
