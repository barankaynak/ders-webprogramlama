# Hafta 6 — Model Doğrulama

| | |
|---|---|
| 📄 **Ders notu** | [H06-Ders-Notu.pdf](../PDF/H06-Ders-Notu.pdf) — *sınav kapsamı* |
| 🔧 **Uygulama** | [H06-Uygulama.pdf](../PDF/H06-Uygulama.pdf) — derste yapılanların dökümü |
| 📚 **Detay** | [H06-Detay.pdf](../PDF/H06-Detay.pdf) — *sınav kapsamı DIŞI* |

---

## Bu haftanın konusu

- **Doğrulama nerede çalışır** — Model Binding → doğrulama → action; `ModelState`'in içi
- **Örtük doğrulama** — attribute yazmadan gelen hatalar: tip, nullable işareti, Model Binding
- **DataAnnotations** — `[Required]`, `[StringLength]`, `[Range]`, `[EmailAddress]`, `[Compare]`, `[RegularExpression]`, `[Url]`; `ErrorMessage` ile Türkçe mesaj
- **`ModelState` ve sunucu tarafı doğrulama** — kanonik action kalıbı, `AddModelError`, `asp-validation-for`, `asp-validation-summary`
- **İstemci doğrulaması** — `data-val-*` + jQuery Validation; atlatılabilir olduğu ve Türkçe ondalık ayracı
- **Özel doğrulama** — kendi `ValidationAttribute`'ünüz ve `IValidatableObject`; veri doğrulama ile iş kuralı ayrımı

🧩 **Haftanın ilkesi — Güvenme, doğrula; ve doğrulamayı denetlediğin tarafta yap.**
Tarayıcıdaki doğrulama kullanıcıya yardım eder, ama tarayıcı **sizin** değildir.
Kural sunucuda, modelde yazılır; istemci yalnızca aynı kuralın erken uyarısıdır.

> **Haftanın tek cümlelik özeti:** Hafta 5'te `data-val-*` attribute'leri HTML'e
> yazılıyordu ve onları okuyan hiçbir şey yoktu. Bu hafta hem onları okuyan betiği
> hem de — asıl önemlisi — **sunucunun** kendi denetimini kuruyoruz.

---

## İki proje: Başlangıç ve Bitmiş

| Klasör | Ne | Port |
|---|---|---|
| **`Baslangic/`** | **Buradan başlayın.** Haftanın konusu olan yerler boş bırakıldı ve **`TODO 6.x`** ile işaretlendi. Proje derlenir ve çalışır; boşlukları doldurdukça formun davranışı değişir. | `5072` |
| **`Bitmis/`** | Aynı projenin tamamlanmış hâli. Doldurulmuş her yerin üstünde **`✔ TODO 6.x`** izi var — takıldığınızda ya da bitirince karşılaştırmak için. | `5073` |

```bash
cd Baslangic        # önce bu
dotnet run          # → http://localhost:5072

cd Bitmis           # karşılaştırmak için, ayrı bir terminalde
dotnet run          # → http://localhost:5073
```

> ⚠️ **Önce Bitmiş'e bakmayın.** Boşluğu kendiniz doldurduğunuzda neyin değiştiğini
> görürsünüz; hazır çözümü okuduğunuzda yalnızca sonucu görürsünüz.

### Doldurulacak yerler

Editörde **`TODO 6.`** diye aratın. Her görevden sonra `/Kitap/EkleTH` formunu
**boş** ve **hatalı** gönderip ne değiştiğine bakın.

<!-- GOREVLER-BAS -->
| Görev | Ne yapılacak | Dosya | Doldurunca ne görülür |
|---|---|---|---|
| **TODO 6.1** | Başlık ve Yazar için doğrulama attribute'lerini ekleyin: `[Required]` ve `[StringLength]`, Türkçe `ErrorMessage` ile | `ViewModels/KitapEkleVM.cs` | Hafta 6 ders notu bölüm 2.2 — `{0}` alanın adı, `{1}` üst sınır, `{2}` alt sınırdır. Boş gönderin; `/Kitap/EkleTH`'deki hata listesinde mesajınız görünmeli |
| **TODO 6.2** | Fiyat ve kategori için `[Required]` ve `[Range]` ekleyin | `ViewModels/KitapEkleVM.cs` | Hafta 6 ders notu bölüm 2.2 — `decimal?` için `[Range(1.0, 10000.0, …)]`; kategori 1, 2 ya da 3 olmalı. TODO'dan önce boş fiyatla gönderin: 0 TL'lik kitap eklenir |
| **TODO 6.3** | Hataları gösterin: formun üstüne `asp-validation-summary`, her alanın altına `asp-validation-for` | `Views/Kitap/EkleTH.cshtml` | Hafta 6 ders notu bölüm 3.3 — elle yazılmış ModelState döngüsünün yerini iki Tag Helper alır. `ModelOnly` yalnızca alana bağlı olmayan hataları listeler |
| **TODO 6.4** | İş kuralı: aynı başlıkta bir kitap varsa `ModelState.AddModelError` ile hata ekleyin | `Controllers/KitapController.cs` | Hafta 6 ders notu bölüm 3.4 — anahtar `nameof(KitapEkleVM.Baslik)` olursa hata Başlık alanının altına düşer. "Suç ve Ceza" ekleyerek deneyin |
| **TODO 6.5** | İstemci doğrulamasını açın: `_ValidationScriptsPartial`'ı sayfaya ekleyin | `Views/Kitap/EkleTH.cshtml` | Hafta 6 ders notu bölüm 4.2 — tek satır. Sonra formu boş gönderin: istek sunucuya GİTMEDEN hata görünmeli (F12 → Network boş kalır) |
| **TODO 6.6** | `IsValid` gövdesini yazın: metin büyük harfle başlamıyorsa `false` | `Validation/BuyukHarfleBaslarAttribute.cs` | Hafta 6 ders notu bölüm 5.2 — boş değer bu attribute'ün işi DEĞİL (`[Required]`'ın işi), boşsa `true` dönün. `char.IsUpper` |
| **TODO 6.7** | Başlık ile yazar aynı olamaz: `Validate` gövdesini yazın | `ViewModels/KitapEkleVM.cs` | Hafta 6 ders notu bölüm 5.3 — `yield return new ValidationResult("…", [nameof(Baslik), nameof(Yazar)])`. Büyük-küçük harf farkını yok sayın |
<!-- GOREVLER-SON -->

### Bu hafta eklenenler

| Dosya | Ne için |
|---|---|
| `ViewModels/KitapEkleVM.cs` | Kurallar burada — TODO 6.1, 6.2, 6.7. `Fiyat` ve `KategoriId` artık **nullable** (bölüm 2.3) |
| `Validation/BuyukHarfleBaslarAttribute.cs` | Projenin **kendi** doğrulama attribute'ü — TODO 6.6 |
| `Views/Kitap/EkleTH.cshtml` | Hataları gösteren form — TODO 6.3, 6.5 |
| `Controllers/KitapController.cs` | İş kuralı: aynı başlıkta kitap — TODO 6.4 |
| `Controllers/DogrulaController.cs`, `Views/Dogrula/` | Haftanın ölçüm sayfaları — **iki projede de tam** |
| `Views/Shared/_ModelStateTablosu.cshtml` | `ModelState`'in içini tablo hâlinde basan partial |
| `wwwroot/js/dogrulama-tr.js` | jQuery Validation'a Türkçe sayı biçimini öğretir: `120,50` |
| `Program.cs` | Kültür `tr-TR`'ye sabitlendi — sunucu her makinede aynı sayıyı okur |

### Denemeye değer adresler

| Adres | Ne gösteriyor |
|---|---|
| `/Dogrula` | Hazır attribute'ler, varsayılan mesajlar, `AddModelError`, iki özet türü |
| `/Dogrula?istemci=true` | Aynı form, istemci doğrulaması açık |
| `/Dogrula?istemci=true&ham=true` | Türkçe sayı düzeltmesi **olmadan**: `1,75` reddedilir |
| `/Dogrula/Ortuk` | Hiç attribute yokken gelen hatalar — ve "yok" ile "boş" farkı |
| `/Dogrula/Tuzak` | 🐛 `ModelState.IsValid` neden hep `false` |
| `/Dogrula/Sira` | `IValidatableObject` ne zaman çalışır |
| `/Kitap/EkleTH` | Haftanın formu — Başlangıç'ta kuralsız, doldurdukça doğrulanan |

---

## 🧪 Kendiniz deneyin — kodu kasten bozun

Bunları **Bitmiş** projede deneyin.

| Deneme | Nerede | Beklenen |
|---|---|---|
| `if (!ModelState.IsValid) return View(girdi);` satırını silin, formu boş gönderin | `KitapController.EkleTH` | Başlığı **boş**, fiyatı **0 TL** bir kitap eklenir — kurallar çalıştı, kimse bakmadı |
| Konsola `document.forms[0].submit()` yazın | `/Kitap/EkleTH` | Tarayıcı ve jQuery atlanır; **sunucu** yine bütün hataları döndürür |
| `Fiyat` tipini `decimal` yapın, boş gönderin | `KitapEkleVM.cs` | Sizin mesajınız değil, **"The value '' is invalid."** gelir |
| İki e-posta alanına da `xyz` yazıp gönderin | `/Dogrula/Ortuk` | `[DataType(EmailAddress)]` olan **geçer**, `[EmailAddress]` olan düşer — `[DataType]` doğrulamaz |
| `_ValidationScriptsPartial`'daki `dogrulama-tr.js` satırını silin | `Views/Shared` | `120,50` tarayıcıda **reddedilir** |

---

## 🏠 Ev görevi

0. **Başlangıç** projesinde **TODO 6.1 – 6.7**'yi tamamlayın.
1. Kendi projenizdeki kayıt formunun ViewModel'ine **en az beş** doğrulama kuralı
   yazın; her birine Türkçe `ErrorMessage` verin.
2. POST action'ınızı kanonik kalıpla yazın: geçersizse formu hatalarla geri gösterin,
   geçerliyse kaydedip yönlendirin.
3. Formda her alanın altına `asp-validation-for`, üstüne `asp-validation-summary`
   koyun ve `_ValidationScriptsPartial`'ı ekleyin.
4. Sunucunun gerçekten denetlediğini **kanıtlayın**: istemci doğrulaması açıkken
   konsoldan `document.forms[0].submit()` ile boş formu gönderin ve sunucunun
   döndürdüğü hataların ekran görüntüsünü alın.
5. Birden çok alana bakan bir kural için `IValidatableObject` yazın.

**Teslim:** proje klasörü + 4. maddenin ekran görüntüsü + şu soruya **iki cümlelik**
cevap: *"İstemci doğrulaması varken sunucu doğrulamasına neden hâlâ ihtiyaç var?"*

---

## 🔜 Hafta 7 — State Management

Bu hafta her isteği **tek başına** doğruladık. Ama HTTP her isteği unutur: kullanıcı
sayfayı yenilediğinde ya da başka bir sayfaya geçtiğinde neyin "kendisine ait"
olduğunu nereden bileceğiz?

**Düşünerek gelin:** Sepetinize eklediğiniz ürün, ikinci sayfaya geçtiğinizde nerede
duruyor — sunucuda mı, tarayıcıda mı? İkisinin farkı güvenlik açısından ne?
