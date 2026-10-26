using Microsoft.AspNetCore.Mvc;
using KampusKitap.Data;
using KampusKitap.Models;
using KampusKitap.ViewModels;

namespace KampusKitap.Controllers;

public class KitapController : Controller
{
    // HAFTA 3 — Controller artık Content() değil View() döndürüyor.
    //
    // Hafta 2'de buradaki action'lar düz metin döndürüyordu. Değişen tek şey
    // dönüş tipi değil, SORUMLULUK: HTML üretimi artık view'ın işi.
    //
    // DİKKAT (ders notu bölüm 3.5): veri burada, BİR KEZ alınır. View yalnızca
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

        // Kategori adını BURADA çözüyoruz, view'da değil.
        // View'dan çağırsaydık kod yine çalışırdı — ve bölüm 3.5'teki tuzağa
        // düşmüş olurduk. View'ın işi GÖSTERMEK; veri hazırlamak değil.
        ViewData["Kategori"] = KitapDeposu.KategoriAdi(kitap.KategoriId);

        return View(kitap);                // güçlü tipli view: @model Kitap
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

    // ---------------------------------------------------------------
    // HAFTA 4 — form: GET/POST çifti, ViewModel, PRG
    // ---------------------------------------------------------------

    // GET /Kitap/Ekle → boş formu göster
    [HttpGet]
    public IActionResult Ekle()
    {
        ViewData["Title"] = "Kitap ekle";
        return View(new KitapEkleVM());
    }

    // POST /Kitap/Ekle → gelen veriyi işle
    //
    // Parametre ENTITY değil, ViewModel (Hafta 4 ders notu bölüm 4.4). Kullanıcı
    // Onayli=true gönderse bile bağlanacak özellik olmadığı için düşer.
    [HttpPost]
    public IActionResult Ekle(KitapEkleVM girdi)
    {
        ViewData["Title"] = "Kitap ekle";

        // Model Binding başarısız olabilir ve HATA FIRLATMAZ (Hafta 4 ders notu bölüm 3.7).
        // Tek ayırt edici ModelState'tir.
        if (!ModelState.IsValid)
            return View(girdi);          // formu hatalarla geri göster

        // Entity'yi BİZ oluşturuyoruz; hassas alanın kararı bizde.
        var kitap = new Kitap
        {
            Baslik = girdi.Baslik,
            Yazar = girdi.Yazar,
            Fiyat = girdi.Fiyat ?? 0,              // doğrulama geçtiyse null değildir
            KategoriId = girdi.KategoriId ?? 0,
            Onayli = false               // ← istemciden GELMİYOR
        };
        KitapDeposu.Ekle(kitap);

        // PRG'nin R'si (Hafta 4 ders notu bölüm 4.2): View() dönersek F5 kaydı tekrar ekler.
        TempData["Mesaj"] = $"\"{kitap.Baslik}\" eklendi.";
        return RedirectToAction(nameof(Index));
    }

    // ---------------------------------------------------------------
    // HAFTA 5 — AYNI form, Tag Helper'larla (Hafta 5 ders notu bölüm 3)
    //
    // Action kodu Ekle ile neredeyse birebir aynı. Değişen tek şey GÖRÜNÜM —
    // ve POST tarafına eklenen [ValidateAntiForgeryToken].
    // ---------------------------------------------------------------

    // GET /Kitap/EkleTH
    [HttpGet]
    public IActionResult EkleTH()
    {
        ViewData["Title"] = "Kitap ekle (Tag Helper)";
        return View(new KitapEkleVM());
    }

    // POST /Kitap/EkleTH
    //
    // [ValidateAntiForgeryToken] olmadan token ÜRETİLİR ama KONTROL EDİLMEZ.
    // Üretmek yetmez; doğrulamayı da istemek gerekir (ders notu bölüm 4.3).
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EkleTH(KitapEkleVM girdi)
    {
        ViewData["Title"] = "Kitap ekle (Tag Helper)";

        // HAFTA 6 — buraya gelindiğinde çatı modeli ZATEN doğruladı: attribute'ler
        // ve IValidatableObject çalıştı, sonuçları ModelState'te bekliyor.
        //
        // İş kuralı ise attribute'e yazılmaz: veri deposuna bakmak gerekir.
        // Kontrolü burada yapıp sonucu AYNI yere, ModelState'e ekliyoruz.
        // TODO 6.4 · İş kuralı: aynı başlıkta bir kitap varsa `ModelState.AddModelError` ile hata ekleyin
        //      ipucu: Hafta 6 ders notu bölüm 3.4 — anahtar `nameof(KitapEkleVM.Baslik)` olursa hata Başlık alanının altına düşer. "Suç ve Ceza" ekleyerek deneyin

        // Kanonik kalıp (Hafta 4'ten beri burada): geçersizse formu hatalarla
        // geri göster (200), geçerliyse kaydet ve yönlendir (302, PRG).
        if (!ModelState.IsValid)
            return View(girdi);

        var kitap = new Kitap
        {
            Baslik = girdi.Baslik,
            Yazar = girdi.Yazar,
            Fiyat = girdi.Fiyat ?? 0,              // doğrulama geçtiyse null değildir
            KategoriId = girdi.KategoriId ?? 0,
            Onayli = false
        };
        KitapDeposu.Ekle(kitap);

        TempData["Mesaj"] = $"\"{kitap.Baslik}\" eklendi (Tag Helper formu).";
        return RedirectToAction(nameof(Index));
    }

    // POST /Kitap/EkleYanlis → PRG'SİZ sürüm, yalnızca karşılaştırma için
    //
    // ⚠️ BİLEREK YANLIŞ: başarı durumunda View() dönüyor. Adres çubuğunda POST
    // kalır; F5 kaydı İKİNCİ KEZ ekler. Derste ikisi yan yana denenir.
    [HttpPost]
    public IActionResult EkleYanlis(KitapEkleVM girdi)
    {
        ViewData["Title"] = "PRG'siz ekleme";
        if (!ModelState.IsValid) return View("Ekle", girdi);

        KitapDeposu.Ekle(new Kitap
        {
            Baslik = girdi.Baslik, Yazar = girdi.Yazar,
            Fiyat = girdi.Fiyat ?? 0, KategoriId = girdi.KategoriId ?? 0
        });
        ViewData["Adet"] = KitapDeposu.Hepsi.Count;
        return View("EkleYanlis");       // ⚠️ PRG yok
    }

    // GET /Kitap/Sozdizimi — Razor sözdizimi kılavuzu (ders notu bölüm 1)
    // View veri almaz; amacı sözdizimini ve ürettiği HTML'i göstermektir.
    public IActionResult Sozdizimi() => View();

    // GET /Kitap/Tuzak  — 🕳️ KLASİK TUZAK (ders notu bölüm 3.5)
    // Bu action BİLEREK eksiktir: view'a hiç veri göndermiyor.
    // Views/Kitap/Tuzak.cshtml veriyi @inject ile KENDİ çekiyor.
    // Kod çalışır. Neden yanlış olduğu view'ın içinde yazılı.
    public IActionResult Tuzak()
    {
        ViewData["Title"] = "Klasik Tuzak";
        return View();
    }
}
