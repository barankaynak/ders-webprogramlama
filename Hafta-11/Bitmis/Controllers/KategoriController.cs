using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KampusKitap.Data;
using KampusKitap.Models;
using KampusKitap.ViewModels;

namespace KampusKitap.Controllers;

public class KategoriController : Controller
{
    private readonly KampusKitapContext _db;

    public KategoriController(KampusKitapContext db)
    {
        _db = db;
    }

    // HAFTA 11 — kategoriler artık Kategoriler TABLOSUNDA (Hafta 11 ders notu bölüm 1.1).
    // Hafta 9'da bu controller'ın beş adlık ayrı listesi kaldırılmıştı; Hafta 10'da kaynak
    // KitapDeposu.Kategoriler'di. Artık sayfa da formlar da aynı tablodan okur.

    // GET /Kategori
    // Bu view @section Scripts TANIMLAMAZ; layout'ta required: false (Hafta 3).
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Kategoriler";

        // HAFTA 11 — ilişki üzerinden sayım VERİTABANINDA (Hafta 11 ders notu bölüm 2.5).
        // k.Kitaplar.Count bir navigation; EF Core onu alt sorguya çevirir:
        //   SELECT … (SELECT COUNT(*) FROM "Kitaplar" AS "k0" WHERE "k"."Id" = "k0"."KategoriId") …
        // Kitaplar belleğe gelmez; yalnızca üç satır gelir: Id, Ad, Adet.
        // ✔ TODO 11.6 · Her kategorinin kitap sayısını veritabanında hesaplayın: navigation üzerinden Count
        var satirlar = await _db.Kategoriler
            .OrderBy(k => k.Id)
            .Select(k => new KategoriSatiriVM
            {
                Id = k.Id,
                Ad = k.Ad,
                Adet = k.Kitaplar.Count
            })
            .ToListAsync();

        return View(satirlar);
    }

    // GET /Kategori/Detay/2
    public async Task<IActionResult> Detay(int? id)
    {
        if (id is null)
            return NotFound();

        // FindAsync: primary key ile tek satır; yoksa null → 404 (Hafta 10).
        var kategori = await _db.Kategoriler.FindAsync(id.Value);
        if (kategori is null)
            return NotFound();

        ViewData["Title"] = kategori.Ad;
        ViewData["KategoriAdi"] = kategori.Ad;

        // HAFTA 11 — süzme ve sıralama veritabanında: WHERE "k"."KategoriId" = @id ORDER BY …
        var kitaplar = await _db.Kitaplar
            .Where(k => k.KategoriId == id)
            .OrderBy(k => k.Baslik)
            .ToListAsync();

        return View("Detay", kitaplar);
    }

    // GET /Kategori/Ara → RedirectToAction (Hafta 2 bölüm 2.5)
    public IActionResult Ara()
    {
        return RedirectToAction("Index");
    }
}
