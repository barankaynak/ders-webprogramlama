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

## Projeyi çalıştırma

```bash
cd KampusKitap
dotnet run
```

### Bu hafta eklenenler

| Dosya | Ne için |
|---|---|
| `Views/Kitap/EkleTH.cshtml` | Hafta 4'teki formun **Tag Helper'lı hâli** — yan yana karşılaştırma |
| `Controllers/EtiketController.cs` | Haftanın bütün deneyleri |
| `Views/Etiket/Index.cshtml` | `asp-for` galerisi — her tip için **üretilen HTML** |
| `Views/Etiket/Baglanti.cshtml` | URL üretimi + 🕳️ elle yazılan `href` tuzağı |
| `Views/Etiket/Csrf.cshtml` | Üç form: TH+POST, TH+GET, elle POST |
| `Views/Etiket/Eksik.cshtml` | 🐛 `@addTagHelper` olmayınca ne oluyor |
| `ViewModels/EtiketVM.cs` | `asp-for` çıktısını göstermek için kurulmuş gösteri sınıfı |

> `Views/Kitap/Ekle.cshtml` (Hafta 4'ün elle yazılmış formu) **bilerek değiştirilmedi**.
> Haftanın anlatımı ikisini yan yana koymaya dayanıyor.

### Denemeye değer adresler

| Adres | Ne gösteriyor |
|---|---|
| `/Etiket` | `asp-for` her tip için **ne üretiyor** — tablo kendi kendini yazıyor |
| `/Etiket/Baglanti` | `asp-route-*` yola mı query string'e mi gidiyor |
| `/Etiket/Csrf` | Token'ı kim koyuyor, GET'te neden yok |
| `/Etiket/Eksik` | 🐛 `@addTagHelper` yoksa: **200 döner, hiçbir şey çalışmaz** |
| `/Kitap/Ekle` | Hafta 4 — **elle** yazılmış form |
| `/Kitap/EkleTH` | Aynı form, **Tag Helper** ile |

> 💡 Bu haftanın bütün sayfaları **Ctrl+U** ile okunmak üzere kuruldu.
> Anlatımın merkezi yazdığımız Razor değil, **tarayıcıya giden HTML**.

---

## 🧪 Kendiniz deneyin — kodu kasten bozun

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

1. Hafta 4'te **elle** yazdığınız formu, bu kez **Tag Helper'larla** yeniden yazın.
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
