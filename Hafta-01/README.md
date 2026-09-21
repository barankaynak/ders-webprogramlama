# Hafta 1 — Web Programlama Temelleri

| | |
|---|---|
| 📄 **Ders notu** | [H01-Ders-Notu.pdf](../PDF/H01-Ders-Notu.pdf) — *sınav kapsamı* |
| 🔧 **Uygulama** | [H01-Uygulama.pdf](../PDF/H01-Uygulama.pdf) — derste yapılanların dökümü |
| 📚 **Detay** | [H01-Detay.pdf](../PDF/H01-Detay.pdf) — *sınav kapsamı DIŞI* |
| 🖱️ **Sunum** | [H01-etkilesimli.html](H01-etkilesimli.html) — indirip tarayıcıda açın |
| 🔧 **Kurulum** | [00-Kurulum-Rehberi.pdf](../PDF/00-Kurulum-Rehberi.pdf) — **bu haftanın ev görevi** |

---

## Bu haftanın konusu

- Statik site ↔ dinamik uygulama farkı
- HTTP'nin sunucu tarafı: metotlar, durum kodları, başlıklar
- **Stateless** (durumsuz) olmak ne demek
- Bir isteğin uçtan uca yolculuğu — ve bu dersin o zincirin neresinde durduğu
- .NET Framework ↔ .NET (Core), **LTS** kavramı, dokümantasyon sürümü tuzağı
- Proje iskeleti ve `Program.cs`'in dört evresi

🧩 **Haftanın ilkesi — Katmanlama.** Bir istek, her biri tek bir işten sorumlu katmanlardan
geçer. Bu dersin işi belirli bir katmandır; onu bilmek kadar **sınırını bilmek** de gerekir.

---

## Projeyi çalıştırma

```bash
cd KampusKitap
dotnet run
```

Terminalde yazan `https://localhost:7xxx` adresini tarayıcıda açın.
Durdurmak için <kbd>Ctrl</kbd>+<kbd>C</kbd>.

Geliştirirken kod değişikliklerinin anında yansıması için:

```bash
dotnet watch
```

### Bu klasörde ne var?

Ders notunda projeyi `IlkProje` adıyla oluşturduk — o, `dotnet new` komutunu öğrenmek için
kullanılan tek kullanımlık bir isimdi. Buradaki proje **dönem projesinin kendisidir**:
`KampusKitap`. Dönem boyunca her hafta bu uygulamaya bir katman ekleyeceğiz.

Şu an içeriği, `dotnet new mvc` şablonunun **Türkçeleştirilmiş** hâlinden ibarettir:

| Dosya | Ne yapar |
|---|---|
| `Program.cs` | Uygulamanın başladığı yer — dört evre buradadır |
| `Controllers/HomeController.cs` | İki action: `Index()` ve `Privacy()` |
| `Views/Home/Index.cshtml` | Ana sayfa |
| `Views/Shared/_Layout.cshtml` | Her sayfayı saran ortak iskelet |
| `wwwroot/` | **Dışarıya açık** klasör — CSS, JS, ikon |
| `appsettings.json` | Yapılandırma |

---

## 🏠 Ev görevi

1. [Kurulum rehberini](../PDF/00-Kurulum-Rehberi.pdf) izleyerek **.NET 10 SDK + bir IDE** kurun
2. `dotnet new mvc -n IlkProje` ile proje oluşturup `dotnet run` ile çalıştırın
3. **Teslim:** iki ekran görüntüsü
   - `dotnet --version` çıktısı (sürüm numarası görünmeli)
   - Tarayıcıda açılmış proje sayfası (adres çubuğunda `localhost` görünmeli)

> Bu görev **puan için değil, "kurulum tamam" onayı içindir.**
> Hafta 2'nin ev görevi bunun üzerine kurulur.

---

## 🔜 Hafta 2

`/Urun/Detay/42` adresine bir istek geldiğinde sunucu bunu hangi C# metoduna bağlıyor?
Ayrıca `Program.cs`'teki `app.Use…` satırları ne yapıyor?

**Düşünerek gelin:** Adres çubuğuna yalnızca `https://localhost:7xxx/` yazdınız,
hiçbir sayfa adı vermediniz. Sunucu hangi kodu çalıştıracağını nereden bildi?
