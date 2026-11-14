using KampusKitap.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KampusKitap.Controllers;

/// <summary>
/// HAFTA 9 — çoka-çok ilişki: bir derste birçok kitap, bir kitap birçok derste
/// (Hafta 9 ders notu bölüm 5.3). Ara tablo (DersKitap) için sınıf da controller da yok;
/// iki taraftaki liste yeterli.
/// </summary>
public class DersController : Controller
{
    private readonly KampusKitapContext _db;

    public DersController(KampusKitapContext db)
    {
        _db = db;
    }

    // GET /Ders
    public IActionResult Index()
    {
        ViewData["Title"] = "Dersler";
        ViewData["Kitaplar"] = new SelectList(_db.Kitaplar.ToList(), "Id", "Baslik");

        // TODO 9.9 · Çoka-çok ilişkiyi kurun: Kitap'a Dersler, Ders'e Kitaplar; sonra Add-Migration DersKitap ve Update-Database
        var dersler = _db.Dersler.ToList();

        return View(dersler);
    }

    // POST /Ders/KitapEkle/2   (formdan: kitapId=3)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult KitapEkle(int id, int kitapId)
    {
        // Find, Include ile birleşmez. Aynı işi "Id'si şu olan ilk ders" sorgusu yapar:
        // FirstOrDefault(d => d.Id == id). Bu yazımın ayrıntısı Hafta 10'un konusu (LINQ).
        //
        // Include ŞART: ders.Kitaplar yüklenmezse EF var olan bağları bilmez; zaten ekli
        // bir kitabı yeniden eklemek "UNIQUE constraint failed: DersKitap…" ile düşer (ölçüldü).
        // TODO 9.10 · Kitabı derse ekleyin: dersi kitaplarıyla getirin, kitabı listeye ekleyin, SaveChanges
        //      ipucu: Hafta 9 ders notu bölüm 5.4 — `ders.Kitaplar.Add(kitap)`; ara tabloya kimse elle yazmaz, konsolda INSERT INTO "DersKitap" satırını görün

        return RedirectToAction(nameof(Index));
    }
}
