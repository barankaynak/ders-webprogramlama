using Microsoft.AspNetCore.Mvc;
using KampusKitap.Models;
using KampusKitap.Services;

namespace KampusKitap.ViewComponents;

/// <summary>
/// HAFTA 7 — menüdeki sepet rozeti (Hafta 7 ders notu bölüm 3.6).
///
/// Hafta 3'teki ZiyaretRozeti ile aynı fikir: veriyi KENDİ getirir. Bu kez kaynak
/// bir servis değil, request'in session'ı. Layout'ta durduğu için her sayfada çalışır;
/// hiçbir controller sepet sayısını ViewBag'e koymak zorunda kalmaz.
///
/// Session'ı yalnızca OKUR. Ölçüldü: session'ı yalnızca okuyan bir request
/// .AspNetCore.Session cookie'si üretmez; cookie ilk YAZMADA gelir.
/// </summary>
public class SepetRozetiViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        // ✔ TODO 7.7 · Rozet sepetteki toplam adedi göstersin: sepeti session'dan okuyun
        var sepet = HttpContext.Session.NesneOku<List<SepetSatiri>>("Sepet") ?? [];
        return View(sepet.Sum(s => s.Adet));
    }
}
