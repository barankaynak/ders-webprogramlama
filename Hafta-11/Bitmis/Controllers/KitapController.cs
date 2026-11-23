using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KampusKitap.Data;
using KampusKitap.Models;
using KampusKitap.ViewModels;

namespace KampusKitap.Controllers;

public class KitapController : Controller
{
    // HAFTA 10 — veri veritabanından (Hafta 10 ders notu bölüm 6).
    // Context'i DI verir (Program.cs); her request'e yeni bir context.
    private readonly KampusKitapContext _db;

    public KitapController(KampusKitapContext db)
    {
        _db = db;
    }

    // HAFTA 11 — bütün action'lar ASYNC (Hafta 11 ders notu bölüm 3.2 – 3.3).
    // Dönüş tipi Task<IActionResult>; veritabanına giden her çağrı ...Async ve önünde await.
    // await, sorgu veritabanında beklerken thread'i bırakır: sunucu o sırada başka request'lere bakar.

    // HAFTA 3 — DİKKAT (Hafta 3 ders notu bölüm 3.5): veri controller'da, BİR KEZ alınır.
    // View yalnızca gösterir. Sorguyu .cshtml içine taşırsanız kod çalışır ama N+1 doğar
    // ve hiçbir hata almazsınız (Hafta 11 ders notu bölüm 3.1, /Kitap/Tuzak).

    // GET /Kitap  ·  /Kitap?kategori=1&sirala=fiyat-azalan&sayfa=2
    public async Task<IActionResult> Index(int? kategori, string? sirala, int sayfa = 1)
    {
        ViewData["Title"] = "Kitaplar";
        await KategorileriYukle(kategori);

        // HAFTA 7 — cookie istemci verisidir; değere güvenmeden çeviriyoruz.
        if (int.TryParse(Request.Cookies["SonBakilan"], out var sonId))
            ViewData["SonBakilan"] = await _db.Kitaplar.FindAsync(sonId);

        var vm = new KitapListeVM { KategoriId = kategori, Sirala = sirala };
        const int sayfaBoyu = 4;

        // HAFTA 11 — sorgu VERİTABANINDA kurulur (Hafta 11 ders notu bölüm 2.3).
        // Tip IQueryable<Kitap>: Where, OrderBy, Skip, Take bir SQL cümlesine eklenir; hiçbiri
        // veri getirmez. Veri sondaki ToListAsync()'te gelir: yalnızca o sayfanın kitapları.
        // Konsolda: WHERE "k"."KategoriId" = @p … ORDER BY … LIMIT @p OFFSET @p (ölçüldü).
        // ✔ TODO 11.4 · Liste sorgusunu veritabanına taşıyın: IQueryable ile kurun, ToListAsync en sona
        IQueryable<Kitap> sorgu = _db.Kitaplar;

        if (kategori is not null)
            sorgu = sorgu.Where(k => k.KategoriId == kategori);

        sorgu = sirala switch
        {
            "fiyat"        => sorgu.OrderBy(k => k.Fiyat).ThenBy(k => k.Baslik),
            "fiyat-azalan" => sorgu.OrderByDescending(k => k.Fiyat).ThenBy(k => k.Baslik),
            _              => sorgu.OrderBy(k => k.Baslik)
        };

        vm.ToplamKitap = await sorgu.CountAsync();              // SELECT COUNT(*) … WHERE …
        vm.ToplamSayfa = Math.Max(1, (int)Math.Ceiling(vm.ToplamKitap / (double)sayfaBoyu));
        vm.Sayfa = Math.Clamp(sayfa, 1, vm.ToplamSayfa);
        vm.Kitaplar = await sorgu
            .Skip((vm.Sayfa - 1) * sayfaBoyu)
            .Take(sayfaBoyu)
            .ToListAsync();                                     // SELECT … LIMIT @p OFFSET @p
        return View(vm);
    }

    // GET /Kitap/Detay/42
    public async Task<IActionResult> Detay(int? id)
    {
        if (id is null)
            return View("Secilmedi");

        // HAFTA 11 — Include: kitabı KATEGORİSİYLE birlikte getir (Hafta 11 ders notu bölüm 1.5).
        // Find, Include ile birleşmez; tek kayıt için FirstOrDefaultAsync kullanılır.
        // Konsolda: … FROM "Kitaplar" AS "k" INNER JOIN "Kategoriler" AS "k0" ON … (ölçüldü).
        // ✔ TODO 11.3 · Kitabı kategorisiyle birlikte getirin: Include
        var kitap = await _db.Kitaplar
            .Include(k => k.Kategori)
            .FirstOrDefaultAsync(k => k.Id == id);
        if (kitap is null)
            return NotFound();
        ViewData["Kategori"] = kitap.Kategori?.Ad;

        ViewData["Title"] = kitap.Baslik;

        // HAFTA 9 — "Aynı kategoriden": artık veritabanında (WHERE … ORDER BY … LIMIT 3).
        ViewData["Benzer"] = await _db.Kitaplar
            .Where(k => k.KategoriId == kitap.KategoriId && k.Id != kitap.Id)
            .OrderBy(k => k.Fiyat)
            .Take(3)
            .ToListAsync();

        // HAFTA 7 — cookie yazmak; tarayıcı onu SONRAKİ request'lerde geri getirir.
        Response.Cookies.Append("SonBakilan", kitap.Id.ToString(), new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddDays(30),
            HttpOnly = true,
            SameSite = SameSiteMode.Lax
        });

        return View(kitap);
    }

    // GET /Kitap/Ara?kelime=ince
    public async Task<IActionResult> Ara(string? kelime)
    {
        ViewData["Title"] = "Arama";
        ViewData["Kelime"] = kelime;
        if (string.IsNullOrWhiteSpace(kelime))
            return View(new List<Kitap>());

        // HAFTA 11 — BİLİNÇLİ OLARAK bellekte (Hafta 11, 🕳️ Klasik Tuzak).
        // Türkçe büyük/küçük harf karşılaştırmasını SQLite bilmez: veritabanında Contains("ince")
        // "İnce Memed"i bulmadı; StringComparison.CurrentCultureIgnoreCase'li Contains ise SQL'e
        // ÇEVRİLEMEDİ (InvalidOperationException) — ikisi de ölçüldü. Kitap sayısı küçük olduğu
        // için tabloyu alıp Türkçe kuralla C#'ta süzüyoruz.
        var kitaplar = await _db.Kitaplar.ToListAsync();
        return View(kitaplar
            .Where(k => k.Baslik.Contains(kelime, StringComparison.CurrentCultureIgnoreCase)
                     || k.Yazar.Contains(kelime, StringComparison.CurrentCultureIgnoreCase))
            .ToList());
    }

    // ---------------------------------------------------------------
    // HAFTA 4 — form: GET/POST çifti, ViewModel, PRG
    // ---------------------------------------------------------------

    [HttpGet]
    public IActionResult Ekle()
    {
        ViewData["Title"] = "Kitap ekle";
        return View(new KitapEkleVM());
    }

    // Parametre ENTITY değil, ViewModel (Hafta 4 ders notu bölüm 4.4): over-posting'e kapalı.
    [HttpPost]
    public async Task<IActionResult> Ekle(KitapEkleVM girdi)
    {
        ViewData["Title"] = "Kitap ekle";
        if (!ModelState.IsValid)
            return View(girdi);

        var kitap = new Kitap
        {
            Baslik = girdi.Baslik,
            Yazar = girdi.Yazar,
            Fiyat = girdi.Fiyat ?? 0,
            KategoriId = girdi.KategoriId ?? 0,
            Onayli = false               // ← istemciden GELMİYOR
        };

        // HAFTA 10 — Add yalnızca işaretler (Added); INSERT SaveChanges'te gider.
        // HAFTA 11 — SaveChangesAsync: aynı iş, beklerken thread'i bırakarak.
        _db.Kitaplar.Add(kitap);
        await _db.SaveChangesAsync();

        TempData["Mesaj"] = $"\"{kitap.Baslik}\" eklendi.";
        return RedirectToAction(nameof(Index));
    }

    // ---------------------------------------------------------------
    // HAFTA 5 — AYNI form, Tag Helper'larla + [ValidateAntiForgeryToken]
    // ---------------------------------------------------------------

    [HttpGet]
    public async Task<IActionResult> EkleTH()
    {
        ViewData["Title"] = "Kitap ekle (Tag Helper)";
        await KategorileriYukle();
        return View(new KitapEkleVM());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EkleTH(KitapEkleVM girdi)
    {
        ViewData["Title"] = "Kitap ekle (Tag Helper)";

        // HAFTA 6 — iş kuralı: aynı başlık ikinci kez eklenemez.
        // HAFTA 11 — yalnızca BAŞLIKLAR getirilir (SELECT "k"."Baslik"), karşılaştırma bellekte:
        // Türkçe büyük/küçük harf kuralını SQLite bilmez (Klasik Tuzak).
        var basliklar = await _db.Kitaplar.Select(k => k.Baslik).ToListAsync();
        if (basliklar.Any(b => string.Equals(b, girdi.Baslik?.Trim(), StringComparison.CurrentCultureIgnoreCase)))
            ModelState.AddModelError(nameof(KitapEkleVM.Baslik), "Bu başlıkta bir kitap zaten var.");

        if (!ModelState.IsValid)
        {
            await KategorileriYukle(girdi.KategoriId);
            return View(girdi);
        }

        var kitap = new Kitap
        {
            Baslik = girdi.Baslik,
            Yazar = girdi.Yazar,
            Fiyat = girdi.Fiyat ?? 0,
            KategoriId = girdi.KategoriId ?? 0,
            Onayli = false
        };
        _db.Kitaplar.Add(kitap);
        await _db.SaveChangesAsync();

        TempData["Mesaj"] = $"\"{kitap.Baslik}\" eklendi (Tag Helper formu).";
        return RedirectToAction(nameof(Index));
    }

    // ⚠️ BİLEREK YANLIŞ (Hafta 4): PRG yok; F5 kaydı İKİNCİ KEZ ekler.
    [HttpPost]
    public async Task<IActionResult> EkleYanlis(KitapEkleVM girdi)
    {
        ViewData["Title"] = "PRG'siz ekleme";
        if (!ModelState.IsValid) return View("Ekle", girdi);

        _db.Kitaplar.Add(new Kitap
        {
            Baslik = girdi.Baslik, Yazar = girdi.Yazar,
            Fiyat = girdi.Fiyat ?? 0, KategoriId = girdi.KategoriId ?? 0
        });
        await _db.SaveChangesAsync();
        ViewData["Adet"] = await _db.Kitaplar.CountAsync();     // SELECT COUNT(*) FROM "Kitaplar"
        return View("EkleYanlis");
    }

    // ---------------------------------------------------------------
    // HAFTA 10 — düzenlemek ve silmek; HAFTA 11 — async
    // ---------------------------------------------------------------

    [HttpGet]
    public async Task<IActionResult> Duzenle(int id)
    {
        var kitap = await _db.Kitaplar.FindAsync(id);
        if (kitap is null)
            return NotFound();

        ViewData["Title"] = "Kitap düzenle";
        await KategorileriYukle(kitap.KategoriId);
        return View(new KitapEkleVM
        {
            Baslik = kitap.Baslik, Yazar = kitap.Yazar,
            Fiyat = kitap.Fiyat, KategoriId = kitap.KategoriId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Duzenle(int id, KitapEkleVM girdi)
    {
        var basliklar = await _db.Kitaplar.Where(k => k.Id != id).Select(k => k.Baslik).ToListAsync();
        if (basliklar.Any(b => string.Equals(b, girdi.Baslik?.Trim(), StringComparison.CurrentCultureIgnoreCase)))
            ModelState.AddModelError(nameof(KitapEkleVM.Baslik), "Bu başlıkta bir kitap zaten var.");

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Kitap düzenle";
            await KategorileriYukle(girdi.KategoriId);
            return View(girdi);
        }

        // Change tracking (Hafta 10): FindAsync'in getirdiği nesne izlenir; değişen sütun yazılır.
        var kitap = await _db.Kitaplar.FindAsync(id);
        if (kitap is null)
            return NotFound();
        kitap.Baslik = girdi.Baslik;
        kitap.Yazar = girdi.Yazar;
        kitap.Fiyat = girdi.Fiyat ?? 0;
        kitap.KategoriId = girdi.KategoriId ?? 0;
        await _db.SaveChangesAsync();

        TempData["Mesaj"] = $"\"{girdi.Baslik}\" güncellendi.";
        return RedirectToAction(nameof(Detay), new { id });
    }

    // GET /Kitap/Sil/3 yalnızca sorar; silme POST'ta (Hafta 4–5).
    [HttpGet]
    public async Task<IActionResult> Sil(int id)
    {
        var kitap = await _db.Kitaplar.FindAsync(id);
        if (kitap is null)
            return NotFound();

        ViewData["Title"] = "Kitap sil";
        return View(kitap);
    }

    [HttpPost, ActionName("Sil")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SilOnay(int id)
    {
        var kitap = await _db.Kitaplar.FindAsync(id);
        if (kitap is null)
            return NotFound();
        _db.Kitaplar.Remove(kitap);
        await _db.SaveChangesAsync();

        TempData["Mesaj"] = "Kitap silindi.";
        return RedirectToAction(nameof(Index));
    }

    // GET /Kitap/Sozdizimi — Razor sözdizimi kılavuzu (Hafta 3)
    public IActionResult Sozdizimi() => View();

    // ---------------------------------------------------------------
    // HAFTA 9 — İstatistik; HAFTA 11 — hesap VERİTABANINDA (Hafta 11 ders notu bölüm 2.4)
    // Her satır ayrı, küçük bir SQL: SELECT COUNT(*), SELECT ef_sum(…), … (ölçüldü).
    // Tablo belleğe hiç gelmez; yalnızca sonuçlar gelir.
    // ---------------------------------------------------------------

    // GET /Kitap/Istatistik
    public async Task<IActionResult> Istatistik()
    {
        ViewData["Title"] = "İstatistik";
        var vm = new IstatistikVM();

        // ✔ TODO 11.5 · İstatistiği veritabanında hesaplayın: toplama metotlarının Async hâlleri; gruplamayı kategori adına göre navigation üzerinden yapın
        vm.KitapSayisi     = await _db.Kitaplar.CountAsync();
        vm.ToplamFiyat     = await _db.Kitaplar.SumAsync(k => k.Fiyat);
        vm.OrtalamaFiyat   = await _db.Kitaplar.AverageAsync(k => k.Fiyat);
        vm.EnUcuz          = await _db.Kitaplar.MinAsync(k => k.Fiyat);
        vm.EnPahali        = await _db.Kitaplar.MaxAsync(k => k.Fiyat);
        vm.UcuzKitapSayisi = await _db.Kitaplar.CountAsync(k => k.Fiyat < 150);
        vm.PahaliVarMi     = await _db.Kitaplar.AnyAsync(k => k.Fiyat > 500);
        vm.HepsiOnayli     = await _db.Kitaplar.AllAsync(k => k.Onayli);

        // Gruplama anahtarı artık kategorinin ADI — navigation üzerinden (JOIN). Hafta 9'da
        // KategoriId'ye göre gruplayıp adı KitapDeposu.KategoriAdi'ndan alıyorduk.
        vm.Kategoriler = await _db.Kitaplar
            .GroupBy(k => k.Kategori!.Ad)
            .Select(g => new KategoriOzetVM
            {
                Ad            = g.Key,
                Adet          = g.Count(),
                OrtalamaFiyat = g.Average(k => k.Fiyat)
            })
            .OrderByDescending(o => o.Adet)
            .ToListAsync();

        vm.Yazarlar = await _db.Kitaplar
            .Select(k => k.Yazar)
            .Distinct()
            .OrderBy(y => y)
            .ToListAsync();                 // SELECT DISTINCT … ORDER BY

        return View(vm);
    }

    // GET /Kitap/Tuzak  — N+1 (Hafta 11 ders notu bölüm 3.1)
    // ⚠️ duzelt=false iken BİLEREK YANLIŞ: action view'a veri göndermiyor; view kitapları
    // KENDİ çekiyor ve her kitap için kategorisini AYRI bir sorguyla soruyor: 1 + N sorgu.
    // Hata yok, sayfa doğru — konsolda SELECT'leri sayın.
    // GET /Kitap/Tuzak?duzelt=true — doğrusu: tek sorgu, JOIN ile.
    public async Task<IActionResult> Tuzak(bool duzelt = false)
    {
        ViewData["Title"] = "N+1";
        if (!duzelt)
            return View();

        // ✔ TODO 11.7 · N+1'i düzeltin: kitapları kategorileriyle TEK sorguda getirin
        var kitaplar = await _db.Kitaplar
            .Include(k => k.Kategori)
            .OrderBy(k => k.Id)
            .ToListAsync();
        return View(kitaplar);
    }

    // Formlardaki kategori listesi artık VERİTABANINDAN (Hafta 5'ten Hafta 10'a kadar KitapDeposu'ndaydı).
    private async Task KategorileriYukle(int? secili = null)
    {
        var kategoriler = await _db.Kategoriler.OrderBy(k => k.Id).ToListAsync();
        ViewData["Kategoriler"] = new SelectList(kategoriler, "Id", "Ad", secili);
    }
}
