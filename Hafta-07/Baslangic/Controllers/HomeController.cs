using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KampusKitap.Models;

namespace KampusKitap.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    // Kurucu enjeksiyonu (constructor injection): ILogger'ı biz oluşturmuyoruz,
    // çatı bize veriyor. Bunun nasıl çalıştığı Hafta 2'nin konusu.
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // GET /  veya  GET /Home/Index
    // View() çağrısı, adı bu metotla aynı olan Views/Home/Index.cshtml dosyasını render eder.
    public IActionResult Index()
    {
        return View();
    }

    // GET /Home/Privacy
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
