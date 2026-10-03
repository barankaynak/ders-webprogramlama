using Microsoft.AspNetCore.Mvc;
using KampusKitap.Services;

namespace KampusKitap.ViewComponents;

/// <summary>
/// HAFTA 3 — View Component (ders notu §3.2).
///
/// Partial View ile farkı tek cümlede:
///   Partial veriyi ALIR  ·  View Component veriyi KENDİ GETİRİR.
///
/// Bu rozet layout'a konuldu; üç sayfada da görünüyor ve HİÇBİR controller
/// bunun için tek satır yazmadı. Partial ile yapsaydık, rozeti kullanan her
/// controller sayacı alıp ViewBag'e koymak zorunda kalırdı.
///
/// DİKKAT: Bu bir controller DEĞİLDİR.
///   - route'lanmaz, kendi adresi (URL) yoktur
///   - doğrudan HTTP isteği almaz
///   - model binding ve filtre hattından geçmez
/// Ama Hafta 2'nin Dependency Injection'ı aynen çalışır.
/// </summary>
public class ZiyaretRozetiViewComponent : ViewComponent
{
    private readonly IZiyaretSayaci _sayac;

    // Kurucu enjeksiyonu — Hafta 2 §5.2 ile birebir aynı.
    public ZiyaretRozetiViewComponent(IZiyaretSayaci sayac)
    {
        _sayac = sayac;
    }

    // Görünümü aranan yol SÖZLEŞMEDİR, isteğe bağlı değil:
    //   Views/Shared/Components/ZiyaretRozeti/Default.cshtml
    // Klasör adı = sınıf adından "ViewComponent" soneki atılmış hâli.
    public IViewComponentResult Invoke()
    {
        var deger = _sayac.Arttir();   // ← VERİYİ KENDİ GETİRİYOR
        return View(deger);
    }
}
