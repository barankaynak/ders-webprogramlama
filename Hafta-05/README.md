# Hafta 5 — Tag Helper'lar

| | |
|---|---|
| 📄 **Ders notu** | [H05-Ders-Notu.pdf](../PDF/H05-Ders-Notu.pdf) — *sınav kapsamı* |
| 🔧 **Uygulama** | [H05-Uygulama.pdf](../PDF/H05-Uygulama.pdf) — derste yapılanların dökümü |
| 📚 **Detay** | [H05-Detay.pdf](../PDF/H05-Detay.pdf) — *sınav kapsamı DIŞI* |

---

## Bu haftanın konusu

- **Tag Helper nedir** — HTML gibi görünen C#; `@Html` yardımcılarından farkı
- **`@addTagHelper`** ve `_ViewImports.cshtml`
- **URL üreten Tag Helper'lar** — `asp-controller`, `asp-action`, `asp-route-*`
- **`asp-for`** — `name`, `id`, `type`, `value` üretimi
- **`<label asp-for>`, `<textarea asp-for>`, `<select asp-items>`**
- **CSRF** ve Tag Helper'ın sessiz katkısı: `__RequestVerificationToken`
- **`[ValidateAntiForgeryToken]`** — üretmek yetmez, doğrulamayı da istemek gerekir

🧩 **Haftanın ilkesi — Çerçeve, doğru olanı kolay yapar.**
Tag Helper bir kısaltma değildir. Yazmadığınız `name`'i, yazmadığınız `action`'ı ve
**istemediğiniz güvenlik alanını** sizin yerinize üretir. Kolay olan yol, artık
**doğru** olan yoldur.

> **Haftanın tek cümlelik özeti:** Hafta 4'te hata **çalışma anında sessizdi**;
> bu hafta aynı hata **derlemede** yakalanıyor.

---

## İki proje: Başlangıç ve Bitmiş

Bu haftadan itibaren örnek proje **iki hâlde** gelir:

| Klasör | Ne | Port |
|---|---|---|
| **`Baslangic/`** | **Buradan başlayın.** Haftanın konusu olan yerler boş bırakıldı ve **`TODO 5.x`** ile işaretlendi. Proje derlenir ve çalışır; boşlukları doldurdukça değişikliği ekranda ya da **Ctrl+U**'da görürsünüz. | `5072` |
| **`Bitmis/`** | Aynı projenin tamamlanmış hâli. Doldurulmuş her yerin üstünde **`✔ TODO 5.x`** izi var — takıldığınızda ya da bitirince karşılaştırmak için. | `5073` |

```bash
cd Baslangic        # önce bu
dotnet run          # → http://localhost:5072

cd Bitmis           # karşılaştırmak için, ayrı bir terminalde
dotnet run          # → http://localhost:5073
```

> 💡 İkisi **farklı portta** çalışır; yan yana iki sekmede açıp aynı sayfayı
> karşılaştırabilirsiniz. En öğretici karşılaştırma **Ctrl+U** ile yapılır.

> ⚠️ **Önce Bitmiş'e bakmayın.** Boşluğu kendiniz doldurduğunuzda neyin değiştiğini
> görürsünüz; hazır çözümü okuduğunuzda yalnızca sonucu görürsünüz.

### Doldurulacak yerler

Editörde **`TODO 5.`** diye aratın. Sırayla gidin — bazı görevler bir öncekine dayanıyor.

<!-- GOREVLER-BAS -->
| Görev | Ne yapılacak | Dosya | Doldurunca ne görülür |
|---|---|---|---|
| **TODO 5.1** | Menüye "Tag Helper'lar" bağlantısı ekleyin: `asp-controller="Etiket"` `asp-action="Index"` | `Views/Shared/_Layout.cshtml` | ders notu bölüm 2.1 — üstteki satırları örnek alın. Ctrl+U'da `href="/Etiket"` görünmeli, `asp-` görünmemeli |
| **TODO 5.2** | Form etiketini Tag Helper'a çevirin: `asp-controller` ve `asp-action` | `Views/Kitap/EkleTH.cshtml` | ders notu bölüm 2.4 — Ctrl+U: `action` adresi üretildi ve formun sonunda `__RequestVerificationToken` belirdi |
| **TODO 5.3** | Başlık alanını Tag Helper'a çevirin: `<label asp-for>` ve `<input asp-for>` | `Views/Kitap/EkleTH.cshtml` | ders notu bölüm 3.1 ve 3.3 — Ctrl+U: `name`, `id` ve `type`'ı siz yazmadınız. Etiket metni "Baslik" oldu (TODO 5.5'i bekleyin) |
| **TODO 5.4** | Yazar ve Fiyat alanları için de aynısını yapın | `Views/Kitap/EkleTH.cshtml` | Fiyat için Ctrl+U'da `type`'a bakın — neden `number` değil de `text`? (ders notu bölüm 3.2) |
| **TODO 5.5** | Dört özelliğe `[Display(Name = "…")]` ekleyin: Başlık, Yazar, Fiyat (TL), Kategori | `ViewModels/KitapEkleVM.cs` | ders notu bölüm 3.3 — `/Kitap/EkleTH`'deki etiketler "Baslik", "KategoriId" yerine Türkçe adları gösterir |
| **TODO 5.6** | Kategori listesini `<select asp-for asp-items>` yapın; kaynak: `KitapDeposu.Kategoriler` | `Views/Kitap/EkleTH.cshtml` | ders notu bölüm 3.4 — "— seçiniz —" satırını siz bırakın, Tag Helper diğerlerini arkasına ekler |
| **TODO 5.7** | Bu POST action'ının anti-forgery token'ını doğrulatın | `Controllers/KitapController.cs` | ders notu bölüm 4.3 — tek satırlık bir attribute. Önce TODO 5.2'yi bitirin; sırayı ters yaparsanız ne olur? (Uygulama C) |
| **TODO 5.8** | (İLERİ) `Process` gövdesini yazın: `<fiyat>` etiketini `<span class="fiyat">120,50 ₺</span>` yapın | `TagHelpers/FiyatTagHelper.cs` | H05-detay.md bölüm 1 — `output.TagName`, `TagMode`, `Attributes.SetAttribute`, `Content.SetContent`. `/Etiket` sayfasının en altındaki satır dolar |
<!-- GOREVLER-SON -->

> **TODO 5.8 isteğe bağlıdır** (ileri düzey, sınav kapsamı dışı).

### Bu hafta eklenenler

| Dosya | Ne için |
|---|---|
| `Views/Kitap/EkleTH.cshtml` | Hafta 4'teki formun **Tag Helper'lı hâli** — Başlangıç'ta elle yazılmış hâlde durur, TODO 5.2 – 5.6 ile dönüştürülür |
| `Controllers/EtiketController.cs` | Haftanın bütün deneyleri — **iki projede de tam** |
| `Views/Etiket/Index.cshtml` | `asp-for` galerisi — her tip için **üretilen HTML** |
| `Views/Etiket/Baglanti.cshtml` | URL üretimi + 🕳️ elle yazılan `href` tuzağı |
| `Views/Etiket/Csrf.cshtml` | Üç form: TH+POST, TH+GET, elle POST |
| `Views/Etiket/Eksik.cshtml` | 🐛 `@addTagHelper` olmayınca ne oluyor |
| `ViewModels/EtiketVM.cs` | `asp-for` çıktısını göstermek için kurulmuş gösteri sınıfı |
| `TagHelpers/FiyatTagHelper.cs` | Projenin **kendi** Tag Helper'ı (TODO 5.8, ileri) |

> `Etiket` sayfaları **ölçüm aracıdır**, alıştırma değil — iki projede de eksiksizdir.
> `Views/Kitap/Ekle.cshtml` (Hafta 4'ün elle yazılmış formu) **bilerek değiştirilmedi**.

### Denemeye değer adresler

| Adres | Ne gösteriyor |
|---|---|
| `/Etiket` | `asp-for` her tip için **ne üretiyor** — tablo kendi kendini yazıyor |
| `/Etiket/Baglanti` | `asp-route-*` yola mı query string'e mi gidiyor |
| `/Etiket/Csrf` | Token'ı kim koyuyor, GET'te neden yok |
| `/Etiket/Eksik` | 🐛 `@addTagHelper` yoksa: **200 döner, hiçbir şey çalışmaz** |
| `/Kitap/Ekle` | Hafta 4 — **elle** yazılmış form |
| `/Kitap/EkleTH` | Aynı form — Başlangıç'ta elle, doldurdukça **Tag Helper** |

> 💡 Bu haftanın bütün sayfaları **Ctrl+U** ile okunmak üzere kuruldu.
> Anlatımın merkezi yazdığımız Razor değil, **tarayıcıya giden HTML**.

---

## 🧪 Kendiniz deneyin — kodu kasten bozun

Bunları **Bitmiş** projede deneyin — Başlangıç'taki boşlukları doldurduktan sonra
kendi projenizde de aynısını yapabilirsiniz.

| Deneme | Nerede | Beklenen |
|---|---|---|
| `asp-for="Baslik"` → `asp-for="Balsik"` | `Views/Kitap/EkleTH.cshtml` | **Derleme hatası** — Hafta 4'te sessizdi |
| `@model` satırını sil | aynı dosya | `error CS1963` — proje derlenmez |
| Rota şablonuna `site/` öneki ekle | `Program.cs` | TH bağlantıları **taşınır**, elle yazılan `href` **404** |
| `[ValidateAntiForgeryToken]`'ı sil | `KitapController.EkleTH` | Token **üretilir** ama **kontrol edilmez** |
| `<form asp-action>` → `<form action="/Kitap/EkleTH">` | `EkleTH.cshtml` | Token **üretilmez** → POST **400** |
| `method="post"` → `method="get"` | aynı form | Token **kaybolur** — GET'te üretilmiyor |
| `@addTagHelper` satırını sil | `Views/_ViewImports.cshtml` | Bütün `asp-*` **metin olarak** tarayıcıya gider |

> Son satır bu haftanın en sinsi hatasıdır: **sayfa açılır, hata yoktur, hiçbir şey çalışmaz.**
> Teşhis kuralı: kaynakta `asp-` görüyorsanız o Tag Helper çalışmamıştır.

---

## 🏠 Ev görevi

0. Derste **Başlangıç** projesindeki TODO 5.1 – 5.7'yi tamamlayın (5.8 isteğe bağlı).
1. Kendi projenizde, Hafta 4'te **elle** yazdığınız formu bu kez **Tag Helper'larla** yeniden yazın.
   Eski dosyayı **silmeyin**; ikisi yan yana dursun.
2. İki formun ürettiği HTML'i **Ctrl+U** ile alıp yan yana koyun. **Farkları işaretleyin.**
3. `name`, `id` ve `type` değerlerinin Tag Helper'lı sürümde **nereden geldiğini** yazın.
4. `[Display(Name = "…")]` ekleyip `<label asp-for>` çıktısının değiştiğini gösterin.
5. Bir `asp-for` değerini **bilerek yanlış** yazın. Derleyicinin ne dediğini ekran
   görüntüsüyle kaydedin ve Hafta 4'teki davranışla **karşılaştırın**.
6. POST action'ınıza `[ValidateAntiForgeryToken]` ekleyin. Sonra formu
   **elle yazılmış** hâline çevirip gönderin: aldığınız durum kodunu yazın.
7. `Program.cs`'teki rota şablonuna bir önek ekleyin. Hangi bağlantılar taşındı,
   hangileri kırıldı?

**Teslim:** proje klasörü + iki HTML çıktısının işaretlenmiş karşılaştırması +
5., 6. ve 7. maddelerin ekran görüntüleri + şu soruya **iki cümlelik** cevap:
*"Tag Helper bir kısaltma mı, yoksa başka bir şey mi?"*

---

## 🔜 Hafta 6

Bu hafta `data-val-*` attribute'lerinin HTML'e **kendiliğinden** yazıldığını gördük
ama onları okuyan hiçbir şey yoktu.

**Hafta 6**'da o attribute'leri okuyan betiği ve asıl önemlisi **sunucu tarafı
doğrulamayı** kuracağız.

**Düşünerek gelin:** Tarayıcıdaki doğrulama kullanıcıya yardım eder. Peki
kullanıcı tarayıcının doğrulamasını **atlatırsa**? Hafta 4'ün ilkesi hâlâ geçerli mi?
