# Web Programlama

**2026 · Bahar Dönemi** · ASP.NET Core MVC (.NET 10 LTS)
Dr. Öğr. Üyesi Baran Kaynak

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

> Haftalar ders ilerledikçe eklenir. Aşağıdaki plan dönem başında duyurulmuştur.

**Planlanan konular:** 06 Model doğrulama · 07 State management ·
**ara sınav** · 08 EF Core ve Code First · 09–10 LINQ · 11 Authentication/Authorization ·
12 Web API ve MCP · 13 Uygulama geliştirme

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
| Veritabanı | Yok — Hafta 8'e kadar veri bellekte tutulur, sonra **SQLite** (kurulum gerektirmez) |

> 💡 Her hafta klasörü **kendi başına çalışır.** İstediğiniz haftadan başlayabilirsiniz.

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
| **Ara sınav** (%30) | Hafta 1–7 |
| **Final** (%40) | Hafta 8–13 ağırlıklı; Hafta 1–7 temel düzeyde |
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
