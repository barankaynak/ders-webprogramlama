# Web Programlama

**2026 · Bahar Dönemi** · ASP.NET Core MVC (.NET 10 LTS)
Hazırlayan: **Dr. Öğr. Üyesi Baran Kaynak** (Bilişim Sistemleri Mühendisliği)  
Ortak yürütücüler: **Can Yüzkollar**, **Ahmet Şanslı** (Bilgisayar Mühendisliği)  
Sakarya Üniversitesi

Bu depo dersin **ders notlarını (PDF)**, **etkileşimli sunumlarını** ve
**hafta hafta büyüyen örnek projeyi** içerir.

---

## Buradan nasıl başlanır?

1. **[İzlenceyi okuyun](PDF/00-Ders-Izlencesi.pdf)** — dersin ne olduğu, nasıl işlendiği,
   haftalık plan ve değerlendirme.
2. **[Kurulum rehberini uygulayın](PDF/00-Kurulum-Rehberi.pdf)** — .NET 10 SDK + IDE.
   Bu, **Hafta 1'in ev görevidir** ve yapılmadan diğer haftalar takip edilemez.
3. Her hafta: **ders notunu derse gelmeden okuyun**, dersten sonra o haftanın
   projesini kendi makinenizde çalıştırın.

---

## Haftalar

| # | Konu | Ders notu | Uygulama | Detay | Proje |
|---|---|---|---|---|---|
| 01 | Web Programlama Temelleri | [PDF](PDF/H01-Ders-Notu.pdf) | [PDF](PDF/H01-Uygulama.pdf) | [PDF](PDF/H01-Detay.pdf) | [Hafta-01](Hafta-01/) |
| 02 | ASP.NET Core MVC Temelleri ve Mimari Yapısı | [PDF](PDF/H02-Ders-Notu.pdf) | [PDF](PDF/H02-Uygulama.pdf) | [PDF](PDF/H02-Detay.pdf) | [Hafta-02](Hafta-02/) |
| 03 | Razor Syntax, View Component, Layout | [PDF](PDF/H03-Ders-Notu.pdf) | [PDF](PDF/H03-Uygulama.pdf) | [PDF](PDF/H03-Detay.pdf) | [Hafta-03](Hafta-03/) |
| 04 | Web Formlar | [PDF](PDF/H04-Ders-Notu.pdf) | [PDF](PDF/H04-Uygulama.pdf) | [PDF](PDF/H04-Detay.pdf) | [Hafta-04](Hafta-04/) |
| 05 | Tag Helper’lar | [PDF](PDF/H05-Ders-Notu.pdf) | [PDF](PDF/H05-Uygulama.pdf) | [PDF](PDF/H05-Detay.pdf) | [Başlangıç](Hafta-05/Baslangic/) · [Bitmiş](Hafta-05/Bitmis/) |
| 06 | Model Doğrulama | [PDF](PDF/H06-Ders-Notu.pdf) | [PDF](PDF/H06-Uygulama.pdf) | [PDF](PDF/H06-Detay.pdf) | [Başlangıç](Hafta-06/Baslangic/) · [Bitmiş](Hafta-06/Bitmis/) |
| 07 | State Management: Cookie ve Session | [PDF](PDF/H07-Ders-Notu.pdf) | [PDF](PDF/H07-Uygulama.pdf) | [PDF](PDF/H07-Detay.pdf) | [Başlangıç](Hafta-07/Baslangic/) · [Bitmiş](Hafta-07/Bitmis/) |
| 08 | 🔶 **Ara sınav — ders yok** | | | | |
| 09 | LINQ: Koleksiyonları Sorgulamak | [PDF](PDF/H09-Ders-Notu.pdf) | [PDF](PDF/H09-Uygulama.pdf) | [PDF](PDF/H09-Detay.pdf) | [Başlangıç](Hafta-09/Baslangic/) · [Bitmiş](Hafta-09/Bitmis/) |
| 10 | ORM ve EF Core: Veriyi Kalıcı Yapmak | [PDF](PDF/H10-Ders-Notu.pdf) | [PDF](PDF/H10-Uygulama.pdf) | [PDF](PDF/H10-Detay.pdf) | [Başlangıç](Hafta-10/Baslangic/) · [Bitmiş](Hafta-10/Bitmis/) |
| 11 | EF Core ile LINQ: İlişkiler ve Veritabanında Sorgu | [PDF](PDF/H11-Ders-Notu.pdf) | [PDF](PDF/H11-Uygulama.pdf) | [PDF](PDF/H11-Detay.pdf) | [Başlangıç](Hafta-11/Baslangic/) · [Bitmiş](Hafta-11/Bitmis/) |

> Haftalar ders ilerledikçe eklenir. Aşağıdaki plan dönem başında duyurulmuştur.

**Planlanan konular:**
12 Authentication/Authorization · 13 Web API ve MCP · 14 Uygulama geliştirme

> Hafta numaraları takvim haftasıdır: Hafta 8 ara sınav haftasıydı. Sıra da değişti:
> önce LINQ (Hafta 9), sonra ORM ve EF Core (Hafta 10), sonra ikisi birlikte (Hafta 11).
> Hafta 3–7 notlarında EF Core "Hafta 8" ya da "ara sınavdan sonraki ilk ders" diye geçer;
> o konu Hafta 10'dadır.

---

## Örnek proje: KampusKitap

Dönem boyunca **tek bir uygulama** büyür: kampüs içi ikinci el kitap platformu.
Her `Hafta-NN/` klasörü, o haftanın sonundaki **anlık görüntüdür**.

```bash
cd Hafta-01/KampusKitap
dotnet run
```

Ardından terminalde yazan `https://localhost:7xxx` adresini açın.

| İhtiyacınız olan | Sürüm |
|---|---|
| .NET SDK | **10.0** veya üzeri (`dotnet --version` ile kontrol edin) |
| Veritabanı | Yok — Hafta 9'a kadar veri bellekte tutulur; Hafta 10'dan itibaren **SQLite** (kurulum gerektirmez) |

> 💡 Her hafta klasörü **kendi başına çalışır.** İstediğiniz haftadan başlayabilirsiniz.
> Hafta 10'dan itibaren tek bir ön adım var: veritabanını kurmak. `Bitmis/` klasöründe bir kez
> `Update-Database` (Visual Studio, Package Manager Console) ya da `dotnet ef database update`.

### Hafta 5'ten itibaren: Başlangıç ve Bitmiş

Hafta 5'ten itibaren her haftanın projesi **iki klasörde** gelir:

| Klasör | Ne | Port |
|---|---|---|
| **`Hafta-NN/Baslangic/`** | **Buradan başlayın.** Haftanın konusu olan yerler boş ve `TODO N.x` ile işaretli. Derlenir, çalışır; boşlukları doldurdukça değişikliği görürsünüz. | 5072 |
| **`Hafta-NN/Bitmis/`** | Aynı projenin tamamlanmış hâli. Her doldurulmuş yerin üstünde `✔ TODO N.x` izi var. | 5073 |

```bash
cd Hafta-05/Baslangic
dotnet run
```

Her haftanın `README.md` dosyasında **doldurulacak yerlerin tablosu** vardır.
Takıldığınızda Bitmiş'te aynı `TODO` numarasını aratın — **ama önce kendiniz deneyin.**

> Hafta 1 – 4 tek klasördür (`Hafta-NN/KampusKitap/`).

### Bir hafta çalışmazsa

```bash
dotnet --version          # 10.x olmalı
dotnet build              # hata mesajını okuyun
dotnet dev-certs https --trust   # "bağlantınız gizli değil" diyorsa
```

Hafta 10'dan itibaren (veritabanı):

| Görülen | Yapılacak |
|---|---|
| `no such table: …` | Migration'lar uygulanmamış: `Update-Database` ya da `dotnet ef database update` |
| `dotnet ef` bulunamıyor | `dotnet tool install --global dotnet-ef --version 10.0.15` |
| `Unable to retrieve project metadata` | Proje hiç derlenmemiş: önce `dotnet build` |
| Paket kurulmuyor: `NU1202 … not compatible with net10.0` | Sürüm verin: `--version 10.0.15` (sürümsüz kurulum EF Core 11'i seçer) |

Hâlâ çalışmıyorsa o haftanın **ders notundaki 🐛 Hata Köşesi** tablosuna bakın;
en sık karşılaşılan hatalar ve çözümleri orada.

---

## Derste yansıtılan sunumlar

Sunum dosyaları **bu depoda değildir.** Burası dersin **örnek projesi** ve
**PDF'leri** için; sunumlar ayrı tutuluyor ve sizinle **ders kanalından**
paylaşılıyor.

Derste anlatılan her şeyin yazılı karşılığı **ders notu PDF'indedir** — sınav
kapsamı da odur. Sunum, o metnin görsel anlatım aracıdır.

---|---|---|
| <kbd>→</kbd> <kbd>←</kbd> | bölüm geçişi | **adım** geçişi (adımlar bitince slayt) |
| <kbd>Home</kbd> / <kbd>End</kbd> | ilk / son bölüm | ilk / son slayt |
| <kbd>O</kbd> | — | tüm slaytlar (kuş bakışı) |
| <kbd>F</kbd> | — | tam ekran |
| <kbd>B</kbd> | ekranı karart | ekranı karart |
| <kbd>D</kbd> | koyu tema | — |
| <kbd>?</kbd> | — | bütün kısayollar |
| sayı + <kbd>Enter</kbd> | — | o slayda atla |

---

## Sınav kapsamı

| Sınav | Kapsam |
|---|---|
| **Ara sınav** (%30) | Hafta 1–6 |
| **Final** (%40) | Hafta 7 ve 9–14 ağırlıklı; Hafta 1–6 temel düzeyde |
| **Dönem projesi** (%30) | Kod + video anlatım, teslim son hafta |

> ⚠️ **Sınav kapsamı ders notlarıdır.** `H0N-Detay.pdf` dosyaları **kapsam dışıdır** ve
> bunu dosyanın başında açıkça yazar — onlar meraklı öğrenci içindir.

---

## Kaynak

Birincil kaynak Microsoft resmî dokümantasyonudur:
<https://learn.microsoft.com/aspnet/core>

> ⚠️ Adresin sonuna **`?view=aspnetcore-10.0`** ekleyin. Arama motorundan gelen sonuç
> çoğu zaman eski sürüme düşer. **Örnekte `Startup.cs` görüyorsanız yanlış sürümdesiniz** —
> o dosya .NET 6'dan beri yok.

Her ders notunun sonunda o haftanın **kaynakçası** vardır: hangi kaynağın hangi bölümü
dayandırdığı tek tek yazılıdır.

---

*Bu depo ders materyali dağıtımı içindir. Sorular ve geri bildirim için ders saatlerini
veya kurumsal e-posta adresini kullanın.*
