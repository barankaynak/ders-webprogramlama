using Microsoft.AspNetCore.Mvc;
using KampusKitap.ViewModels;

namespace KampusKitap.Controllers;

/// <summary>
/// HAFTA 5 — Tag Helper'lar.
///
/// Bu controller tek bir soruyu cevaplıyor: <c>asp-*</c> yazdığımızda
/// tarayıcıya giden HTML tam olarak NE oluyor?
///
/// Sayfaların hepsi <strong>Ctrl+U</strong> ile okunmak üzere kurulmuştur;
/// anlatımın merkezi üretilen kaynaktır.
/// </summary>
public class EtiketController : Controller
{
    // GET /Etiket
    // asp-for galerisi: her tip ve her attribute için üretilen HTML
    public IActionResult Index()
    {
        ViewData["Title"] = "asp-for ne üretiyor?";
        return View(new EtiketVM { Fiyat = 120.50m, Sayi = 42 });
    }

    // GET /Etiket/Baglanti
    // URL üreten Tag Helper'lar + 🕳️ elle yazılan href tuzağı
    public IActionResult Baglanti()
    {
        ViewData["Title"] = "URL üreten Tag Helper'lar";
        return View();
    }

    // GET /Etiket/Csrf
    // Token nereden geliyor, neden GET'te yok
    public IActionResult Csrf()
    {
        ViewData["Title"] = "Anti-forgery token";
        return View();
    }

    // POST /Etiket/Csrf — KORUMALI
    //
    // Token yoksa ya da cookie ile eşleşmiyorsa action HİÇ ÇALIŞMAZ:
    // çatı isteği 400 ile reddeder. Bu bir exception değil, bir filtre sonucudur —
    // hata sayfası bile görmezsiniz (Hafta 5 ders notu bölüm 4.3).
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Csrf(string? mesaj)
    {
        ViewData["Title"] = "Anti-forgery token";
        ViewData["Sonuc"] = $"✓ Token doğrulandı, action çalıştı. Gelen mesaj: {mesaj ?? "(yok)"}";
        return View();
    }

    // GET /Etiket/Eksik
    // 🐛 _ViewImports'ta @addTagHelper olmadığında ne oluyor?
    // Görünüm @removeTagHelper ile bunu BİLEREK simüle ediyor.
    public IActionResult Eksik()
    {
        ViewData["Title"] = "addTagHelper olmayınca";
        return View(new EtiketVM());
    }
}
