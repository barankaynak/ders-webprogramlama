using Microsoft.AspNetCore.Mvc;

namespace KampusKitap.Controllers;

public class KategoriController : Controller
{
    // Bellek içi liste — Hafta 8'de bunun yerini veritabanı alacak
    private static readonly string[] Kategoriler =
        ["Roman", "Ders Kitabı", "Bilim Kurgu", "Tarih", "Sınav Hazırlık"];

    // GET /Kategori
    //
    // Bu görünüm @section Scripts TANIMLAMAZ. Layout'taki
    // RenderSectionAsync("Scripts", required: false) sayesinde sorun çıkmaz.
    // required: true yaparsanız BU SAYFA hata verir — denemeye değer.
    public IActionResult Index()
    {
        ViewData["Title"] = "Kategoriler";
        return View(Kategoriler.ToList());
    }

    // GET /Kategori/Detay/2
    public IActionResult Detay(int? id)
    {
        if (id is null || id < 0 || id >= Kategoriler.Length)
            return NotFound();                       // 404 — Hafta 2 bölüm 2.2

        ViewData["Title"] = Kategoriler[id.Value];
        return View("Detay", Kategoriler[id.Value]);
    }

    // GET /Kategori/Ara
    // Bu action bilinçli olarak RedirectToAction gösteriyor (Hafta 2 bölüm 2.5)
    public IActionResult Ara()
    {
        return RedirectToAction("Index");            // 302 → /Kategori
    }
}
