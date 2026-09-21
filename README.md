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

> Haftalar ders ilerledikçe eklenir. Aşağıdaki plan dönem başında duyurulmuştur.

**Planlanan konular:** 02 MVC temelleri ve mimari · 03 Razor, View Component, Layout ·
04 Web formlar · 05 Tag Helper'lar · 06 Model doğrulama · 07 State management ·
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

| Tuş | İşlev |
|---|---|
| <kbd>→</kbd> <kbd>←</kbd> | Bölüm geçişi |
| <kbd>Home</kbd> / <kbd>End</kbd> | İlk / son bölüm |
| <kbd>D</kbd> | Koyu tema |
| <kbd>B</kbd> | Ekranı karart |
| Sol kenar numaraları | Bölüme atlama |

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
