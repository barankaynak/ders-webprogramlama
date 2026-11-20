using Microsoft.AspNetCore.Mvc;
using KampusKitap.Data;
using KampusKitap.Models;
using KampusKitap.ViewModels;

namespace KampusKitap.Controllers;

/// <summary>
/// HAFTA 9 — LINQ laboratuvarı (Hafta 9 ders notu bölüm 1, 2, 4 ve 6).
///
/// Derste açılan sayfa: her kutuda bir kod parçası ve o kodun BU request'te
/// ürettiği sonuç var. Sonuçlar elle yazılmadı; sayfa her açıldığında yeniden hesaplanır.
/// Kendi denemelerinizi Index'e yeni bir Ekle(…) satırı olarak ekleyin.
/// </summary>
public class SorguController : Controller
{
    // HAFTA 10 — veri veritabanından; context'i DI verir (Hafta 10 ders notu bölüm 4.4).
    private readonly KampusKitapContext _db;

    public SorguController(KampusKitapContext db)
    {
        _db = db;
    }

    // GET /Sorgu
    public IActionResult Index()
    {
        ViewData["Title"] = "LINQ laboratuvarı";
        // HAFTA 10 — liste artık veritabanından gelir; laboratuvarın geri kalanı Hafta 9'daki gibi.
        var kitaplar = _db.Kitaplar.ToList();
        var satirlar = new List<LabSatiri>();
        void Ekle(string baslik, string kod, object? sonuc) =>
            satirlar.Add(new LabSatiri(baslik, kod, Yaz(sonuc)));

        // ---- 1. Döngüden sorguya (bölüm 1.1 – 1.2) — iki metot Data/LinqOrnekleri.cs'te ----
        Ekle("1 · foreach ile: 150 TL altı kitaplar",
            "var ucuzlar = new List<string>();\nforeach (var k in kitaplar)\n{\n    if (k.Fiyat < 150)\n        ucuzlar.Add(k.Baslik);\n}",
            LinqOrnekleri.DonguIle(kitaplar));
        Ekle("1 · Aynı iş LINQ ile",
            "kitaplar.Where(k => k.Fiyat < 150).Select(k => k.Baslik).ToList()",
            LinqOrnekleri.LinqIle(kitaplar));

        // ---- 2. Dil zemini (bölüm 2) ----
        Func<Kitap, bool> ucuzMu = k => k.Fiyat < 150;
        Ekle("2.2 · Lambda bir fonksiyondur: değişkene konur, çağrılır",
            "Func<Kitap, bool> ucuzMu = k => k.Fiyat < 150;\nucuzMu(kitaplar[0])   // Suç ve Ceza, 120 TL",
            ucuzMu(kitaplar[0]));
        Ekle("2.2 · … ve Where'e verilir",
            "kitaplar.Where(ucuzMu).Count()",
            kitaplar.Where(ucuzMu).Count());
        Ekle("2.3 · Extension method: iki yazım, aynı çağrı",
            "Enumerable.Where(kitaplar, ucuzMu).Count()",
            Enumerable.Where(kitaplar, ucuzMu).Count());
        Ekle("2.4 · Where'in döndürdüğü şey bir liste DEĞİL",
            "kitaplar.Where(ucuzMu).GetType().Name",
            kitaplar.Where(ucuzMu).GetType().Name);
        Ekle("2.5 · Anonim tip: adı olmayan, derleyicinin ürettiği sınıf",
            "kitaplar.Select(k => new { k.Baslik, KdvHaric = k.Fiyat * 0.82m }).First()",
            kitaplar.Select(k => new { k.Baslik, KdvHaric = k.Fiyat * 0.82m }).First());
        Ekle("2.6 · Query syntax",
            "from k in kitaplar\nwhere k.KategoriId == 3\norderby k.Fiyat\nselect k.Baslik",
            from k in kitaplar where k.KategoriId == 3 orderby k.Fiyat select k.Baslik);
        Ekle("2.6 · Method syntax — aynı sonuç",
            "kitaplar.Where(k => k.KategoriId == 3).OrderBy(k => k.Fiyat).Select(k => k.Baslik)",
            kitaplar.Where(k => k.KategoriId == 3).OrderBy(k => k.Fiyat).Select(k => k.Baslik));

        // ---- 4. Tek eleman (bölüm 4) ----
        Ekle("4.1 · First: koşulu sağlayan İLK kitap",
            "kitaplar.First(k => k.Yazar == \"Oğuz Atay\").Baslik",
            kitaplar.First(k => k.Yazar == "Oğuz Atay").Baslik);
        Ekle("4.1 · FirstOrDefault: yoksa null (istisna yok)",
            "kitaplar.FirstOrDefault(k => k.Yazar == \"Tolstoy\")",
            kitaplar.FirstOrDefault(k => k.Yazar == "Tolstoy"));
        Ekle("4.2 · Single: TAM BİR tane olmalı",
            "kitaplar.Single(k => k.Id == 42).Baslik",
            kitaplar.Single(k => k.Id == 42).Baslik);
        Ekle("4.2 · Single, iki eşleşme varsa",
            "kitaplar.Single(k => k.Yazar == \"Dostoyevski\")",
            Dene(() => kitaplar.Single(k => k.Yazar == "Dostoyevski")));
        Ekle("4.3 · Contains: değer listesinde",
            "kitaplar.Select(k => k.Yazar).Contains(\"Cormen\")",
            kitaplar.Select(k => k.Yazar).Contains("Cormen"));

        // ---- 6. Deferred execution (bölüm 6.2) — Can Yüzkollar'ın örneği ----
        int[] sayilar = [0, 1, 2, 3, 4, 5, 6];
        var ciftler = from s in sayilar where s % 2 == 0 select s;   // tanım
        sayilar[1] = 8;                                               // kaynak değişti
        Ekle("6.2 · Ertelenmiş: sorgu foreach'te çalışır, değişikliği GÖRÜR",
            "int[] sayilar = [0, 1, 2, 3, 4, 5, 6];\nvar ciftler = from s in sayilar where s % 2 == 0 select s;\nsayilar[1] = 8;\nciftler   // şimdi çalışıyor",
            ciftler);
        int[] sayilar2 = [0, 1, 2, 3, 4, 5, 6];
        var ciftler2 = (from s in sayilar2 where s % 2 == 0 select s).ToList();
        sayilar2[1] = 8;
        Ekle("6.2 · Anlık: ToList o anki sonucu alır, değişikliği GÖRMEZ",
            "int[] sayilar2 = [0, 1, 2, 3, 4, 5, 6];\nvar ciftler2 = (from s in sayilar2 where s % 2 == 0 select s).ToList();\nsayilar2[1] = 8;\nciftler2",
            ciftler2);

        return View(satirlar);
    }

    // GET /Sorgu/Bos?yazar=Tolstoy — 🐛 HATA KÖŞESİ (Hafta 9 ders notu, Hata Köşesi)
    //
    // ⚠️ BİLEREK HATALI. Listede olmayan bir yazar sorulunca First() İSTİSNA fırlatır:
    //   InvalidOperationException: Sequence contains no matching element
    // Development'ta sarı-kırmızı hata sayfası açılır. Düzeltmesi aşağıdaki Dogru action'ında.
    public IActionResult Bos(string yazar = "Tolstoy")
    {
        ViewData["Title"] = "Hata Köşesi";
        var kitap = _db.Kitaplar.ToList().First(k => k.Yazar == yazar);   // ⚠️
        return View(kitap);
    }

    // GET /Sorgu/Dogru?yazar=Tolstoy — aynı istek, FirstOrDefault + null kontrolü
    public IActionResult Dogru(string yazar = "Tolstoy")
    {
        ViewData["Title"] = "Hata Köşesi — doğrusu";
        var kitap = _db.Kitaplar.ToList().FirstOrDefault(k => k.Yazar == yazar);
        if (kitap is null)
            return NotFound();             // "yok" bir istisna değil, beklenen bir durum: 404
        return View("Bos", kitap);
    }

    // GET /Sorgu/Tuzak — 🕳️ KLASİK TUZAK (Hafta 9 ders notu, Klasik Tuzak)
    //
    // ⚠️ BİLEREK YANLIŞ ve HATA VERMEZ. 150 TL altı kitaplar "indirimde" işaretleniyor;
    // sayfada hiçbiri indirimde görünmüyor. Neden: kartlar bir SORGU, liste değil.
    // foreach kartları bir kez üretip işaretler; view'ın foreach'i sorguyu YENİDEN
    // çalıştırır ve Select YENİ (işaretsiz) kartlar üretir. Düzeltme: Select'in sonuna ToList().
    public IActionResult Tuzak(bool duzelt = false)
    {
        ViewData["Title"] = "Klasik Tuzak";

        IEnumerable<IndirimKartiVM> kartlar = _db.Kitaplar.ToList()
            .Select(k => new IndirimKartiVM { Baslik = k.Baslik, Fiyat = k.Fiyat });

        if (duzelt)
            kartlar = kartlar.ToList();    // ✔ bir kez üret, hep aynı nesneler

        foreach (var kart in kartlar)
        {
            if (kart.Fiyat < 150)
                kart.Indirimde = true;
        }

        ViewData["Duzelt"] = duzelt;
        return View(kartlar);              // ⚠️ duzelt=false iken view sorguyu yeniden çalıştırır
    }

    // ------------------------------------------------------------------ yardımcılar
    private static object? Dene(Func<object?> f)
    {
        try { return f(); }
        catch (Exception e) { return new Istisna($"{e.GetType().Name}: {e.Message}"); }
    }

    private record Istisna(string Mesaj);

    private static string Yaz(object? deger) => deger switch
    {
        null => "null",
        Istisna i => "⚠ " + i.Mesaj,
        string s => $"\"{s}\"",
        System.Collections.IEnumerable liste => "[ " + string.Join(", ",
            liste.Cast<object?>().Select(o => o is string s ? $"\"{s}\"" : o?.ToString() ?? "null")) + " ]",
        _ => deger.ToString() ?? ""
    };
}
