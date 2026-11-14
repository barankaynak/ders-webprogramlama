using KampusKitap.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KampusKitap.Controllers;

public class KategoriController : Controller
{
    // HAFTA 9 — Hafta 3'ten beri burada beş adlık bir dizi vardı: "Roman", "Ders Kitabı"…
    // Kitaplar ise KitapDeposu'ndaki başka üç kategoriyi kullanıyordu: "Edebiyat", "Bilgisayar"…
    // İki liste, iki ayrı "doğru"; hiçbiri ötekini bilmiyordu. Artık tek kaynak var:
    // Kategoriler tablosu (Hafta 9'un ilkesi — ders notu bölüm 1).
    private readonly KampusKitapContext _db;

    public KategoriController(KampusKitapContext db)
    {
        _db = db;
    }

    // GET /Kategori
    //
    // Bu view @section Scripts TANIMLAMAZ. Layout'taki
    // RenderSectionAsync("Scripts", required: false) sayesinde sorun çıkmaz.
    // required: true yaparsanız BU SAYFA hata verir — denemeye değer.
    public IActionResult Index()
    {
        ViewData["Title"] = "Kategoriler";

        // HAFTA 9 — Include: her kategorinin kitaplarını AYNI sorguda getirir (LEFT JOIN).
        // Include yazılmazsa navigation boş kalır ve HATA ALMAZSINIZ: her kategoride
        // "0 kitap" görünür (ölçüldü). Hafta 3'ün N+1 tuzağının ilacı da budur.
        // TODO 9.8 · Kategorileri kitaplarıyla birlikte getirin: Include
        //      ipucu: Hafta 9 ders notu bölüm 5.2 — `_db.Kategoriler.Include(k => k.Kitaplar).ToList()`. Önce Include'suz çalıştırın: her kategoride 0 kitap görünür, hata yok. Sonra konsolda LEFT JOIN'i görün
        var kategoriler = _db.Kategoriler.ToList();   // kitaplar yüklenmiyor

        return View(kategoriler);
    }

    // GET /Kategori/Detay/2
    public IActionResult Detay(int? id)
    {
        if (id is null)
            return NotFound();

        var kategori = _db.Kategoriler.Find(id.Value);
        if (kategori is null)
            return NotFound();                       // 404 — Hafta 2 bölüm 2.2

        ViewData["Title"] = kategori.Ad;
        return View("Detay", kategori);
    }

    // GET /Kategori/Ara
    // Bu action bilinçli olarak RedirectToAction gösteriyor (Hafta 2 bölüm 2.5)
    public IActionResult Ara()
    {
        return RedirectToAction("Index");            // 302 → /Kategori
    }
}
