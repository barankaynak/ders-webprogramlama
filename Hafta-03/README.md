# Hafta 3 — Razor Syntax, View Component, Layout

| | |
|---|---|
| 📄 **Ders notu** | [H03-Ders-Notu.pdf](../PDF/H03-Ders-Notu.pdf) — *sınav kapsamı* |
| 🔧 **Uygulama** | [H03-Uygulama.pdf](../PDF/H03-Uygulama.pdf) — derste yapılanların dökümü |
| 📚 **Detay** | [H03-Detay.pdf](../PDF/H03-Detay.pdf) — *sınav kapsamı DIŞI* |
| 🖱️ **Sunum** | [H03-etkilesimli.html](H03-etkilesimli.html) — indirip tarayıcıda açın |

> 🆕 **Sunum biçimi değişti.** Bu haftadan itibaren sunum, akışları **adım adım
> çizen** slayt biçimindedir. <kbd>→</kbd> tuşu slayt değil **adım** ilerletir;
> alt ortadaki noktalar o slaytta kaç adım kaldığını gösterir.
> <kbd>O</kbd> tüm slaytları listeler, <kbd>?</kbd> bütün kısayolları gösterir.

---

## Bu haftanın konusu

- **Razor sözdizimi** — `@` ifadeleri, `@{ }` blokları, `@if`/`@switch`/`@for`/`@foreach`/`@while`/`@try`, `@@` kaçışı
- **Parantez ne zaman zorunlu** — boşluk ve generic; ve ne zaman **gereksiz**
- **Öznitelik üretimi** — `null` değerli öznitelik hiç yazılmaz; `bool` özniteliklerin davranışı
- **Razor yönergeleri** — `@model`, `@using`, `@addTagHelper`, `@inject`, `@section`, `@functions`
- **`@model`** ve güçlü tipli (strongly typed) görünüm
- **Otomatik HTML kodlaması** ve `@Html.Raw` — XSS'e açılan kapı
- **Layout** — `@RenderBody()`, `@section` / `@RenderSection`, `_ViewStart`, `_ViewImports`
- **Görünüm arama sırası** — `Views/{Controller}/` → `Views/Shared/`
- **Partial View** ve **View Component** — ve aralarındaki tek cümlelik fark

🧩 **Haftanın ilkesi — Tek doğruluk kaynağı.** Aynı arayüz parçası iki yerde
duruyorsa, er ya da geç ikisi farklılaşır. Üçüncü kopyada bunu fark etmek aylar alır.

> **Partial veriyi *alır*. View Component veriyi *kendi getirir*.**
> Sınavda bu ayrımı sorulduğunda kendinize şunu sorun: *"Bu parçayı yeni bir sayfaya
> koyduğumda o sayfanın controller'ında bir şey yazmam gerekiyor mu?"*

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
| `Models/Kitap.cs` + `Data/KitapDeposu.cs` | Görünümlere taşınacak veri (bellekte) |
| `Views/Kitap/*.cshtml` | Güçlü tipli görünümler, `@section`, açık ifade, HTML kodlaması |
| `Views/Kitap/Sozdizimi.cshtml` | **Sözdizimi kılavuzu** — 10 yapı, çalışır hâlde |
| `Views/Shared/_KitapKart.cshtml` | **Partial View** — veriyi dışarıdan alır |
| `ViewComponents/ZiyaretRozetiViewComponent.cs` | **View Component** — veriyi kendi getirir |
| `Views/Shared/Components/ZiyaretRozeti/Default.cshtml` | View Component'in görünümü (**yol sözleşmedir**) |
| `Views/Shared/_Layout.cshtml` | Menü, `@RenderBody()`, `@RenderSectionAsync`, rozet |
| `Views/_ViewImports.cshtml` | `@addTagHelper *, KampusKitap` — `<vc:…>` bunsuz çalışmaz |

### Denemeye değer adresler

| Adres | Ne gösteriyor |
|---|---|
| `/Kitap` | Güçlü tipli görünüm + partial · **Ctrl+U ile kaynağa bakın** |
| `/Kitap/Detay/42` | `@model`, açık ifade `@(...)`, HTML kodlaması |
| `/Kitap/Detay` | `View("Secilmedi")` — görünüm adı metot adından farklı |
| `/Kitap/Detay/999` | `NotFound()` → 404 |
| `/Kitap/Ara?kelime=a` | **Aynı partial**, ikinci sayfada |
| `/Kategori` | `@section Scripts` **yok** — `required: false` sayesinde sorun yok |
| `/Kitap/Sozdizimi` | **Razor sözdizimi kılavuzu** — 10 yapı ve ürettikleri HTML yan yana |
| `/Kitap/Tuzak` | 🕳️ Görünümün içinde sorgu — çalışır ama yanlış |

> Sayfanın altındaki **"bu sayfa N kez gösterildi"** rozeti bir View Component'tir.
> Üç sayfada da görünüyor ve bunun için **hiçbir controller** tek satır yazmadı.

---

## 🧪 Kendiniz deneyin — kodu kasten bozun

Aşağıdakileri **tek tek** deneyin; her seferinde **önce tahmin edin**, sonra çalıştırın.

| Deneme | Nerede | Beklenen |
|---|---|---|
| `@RenderBody()` satırını yorum yap | `_Layout.cshtml` | İçerik gider, menü kalır — **hata yok** |
| `_ViewStart.cshtml`'i yorum yap | `Views/` | Menü, CSS, footer gider — çıplak HTML |
| `model="k"` ifadesini sil | `Views/Kitap/Index.cshtml` | Tip uyuşmazlığı: `List<Kitap>` geldi, `Kitap` bekleniyordu |
| `Index.cshtml`'i sil | `Views/Kitap/` | **Aranan yolları listeleyen** istisna — listeyi okuyun |
| `required: false` → `true` | `_Layout.cshtml` | `/Kategori` **hata verir**, `/Kitap` çalışır |
| `@section Scripts` → `@section scripts` | `Views/Kitap/Index.cshtml` | "defined but have not been rendered" — ad **büyük/küçük harfe duyarlı** |
| `Default.cshtml`'i `Components/` dışına taşı | `Views/Shared/` | View Component görünümü bulunamaz |
| `@addTagHelper *, KampusKitap` satırını sil | `_ViewImports.cshtml` | `<vc:ziyaret-rozeti />` ekranda **metin olarak** çıkar |
| `Detay.cshtml`'de `@Html.Raw` satırının yorumunu aç | `Views/Kitap/` | **Uyarı kutusu açılır** — XSS'in ta kendisi |

> Son satır bu haftanın güvenlik dersidir: `@Html.Raw` yazarken
> *"bu HTML'i ben ürettim, kullanıcıdan gelmedi"* demiş olursunuz.

---

## 🏠 Ev görevi

1. Kendi projenizde **Layout** kurun; üst menüyü layout'a taşıyın. En az **üç** sayfada aynı menünün göründüğünü ekran görüntüsüyle gösterin.
2. Menüyü ayrıca `_Menu.cshtml` adlı bir **partial**'a çıkarın ve layout'tan çağırın.
3. Tekrar eden bir **bilgi kartını** partial'a çıkarın (`model="…"` ile veri geçirin).
4. **Bir View Component yazın**: kendi verisini getiren bir parça (örn. "toplam kayıt sayısı" rozeti). Hiçbir controller'a dokunmadan **iki farklı sayfada** göründüğünü kanıtlayın.
5. `@RenderBody()` satırını geçici olarak silin, ekran görüntüsünü alın, geri koyun.
6. Bir alana `<b>kalın</b>` metnini **veri olarak** verin; `@Model.X` ile `@Html.Raw(Model.X)` çıktılarının **farkını** ekran görüntüsüyle gösterin.

**Teslim:** proje klasörü + 6 maddenin ekran görüntüleri + şu soruya **iki cümlelik**
yazılı cevap: *"4. maddede neden partial değil View Component kullandınız?"*

---

## 🔜 Hafta 4

Bu hafta **tek yön** vardı: sunucudan kullanıcıya. Haftaya **ters yöne** geçiyoruz —
**Web Formlar**: formu göster → kullanıcı doldursun → gönder → sunucu C# nesnesine çevirsin.

**Düşünerek gelin:** Hafta 1'de bir HTTP isteğinin **gövdesi (body)** olduğunu görmüştük.
Bir form "Gönder" dendiğinde tarayıcı gövdeye tam olarak ne koyar?
Ve sunucu o metni nasıl olup da bir `Kitap` nesnesine çeviriyor?
