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

> Haftalar ders ilerledikçe eklenir. Aşağıdaki plan dönem başında duyurulmuştur.

**Planlanan konular:** 05 Tag Helper'lar · 06 Model doğrulama · 07 State management ·
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

> 💡 Her hafta klasörü **kendi başına çalışır.** Hafta 5'ten başlamak isterseniz
> doğrudan o klasöre girip `dotnet run` diyebilirsiniz.

### Bir hafta çalışmazsa

```bash
dotnet --version          # 10.x olmalı
dotnet build              # hata mesajını okuyun
dotnet dev-certs https --trust   # "bağlantınız gizli değil" diyorsa
```

Hâlâ çalışmıyorsa o haftanın **ders notundaki 🐛 Hata Köşesi** tablosuna bakın;
en sık karşılaşılan hatalar ve çözümleri orada.

---

## Etkileşimli sunumlar

Her haftanın `H0N-etkilesimli.html` dosyası, derste yansıtılan sunumun kendisidir.
Tek dosyadır, **internetsiz çalışır** — indirip tarayıcıda açabilirsiniz.

**Hafta 3'ten itibaren** sunumlar sabit sahneli slayt biçimindedir ve akışları
**adım adım** çizer: <kbd>→</kbd> tuşu slayt değil **adım** ilerletir.

| Tuş | Hafta 1–2 | Hafta 3+ |
|---|---|---|
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
