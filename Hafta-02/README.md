# Hafta 2 — ASP.NET Core MVC Temelleri ve Mimari Yapısı

| | |
|---|---|
| 📄 **Ders notu** | [H02-Ders-Notu.pdf](../PDF/H02-Ders-Notu.pdf) — *sınav kapsamı* |
| 🔧 **Uygulama** | [H02-Uygulama.pdf](../PDF/H02-Uygulama.pdf) — derste yapılanların dökümü |
| 📚 **Detay** | [H02-Detay.pdf](../PDF/H02-Detay.pdf) — *sınav kapsamı DIŞI* |
| 🖱️ **Sunum** | [H02-etkilesimli.html](H02-etkilesimli.html) — indirip tarayıcıda açın |

---

## Bu haftanın konusu

- **MVC deseni** — Model, View, Controller hangi soruyu cevaplar
- **Controller ve action** — `IActionResult` ve sonuç tipleri
- **Routing** — `{controller=Home}/{action=Index}/{id?}` kalıbı, kısıtlar, Attribute Routing
- **Middleware Pipeline** — sıra neden kritik, short-circuit
- **Dependency Injection** — servis kaydı, kurucu enjeksiyonu, yaşam döngüleri

🧩 **Haftanın ilkesi — Convention ve sıra.** Çerçeveler sık yapılanı varsayılan yapar.
Ama **sırayı bozmak çoğu zaman hata vermez** — sessizce yanlış davranış üretir.

---

## Projeyi çalıştırma

```bash
cd KampusKitap
dotnet run
```

Ana sayfada **denenecek adreslerin listesi** var. Her satır ders notundaki bir konuya karşılık geliyor.

### Bu hafta eklenenler

| Dosya | Ne için |
|---|---|
| `Controllers/KitapController.cs` | Üç action; DI ile servis alıyor |
| `Controllers/KategoriController.cs` | `NotFound()` ve `RedirectToAction()` örnekleri |
| `Services/IZiyaretSayaci.cs` + `ZiyaretSayaci.cs` | DI için arayüz + uygulama |
| `Program.cs` | Servis kaydı, **günlükleyen middleware**, route kısıtı |

### Denemeye değer adresler

| Adres | Ne gösteriyor |
|---|---|
| `/Kitap` | Varsayılan action + DI sayacı — **yenileyin, sayı artıyor** |
| `/Kitap/Detay/42` | Route'un üçüncü segmenti parametreye bağlandı |
| `/Kitap/Detay` | `{id?}` opsiyonel — `int?` olduğu için `null`, **0 değil** |
| `/Kitap/Detay/abc` | **404** — `{id:int?}` kısıtı yüzünden route eşleşmedi |
| `/Kitap/Ara?kelime=roman&sayfa=2` | Değerler sorgu dizesinden geldi |
| `/Kategori/Detay/99` | `NotFound()` → 404 |
| `/Kategori/Ara` | `RedirectToAction` → 302 ile `/Kategori`'ye |

---

## 🧪 Kendiniz deneyin — kodu kasten bozun

Bu haftanın konusu "sessizce yanlış çalışma". Aşağıdakileri **tek tek** deneyin;
her seferinde **önce tahmin edin**, sonra çalıştırın.

| Deneme | Nerede | Beklenen |
|---|---|---|
| `KitapController` → `Kitaplar` (soneki sil) | `Controllers/` | `/Kitap` **404** — derleme hatası yok |
| `public IActionResult Index()` → `private` | `KitapController` | **404** — private action route'lanmaz |
| `await next();` satırını yorum yap | `Program.cs` | **Bomboş sayfa, 200** — hata yok |
| `AddSingleton` → `AddScoped` | `Program.cs` | Sayaç her yenilemede **1** |
| `{id:int?}` → `{id?}` | `Program.cs` | `/Kitap/Detay/abc` artık 404 değil, **farklı bir hata** |
| `UseAuthorization()` satırını `UseRouting()` üstüne taşı | `Program.cs` | **Hiçbir şey olmaz** — ve tehlike tam olarak bu |

> Son satır bu haftanın en önemli dersidir: **uygulama çalışmaya devam eder,**
> ama Hafta 11'de yetkilendirme eklediğinizde koruma çalışmaz — ve yine hata almazsınız.

---

## 🏠 Ev görevi

1. Kendi projenize **iki controller** ekleyin, her birine **üçer action**
2. Şu beş adresin hangi controller/action'a düşeceğini **çalıştırmadan** yazın, sonra doğrulayın:
   `/` · `/Kitap` · `/Kitap/Detay/5` · `/Kategori/Ara` · `/Kitap/Detay/abc`
3. **Günlükleyen middleware'i** ekleyip terminal çıktısını inceleyin
4. **Teslim:** tahmin listeniz + gerçek sonuçlar + terminal çıktısının ekran görüntüsü

**İsteğe bağlı (yapana artı):** `IZiyaretSayaci`'yı kurup `AddSingleton` ↔ `AddScoped`
farkını ekran görüntüsüyle gösterin.

---

## 🔜 Hafta 3

Bu hafta action'lar `Content()` ile **düz metin** döndürdü. Hafta 3'te **HTML üretmeye** geçiyoruz:
Razor sözdizimi, Layout ve tekrar eden parçaları tek yerden yönetmek.

**Düşünerek gelin:** `Views/Shared/_Layout.cshtml` dosyasını açın. İçinde `@RenderBody()`
diye bir satır var. Sizce ne yapıyor?
