using Microsoft.AspNetCore.Mvc;

namespace KampusKitap.Controllers;

public class KategoriController : Controller
{
    // Bellek içi liste — Hafta 8'de bunun yerini veritabanı alacak
    private static readonly string[] Kategoriler =
        ["Roman", "Ders Kitabı", "Bilim Kurgu", "Tarih", "Sınav Hazırlık"];

    // GET /Kategori
    public IActionResult Index()
    {
        return Content("Kategoriler: " + string.Join(", ", Kategoriler));
    }

    // GET /Kategori/Detay/2
    public IActionResult Detay(int? id)
    {
        if (id is null || id < 0 || id >= Kategoriler.Length)
            return NotFound();                       // 404 — ders notu §2.2

        return Content($"Kategori: {Kategoriler[id.Value]}");
    }

    // GET /Kategori/Ara
    // Bu action bilinçli olarak RedirectToAction gösteriyor (ders notu §2.5)
    public IActionResult Ara()
    {
        return RedirectToAction("Index");            // 302 → /Kategori
    }
}
