using Microsoft.AspNetCore.Mvc;
using KampusKitap.Services;

namespace KampusKitap.Controllers;

public class KitapController : Controller
{
    private readonly IZiyaretSayaci _sayac;

    // Kurucu enjeksiyonu — nesneyi biz üretmiyoruz, çatı veriyor (ders notu §5.2)
    public KitapController(IZiyaretSayaci sayac)
    {
        _sayac = sayac;
    }

    // GET /Kitap  ·  GET /Kitap/Index
    public IActionResult Index()
    {
        return Content($"Kitap listesi — {_sayac.Arttir()}. ziyaret");
    }

    // GET /Kitap/Detay/42   → id = 42
    // GET /Kitap/Detay      → id = null  (route'ta {id?} opsiyonel olduğu için int? kullanıldı)
    //
    // DİKKAT: parametreyi "int id" yazarsanız, id gelmediğinde 0 olur ve
    //         hata almazsınız. Ders notu §3.2'deki sessiz tuzak budur.
    public IActionResult Detay(int? id)
    {
        if (id is null)
            return Content("Kitap seçilmedi. /Kitap/Detay/42 deneyin.");

        return Content($"Kitap #{id} detayı");
    }

    // GET /Kitap/Ara?kelime=roman&sayfa=2
    // Değerler sorgu dizesinden geliyor (ders notu §2.6)
    public IActionResult Ara(string? kelime, int sayfa = 1)
    {
        if (string.IsNullOrWhiteSpace(kelime))
            return Content("Arama kelimesi girin: /Kitap/Ara?kelime=roman");

        return Content($"'{kelime}' aranıyor — sayfa {sayfa}");
    }
}
