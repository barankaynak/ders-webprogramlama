using Microsoft.AspNetCore.Mvc;

namespace KampusKitap.Controllers;

/// <summary>
/// HAFTA 4 — Controller'dan görünüme veri taşımanın üç yolu:
/// ViewData, ViewBag ve TempData (ders notu bölüm 1).
///
/// Burada BİLEREK form yok. Amaç yalnızca ÖMÜR farkını göstermek:
/// ViewData/ViewBag tek istek yaşar, TempData yönlendirmeden sağ çıkar.
/// Yönlendirme için Hafta 2'den bilinen RedirectToAction yetiyor.
/// </summary>
public class TasimaController : Controller
{
    // GET /Tasima
    //
    // Üçü de AYNI istekte yazılıp AYNI istekte okunuyor; bu sayfada
    // hiçbir fark görünmez. Fark, yönlendirmeden sonra ortaya çıkar.
    public IActionResult Index()
    {
        ViewData["Title"] = "Veri taşıma";

        ViewData["Mesaj"] = "ViewData ile geldim";
        ViewBag.Sayi = 42;                      // ViewBag = ViewData'nın dynamic yüzü
        TempData["Not"] = "TempData ile geldim";

        // KANIT: ikisi AYNI sözlüğe yazıyor. Birine yazıp diğerinden okuyabiliriz.
        ViewData["Ortak"] = "ViewData ile yazıldı";
        ViewBag.Ortak2 = "ViewBag ile yazıldı";

        return View();
    }

    // GET /Tasima/Yonlendir
    //
    // Üçünü de doldurup YÖNLENDİRİYORUZ. Yönlendirme tarayıcıya
    // "şu adrese yeniden git" demektir: Sonuc YENİ bir istektir.
    public IActionResult Yonlendir()
    {
        ViewData["Mesaj"] = "ViewData ile geldim";
        ViewBag.Sayi = 42;
        TempData["Not"] = "TempData ile geldim";

        return RedirectToAction("Sonuc");       // 302 → /Tasima/Sonuc
    }

    // GET /Tasima/Sonuc
    //
    // Yeni istek: ViewData ve ViewBag BOŞ. TempData hâlâ dolu.
    // Peek okur ama SİLMEZ; köşeli parantezle okumak bu isteğin sonunda siler.
    // Bu yüzden F5'e basınca TempData da kaybolur.
    public IActionResult Sonuc()
    {
        ViewData["Title"] = "Yönlendirmeden sonra";
        ViewData["Kalan"] = TempData.Peek("Not") ?? "(yok)";
        return View();
    }

    // GET /Tasima/Oku
    //
    // Aynı sayfa, ama TempData'yı Peek ile değil köşeli parantezle okuyor.
    // Okunduğu için bu isteğin sonunda silinir; /Tasima/Sonuc'a dönünce yoktur.
    public IActionResult Oku()
    {
        ViewData["Title"] = "TempData'yı okumak";
        ViewData["Okunan"] = TempData["Not"] ?? "(yok)";
        return View();
    }

    // GET /Tasima/Hata
    //
    // 🐛 TempData bir COOKIE'de taşınır; içine ancak serileştirilebilir
    // basit tipler konabilir. Kendi sınıfımızı koymak hata fırlatır.
    public IActionResult Hata()
    {
        TempData["Kitap"] = new Models.Kitap { Id = 1, Baslik = "Deneme" };
        return RedirectToAction("Sonuc");
    }
}
