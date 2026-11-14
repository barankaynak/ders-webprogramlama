using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using KampusKitap.Data;
using KampusKitap.Models;
using KampusKitap.ViewModels;

namespace KampusKitap.Controllers;

public class KitapController : Controller
{
    // HAFTA 9 — veri artık KitapDeposu'ndan değil, veritabanından (Hafta 9 ders notu bölüm 2).
    // Context'i DI verir (Program.cs, TODO 9.1); her request'e yeni bir context.
    private readonly KampusKitapContext _db;

    public KitapController(KampusKitapContext db)
    {
        _db = db;
    }

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

        // HAFTA 7 — cookie'yi okumak (Hafta 7 ders notu bölüm 2.3).
        // Cookie İSTEMCİ VERİSİDİR: kullanıcı F12'den "abc" ya da "999" yazabilir.
        // Bu yüzden değere güvenmeden çeviriyoruz (Hafta 4'ün ilkesi).
        if (int.TryParse(Request.Cookies["SonBakilan"], out var sonId))
            ViewData["SonBakilan"] = _db.Kitaplar.Find(sonId);

        // HAFTA 9 — ToList() sorguyu ÇALIŞTIRIR: SELECT … FROM "Kitaplar" (konsolda görünür).
        // DbSet'in kendisi bir sorgudur, liste değildir; veri ToList()'te gelir.
        // ✔ TODO 9.3 · Kitapları veritabanından okuyun: liste için ToList, tek kitap için Find
        var kitaplar = _db.Kitaplar.ToList();

        return View(kitaplar);
    }

    // GET /Kitap/Detay/42
    public IActionResult Detay(int? id)
    {
        if (id is null)
            return View("Secilmedi");      // Views/Kitap/Secilmedi.cshtml

        // Find: birincil anahtarla tek satır. Bulamazsa null döner, hata fırlatmaz (ölçüldü).
        // ✔ TODO 9.3 · Kitapları veritabanından okuyun: liste için ToList, tek kitap için Find
        var kitap = _db.Kitaplar.Find(id.Value);
        if (kitap is null)
            return NotFound();

        ViewData["Title"] = kitap.Baslik;

        // Kategori adını BURADA çözüyoruz, view'da değil.
        // View'dan çağırsaydık kod yine çalışırdı — ve bölüm 3.5'teki tuzağa
        // düşmüş olurduk. View'ın işi GÖSTERMEK; veri hazırlamak değil.
        ViewData["Kategori"] = _db.Kategoriler.Find(kitap.KategoriId)?.Ad;

        // HAFTA 7 — cookie yazmak (Hafta 7 ders notu bölüm 2.2).
        // Cookie bu response'un Set-Cookie başlığıyla gider; tarayıcı onu SONRAKİ
        // request'lerde geri getirir. Aynı request'te Request.Cookies'te YOKTUR (ölçüldü).
        Response.Cookies.Append("SonBakilan", kitap.Id.ToString(), new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddDays(30),   // yoksa tarayıcı kapanınca silinir
            HttpOnly = true,                            // JavaScript okuyamaz
            SameSite = SameSiteMode.Lax                 // başka sitenin POST'unda gönderilmez
        });

        return View(kitap);                // güçlü tipli view: @model Kitap
    }

    // GET /Kitap/Ara?kelime=roman
    // Index ile AYNI kartı kullanır — kart artık _KitapKart partial'ında.
    public IActionResult Ara(string? kelime)
    {
        ViewData["Title"] = "Arama";
        ViewData["Kelime"] = kelime;
        if (string.IsNullOrWhiteSpace(kelime))
            return View(new List<Kitap>());

        // HAFTA 9 — bütün tabloyu belleğe alıp C#'ta süzüyoruz. Doğru çalışır, ama
        // 10.000 kitapta 10.000 satır taşınır. Süzmeyi veritabanına yaptırmak (WHERE)
        // Hafta 10'un konusu: LINQ.
        var hepsi = _db.Kitaplar.ToList();
        return View(hepsi.Where(k => k.Baslik.Contains(kelime, StringComparison.OrdinalIgnoreCase)
                                  || k.Yazar.Contains(kelime, StringComparison.OrdinalIgnoreCase))
                         .ToList());
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

        // HAFTA 9 — Add() yalnızca context'e "bunu ekle" der (durum: Added).
        // INSERT, SaveChanges()'te gider; Id de o anda dolar (ölçüldü: 0 → 43).
        // ✔ TODO 9.4 · Yeni kitabı veritabanına kaydedin: Add, sonra SaveChanges
        _db.Kitaplar.Add(kitap);
        _db.SaveChanges();

        // PRG'nin R'si (Hafta 4 ders notu bölüm 4.2): View() dönersek F5 kaydı tekrar ekler.
        TempData["Mesaj"] = $"\"{kitap.Baslik}\" eklendi.";
        return RedirectToAction(nameof(Index));
    }

    // ---------------------------------------------------------------
    // HAFTA 5 — AYNI form, Tag Helper'larla (Hafta 5 ders notu bölüm 3)
    //
    // Action kodu Ekle ile neredeyse birebir aynı. Değişen tek şey VIEW —
    // ve POST tarafına eklenen [ValidateAntiForgeryToken].
    // ---------------------------------------------------------------

    // GET /Kitap/EkleTH
    [HttpGet]
    public IActionResult EkleTH()
    {
        ViewData["Title"] = "Kitap ekle (Tag Helper)";
        ViewData["Kategoriler"] = KategoriListesi();
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
        //
        // HAFTA 9 — tabloyu belleğe alıp bakıyoruz; sorguyu veritabanına yaptırmak Hafta 10.
        if (_db.Kitaplar.ToList().Any(k => string.Equals(k.Baslik, girdi.Baslik?.Trim(),
                StringComparison.CurrentCultureIgnoreCase)))
            ModelState.AddModelError(nameof(KitapEkleVM.Baslik), "Bu başlıkta bir kitap zaten var.");

        // Kanonik kalıp (Hafta 4'ten beri burada): geçersizse formu hatalarla
        // geri göster (200), geçerliyse kaydet ve yönlendir (302, PRG).
        if (!ModelState.IsValid)
        {
            ViewData["Kategoriler"] = KategoriListesi();
            return View(girdi);
        }

        var kitap = new Kitap
        {
            Baslik = girdi.Baslik,
            Yazar = girdi.Yazar,
            Fiyat = girdi.Fiyat ?? 0,              // doğrulama geçtiyse null değildir
            KategoriId = girdi.KategoriId ?? 0,
            Onayli = false
        };
        // ✔ TODO 9.4 · Yeni kitabı veritabanına kaydedin: Add, sonra SaveChanges
        _db.Kitaplar.Add(kitap);
        _db.SaveChanges();

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

        _db.Kitaplar.Add(new Kitap
        {
            Baslik = girdi.Baslik, Yazar = girdi.Yazar,
            Fiyat = girdi.Fiyat ?? 0, KategoriId = girdi.KategoriId ?? 0
        });
        _db.SaveChanges();
        ViewData["Adet"] = _db.Kitaplar.Count();      // SELECT COUNT(*) — Hafta 10
        return View("EkleYanlis");       // ⚠️ PRG yok
    }

    // ---------------------------------------------------------------
    // HAFTA 9 — CRUD'un U ve D'si: düzenlemek ve silmek (Hafta 9 ders notu bölüm 3.5, 3.6)
    // Form yine KitapEkleVM'e bağlanır (over-posting, Hafta 4); entity'yi BİZ güncelleriz.
    // ---------------------------------------------------------------

    // GET /Kitap/Duzenle/3
    [HttpGet]
    public IActionResult Duzenle(int id)
    {
        var kitap = _db.Kitaplar.Find(id);
        if (kitap is null)
            return NotFound();

        ViewData["Title"] = "Kitap düzenle";
        ViewData["Kategoriler"] = KategoriListesi();
        return View(new KitapEkleVM
        {
            Baslik = kitap.Baslik, Yazar = kitap.Yazar,
            Fiyat = kitap.Fiyat, KategoriId = kitap.KategoriId
        });
    }

    // POST /Kitap/Duzenle/3
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Duzenle(int id, KitapEkleVM girdi)
    {
        if (_db.Kitaplar.ToList().Any(k => k.Id != id && string.Equals(k.Baslik, girdi.Baslik?.Trim(),
                StringComparison.CurrentCultureIgnoreCase)))
            ModelState.AddModelError(nameof(KitapEkleVM.Baslik), "Bu başlıkta bir kitap zaten var.");

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Kitap düzenle";
            ViewData["Kategoriler"] = KategoriListesi();
            return View(girdi);
        }

        // Change tracking: Find'ın getirdiği nesne context'in GÖZETİMİNDEDİR (Unchanged).
        // Özelliğini değiştirmek onu Modified yapar; Update() çağırmaya gerek yok.
        // SaveChanges yalnızca DEĞİŞEN sütunları yazar (ölçüldü: UPDATE … SET "Fiyat" = @p0).
        // ✔ TODO 9.5 · Kitabı güncelleyin: Find ile getirin, özelliklerini değiştirin, SaveChanges
        var kitap = _db.Kitaplar.Find(id);
        if (kitap is null)
            return NotFound();
        kitap.Baslik = girdi.Baslik;
        kitap.Yazar = girdi.Yazar;
        kitap.Fiyat = girdi.Fiyat ?? 0;
        kitap.KategoriId = girdi.KategoriId ?? 0;
        _db.SaveChanges();

        TempData["Mesaj"] = $"\"{girdi.Baslik}\" güncellendi.";
        return RedirectToAction(nameof(Detay), new { id });
    }

    // GET /Kitap/Sil/3 → onay sayfası. Silmek veri değiştirir: GET'te SİLMEYİZ (Hafta 4).
    [HttpGet]
    public IActionResult Sil(int id)
    {
        var kitap = _db.Kitaplar.Find(id);
        if (kitap is null)
            return NotFound();

        ViewData["Title"] = "Kitap sil";
        return View(kitap);
    }

    // POST /Kitap/Sil/3
    // GET ile POST'un parametreleri aynı (int id) olduğu için C# iki metoda aynı adı
    // veremez; POST'u SilOnay diye adlandırıp [ActionName("Sil")] ile adresini koruyoruz.
    [HttpPost, ActionName("Sil")]
    [ValidateAntiForgeryToken]
    public IActionResult SilOnay(int id)
    {
        // Remove → durum Deleted; DELETE SaveChanges'te gider; sonra nesne Detached (ölçüldü).
        // ✔ TODO 9.6 · Kitabı silin: Find, Remove, SaveChanges
        var kitap = _db.Kitaplar.Find(id);
        if (kitap is null)
            return NotFound();
        _db.Kitaplar.Remove(kitap);
        _db.SaveChanges();

        TempData["Mesaj"] = "Kitap silindi.";
        return RedirectToAction(nameof(Index));
    }

    // Formlardaki kategori listesi artık veritabanından (Hafta 5'te KitapDeposu.Kategoriler'di).
    private SelectList KategoriListesi() => new(_db.Kategoriler.ToList(), "Id", "Ad");

    // GET /Kitap/Sozdizimi — Razor sözdizimi kılavuzu (ders notu bölüm 1)
    // View veri almaz; amacı sözdizimini ve ürettiği HTML'i göstermektir.
    public IActionResult Sozdizimi() => View();

    // GET /Kitap/Tuzak  — 🕳️ KLASİK TUZAK (ders notu bölüm 3.5)
    // Bu action BİLEREK eksiktir: view'a hiç veri göndermiyor.
    // Views/Kitap/Tuzak.cshtml veriyi @inject ile KENDİ çekiyor.
    // Kod çalışır. Neden yanlış olduğu view'ın içinde yazılı.
    // HAFTA 9 — artık veritabanına gidiyor: konsolda her satır için bir SELECT görünür (N+1).
    public IActionResult Tuzak()
    {
        ViewData["Title"] = "Klasik Tuzak";
        return View();
    }
}
