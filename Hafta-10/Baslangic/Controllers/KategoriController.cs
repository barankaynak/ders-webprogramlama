using Microsoft.AspNetCore.Mvc;
using KampusKitap.Data;
using KampusKitap.Models;
using KampusKitap.ViewModels;

namespace KampusKitap.Controllers;

public class KategoriController : Controller
{
    // HAFTA 10 — veri veritabanından; context'i DI verir (Hafta 10 ders notu bölüm 4.4).
    private readonly KampusKitapContext _db;

    public KategoriController(KampusKitapContext db)
    {
        _db = db;
    }

    // HAFTA 9 — bu controller'ın kendi beş adlık listesi ("Roman", "Ders Kitabı", …) kaldırıldı.
    // Kitaplar 1–3 numaralı kategorilere bağlıydı, bu sayfa ise başka beş ad gösteriyordu:
    // aynı bilginin iki kaynağı vardı ve birbirini tutmuyordu. Artık tek kaynak
    // KitapDeposu.Kategoriler; sayılar da kitaplardan LINQ ile çıkıyor (Hafta 10: kitaplar veritabanında).

    // GET /Kategori
    //
    // Bu view @section Scripts TANIMLAMAZ. Layout'taki
    // RenderSectionAsync("Scripts", required: false) sayesinde sorun çıkmaz.
    // required: true yaparsanız BU SAYFA hata verir — denemeye değer.
    public IActionResult Index()
    {
        ViewData["Title"] = "Kategoriler";

        // Kategoriler bir Dictionary<int, string>; LINQ onu da sorgular:
        // her eleman bir KeyValuePair (kv.Key = 1, kv.Value = "Edebiyat").
        // HAFTA 10 — kitaplar veritabanından; kategoriler hâlâ sözlükte (Hafta 11'de tablo).
        var kitaplar = _db.Kitaplar.ToList();
        var satirlar = KitapDeposu.Kategoriler
            .Select(kv => new KategoriSatiriVM
            {
                Id = kv.Key,
                Ad = kv.Value,
                Adet = kitaplar.Count(k => k.KategoriId == kv.Key)
            })
            .ToList();

        return View(satirlar);
    }

    // GET /Kategori/Detay/2
    public IActionResult Detay(int? id)
    {
        // TryGetValue: anahtar sözlükte yoksa false döner — 404 (Hafta 2 bölüm 2.2)
        if (id is null || !KitapDeposu.Kategoriler.TryGetValue(id.Value, out var ad))
            return NotFound();

        ViewData["Title"] = ad;
        ViewData["KategoriAdi"] = ad;

        List<Kitap> kitaplar = [];
        kitaplar = _db.Kitaplar.ToList()
            .Where(k => k.KategoriId == id)
            .OrderBy(k => k.Baslik)
            .ToList();

        return View("Detay", kitaplar);
    }

    // GET /Kategori/Ara
    // Bu action bilinçli olarak RedirectToAction gösteriyor (Hafta 2 bölüm 2.5)
    public IActionResult Ara()
    {
        return RedirectToAction("Index");            // 302 → /Kategori
    }
}
