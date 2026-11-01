using Microsoft.AspNetCore.Mvc;
using KampusKitap.Data;
using KampusKitap.Models;
using KampusKitap.Services;

namespace KampusKitap.Controllers;

/// <summary>
/// HAFTA 7 — Session ile sepet (Hafta 7 ders notu bölüm 3).
///
/// Sepet SUNUCUDA durur. Tarayıcıda yalnızca .AspNetCore.Session cookie'si olur;
/// içinde sepet değil, sepeti bulmaya yarayan anahtar vardır. F12 → Application →
/// Cookies'te bakın: kitap adı ya da fiyat göremezsiniz.
///
/// TODO 7.3 ve 7.4 yapılmadan bu sayfalar 500 döner:
///   InvalidOperationException: Session has not been configured for this application or request.
/// </summary>
public class SepetController : Controller
{
    private const string Anahtar = "Sepet";

    // GET /Sepet
    public IActionResult Index()
    {
        ViewData["Title"] = "Sepet";
        var sepet = HttpContext.Session.NesneOku<List<SepetSatiri>>(Anahtar) ?? [];
        return View(sepet);
    }

    // POST /Sepet/Ekle/3  — kitap detay sayfasındaki "Sepete ekle" düğmesi
    // Veriyi değiştirdiği için POST (Hafta 4); token doğrulanır (Hafta 5); sonunda PRG.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Ekle(int id)
    {
        var kitap = KitapDeposu.Bul(id);
        if (kitap is null)
            return NotFound();

        // ✔ TODO 7.6 · Kitabı sepete ekleyin: sepeti session'dan okuyun, satırı ekleyin ya da adedi artırın, sepeti geri yazın
        var sepet = HttpContext.Session.NesneOku<List<SepetSatiri>>(Anahtar) ?? [];
        var satir = sepet.FirstOrDefault(s => s.KitapId == id);
        if (satir is null)
            sepet.Add(new SepetSatiri
            {
                KitapId = kitap.Id, Baslik = kitap.Baslik, Fiyat = kitap.Fiyat, Adet = 1
            });
        else
            satir.Adet++;
        HttpContext.Session.NesneYaz(Anahtar, sepet);

        TempData["Mesaj"] = $"\"{kitap.Baslik}\" sepete eklendi.";
        return RedirectToAction(nameof(Index));
    }

    // POST /Sepet/Cikar/3
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cikar(int id)
    {
        var sepet = HttpContext.Session.NesneOku<List<SepetSatiri>>(Anahtar) ?? [];
        sepet.RemoveAll(s => s.KitapId == id);
        HttpContext.Session.NesneYaz(Anahtar, sepet);
        return RedirectToAction(nameof(Index));
    }

    // POST /Sepet/Bosalt
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Bosalt()
    {
        HttpContext.Session.Remove(Anahtar);
        return RedirectToAction(nameof(Index));
    }
}
