using Microsoft.AspNetCore.Mvc;
using KampusKitap.Data;

namespace KampusKitap.Controllers;

public class KitapController : Controller
{
    // HAFTA 3 — Controller artık Content() değil View() döndürüyor.
    //
    // Hafta 2'de buradaki action'lar düz metin döndürüyordu. Değişen tek şey
    // dönüş tipi değil, SORUMLULUK: HTML üretimi artık görünümün işi.
    //
    // DİKKAT (ders notu bölüm 3.5): veri burada, BİR KEZ alınır. Görünüm yalnızca
    // gösterir. Sorguyu .cshtml içine taşırsanız kod çalışır ama N+1 problemi
    // doğar ve hiçbir hata almazsınız.

    // GET /Kitap  ·  GET /Kitap/Index
    // View() dosya adı vermedi → çatı sırayla şunları arar (ders notu bölüm 2.5):
    //   1) /Views/Kitap/Index.cshtml
    //   2) /Views/Shared/Index.cshtml
    public IActionResult Index()
    {
        ViewData["Title"] = "Kitaplar";
        return View(KitapDeposu.Hepsi);
    }

    // GET /Kitap/Detay/42
    public IActionResult Detay(int? id)
    {
        if (id is null)
            return View("Secilmedi");      // Views/Kitap/Secilmedi.cshtml

        var kitap = KitapDeposu.Bul(id.Value);
        if (kitap is null)
            return NotFound();

        ViewData["Title"] = kitap.Baslik;

        // Kategori adını BURADA çözüyoruz, görünümde değil.
        // Görünümden çağırsaydık kod yine çalışırdı — ve bölüm 3.5'teki tuzağa
        // düşmüş olurduk. Görünümün işi GÖSTERMEK; veri hazırlamak değil.
        ViewData["Kategori"] = KitapDeposu.KategoriAdi(kitap.KategoriId);

        return View(kitap);                // güçlü tipli görünüm: @model Kitap
    }

    // GET /Kitap/Ara?kelime=roman
    // Index ile AYNI kartı kullanır — kart artık _KitapKart partial'ında.
    public IActionResult Ara(string? kelime)
    {
        ViewData["Title"] = "Arama";
        ViewData["Kelime"] = kelime;
        return View(string.IsNullOrWhiteSpace(kelime)
            ? []
            : KitapDeposu.Ara(kelime));
    }

    // GET /Kitap/Sozdizimi — Razor sözdizimi kılavuzu (ders notu bölüm 1)
    // Görünüm veri almaz; amacı sözdizimini ve ürettiği HTML'i göstermektir.
    public IActionResult Sozdizimi() => View();

    // GET /Kitap/Tuzak  — 🕳️ KLASİK TUZAK (ders notu bölüm 3.5)
    // Bu action BİLEREK eksiktir: görünüme hiç veri göndermiyor.
    // Views/Kitap/Tuzak.cshtml veriyi @inject ile KENDİ çekiyor.
    // Kod çalışır. Neden yanlış olduğu görünümün içinde yazılı.
    public IActionResult Tuzak()
    {
        ViewData["Title"] = "Klasik Tuzak";
        return View();
    }
}
