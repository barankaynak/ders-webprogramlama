using Microsoft.AspNetCore.Mvc;
using KampusKitap.ViewModels;

namespace KampusKitap.Controllers;

/// <summary>
/// HAFTA 6 — Model Doğrulama.
///
/// Bu controller tek bir soruyu cevaplıyor: action'ın ilk satırı çalıştığında
/// ModelState'in İÇİNDE ne var? Her sayfa ModelState'i tablo hâlinde basar
/// (Views/Shared/_ModelStateTablosu.cshtml).
///
/// Sayfaların hepsi İKİ projede de eksiksizdir; alıştırma değil, ölçüm aracıdır.
/// </summary>
public class DogrulaController : Controller
{
    // GET /Dogrula
    //   ?istemci=true            istemci doğrulaması açık (Türkçe sayı düzeltmesiyle)
    //   ?istemci=true&ham=true   açık ama düzeltme YOK — "1,75" reddedilir (bölüm 4.4)
    [HttpGet]
    public IActionResult Index()
    {
        ViewData["Title"] = "Doğrulama attribute'leri";
        ViewData["Istemci"] = Bayrak("istemci");
        ViewData["Ham"] = Bayrak("ham");
        return View(new UyeVM());
    }

    // POST /Dogrula
    //
    // Buraya gelindiğinde çatı İKİ işi bitirmiştir: Model Binding ve doğrulama.
    // Sonuç ModelState'te; action yalnızca okur ve iş kuralı ekler.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(UyeVM girdi)
    {
        bool istemci = Bayrak("istemci"), ham = Bayrak("ham");
        ViewData["Title"] = "Doğrulama attribute'leri";
        ViewData["Istemci"] = istemci;
        ViewData["Ham"] = ham;

        // İş kuralı — alana bağlı: hata E-posta'nın altına düşer
        if (string.Equals(girdi.Eposta, "ali@ornek.com", StringComparison.OrdinalIgnoreCase))
            ModelState.AddModelError(nameof(UyeVM.Eposta), "Bu e-posta ile kayıtlı bir üye var.");

        // İş kuralı — modelin tamamına: anahtar boş, hata özetinde görünür
        if (string.Equals(girdi.Ad, "admin", StringComparison.OrdinalIgnoreCase))
            ModelState.AddModelError(string.Empty, "Bu ad sistem tarafından ayrılmıştır.");

        if (!ModelState.IsValid)
            return View(girdi);                         // 200 — aynı form, hatalarla

        TempData["Mesaj"] = $"{girdi.Ad} kaydedildi (gösteri: hiçbir yere yazılmadı).";
        return RedirectToAction(nameof(Index), new { istemci, ham });   // 302 — PRG
    }

    // Sayfa ayarı (istemci, ham) adresten OKUNUR, parametre olarak BAĞLANMAZ:
    // bağlansaydı ModelState tablosunda modelin alanları arasında görünürdü.
    private bool Bayrak(string ad) =>
        string.Equals(Request.Query[ad], "true", StringComparison.OrdinalIgnoreCase);

    // GET/POST /Dogrula/Ortuk — attribute'süz model: hatalar nereden geliyor?
    [HttpGet]
    public IActionResult Ortuk()
    {
        ViewData["Title"] = "Örtük doğrulama";
        return View(new OrtukVM());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Ortuk(OrtukVM girdi)
    {
        ViewData["Title"] = "Örtük doğrulama";
        ViewData["Gonderildi"] = true;
        return View(girdi);
    }

    // GET/POST /Dogrula/Tuzak — 🐛 IsValid hep false
    [HttpGet]
    public IActionResult Tuzak()
    {
        ViewData["Title"] = "IsValid neden false?";
        return View(new TuzakVM());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Tuzak(TuzakVM girdi)
    {
        ViewData["Title"] = "IsValid neden false?";
        ViewData["Gonderildi"] = true;
        return View(girdi);
    }

    // GET/POST /Dogrula/Sira — IValidatableObject ne zaman çalışır?
    [HttpGet]
    public IActionResult Sira()
    {
        ViewData["Title"] = "Doğrulamanın sırası";
        return View(new SiraVM());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Sira(SiraVM girdi)
    {
        ViewData["Title"] = "Doğrulamanın sırası";
        ViewData["Gonderildi"] = true;
        return View(girdi);
    }
}
