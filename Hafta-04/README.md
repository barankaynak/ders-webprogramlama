# Hafta 4 — Web Formlar

| | |
|---|---|
| 📄 **Ders notu** | [H04-Ders-Notu.pdf](../PDF/H04-Ders-Notu.pdf) — *sınav kapsamı* |
| 🔧 **Uygulama** | [H04-Uygulama.pdf](../PDF/H04-Uygulama.pdf) — derste yapılanların dökümü |
| 📚 **Detay** | [H04-Detay.pdf](../PDF/H04-Detay.pdf) — *sınav kapsamı DIŞI* |
| 🖱️ **Sunum** | [H04-etkilesimli.html](H04-etkilesimli.html) — indirip tarayıcıda açın |

> 🌐 **Sunumda 3B bir sahne var.** Slayt 10, bağlama kaynaklarının öncelik sırasını
> üç boyutlu katman olarak gösterir (sürükleyerek çevirebilirsiniz). Sahne `three.js`
> kütüphanesini internetten çeker; **ağ yoksa ya da tarayıcınız WebGL desteklemiyorsa**
> aynı sırayı gösteren 2B şema otomatik devreye girer.

---

## Bu haftanın konusu

- **Formun sunucuya gönderdiği şey** — istek gövdesi, `Content-Type`, `name` özniteliği
- **GET/POST çifti** — `[HttpGet]` ve `[HttpPost]`
- **Model Binding** — metin değerlerin C# nesnesine dönüşmesi
- **Bağlama kaynakları ve öncelik sırası** — Form → Rota → Sorgu
- **`[FromQuery]` · `[FromRoute]` · `[FromForm]` · `[FromBody]`**
- **Bağlama başarısız olduğunda** — sessiz varsayılan değer ve `ModelState`
- **Post-Redirect-Get (PRG)** — F5 ile kaydın tekrar eklenmesi ve çözümü
- **Over-posting (kütle atama)** ve **ViewModel** çözümü

🧩 **Haftanın ilkesi — İstemciden gelen hiçbir veri güvenilir değildir.**
Formun HTML'ini siz yazarsınız; ama o HTML **kullanıcının makinesinde** çalışır ve
kullanıcı onu değiştirebilir. Sunucuya ulaşan şey sizin formunuz değil,
**kullanıcının göndermeyi seçtiği veridir.**

> **Bağlama öncelik sırası sınavda çıkıyor ve çoğu kişi ters ezberliyor:**
> **1) Form gövdesi · 2) Rota verisi · 3) Sorgu dizesi.**
> Sorgu dizesini adres çubuğunda gördüğümüz için en güçlü sanırız; **en zayıf olan odur.**

---

## Projeyi çalıştırma

```bash
cd KampusKitap
dotnet run
```

Ana sayfada **denenecek adreslerin listesi** var.

### Bu hafta eklenenler

| Dosya | Ne için |
|---|---|
| `Controllers/FormController.cs` | Haftanın bütün deneyleri: ham gövde, kaynak sırası, bağlama hatası, over-posting |
| `Views/Form/Index.cshtml` | Formun gönderdiği **ham gövdeyi** ekrana basar |
| `Views/Form/Kaynak.cshtml` | Aynı ad üç kaynaktan gelince hangisinin kazandığını gösterir |
| `Views/Form/Bagla.cshtml` | Bağlama başarısız olunca ne olduğunu gösterir |
| `Views/Form/Tuzak.cshtml` | 🕳️ Over-posting — iki form, tek satır fark |
| `ViewModels/KitapEkleVM.cs` | Formun alabileceği alanlar — **over-posting çözümü** |
| `Views/Kitap/Ekle.cshtml` | **Elle yazılmış** form (Tag Helper yok) |
| `Views/Kitap/EkleYanlis.cshtml` | ⚠️ PRG **uygulanmamış** hâl — karşılaştırma için |

### Denemeye değer adresler

| Adres | Ne gösteriyor |
|---|---|
| `/Form` | Formun gönderdiği **ham gövde**. `name` ile `id` farkı burada |
| `/Form/Kaynak/ROTA?deger=SORGU` | **Bağlama kaynakları:** POST düğmesiyle üçü birden yarışır |
| `/Form/KaynakZorla/ROTA?deger=SORGU` | Aynı istek, tek fark `[FromQuery]` |
| `/Form/Bagla?sayi=abc&tarih=xyz` | **Bağlama başarısız:** hata yok, `sayi = 0`, `ModelState` geçersiz |
| `/Kitap/Ekle` | Elle yazılmış form · ViewModel · **PRG** |
| `/Form/Tuzak` | 🕳️ **Over-posting:** formda olmayan alan gönderilince |

> 💡 Formu gönderdikten sonra **F12 → Network → Payload** bölümünü de açın.
> Sayfadaki ham gövdeyle aynı şeyi tarayıcının kendi aracında görürsünüz.

---

## 🧪 Kendiniz deneyin — kodu kasten bozun

| Deneme | Nerede | Beklenen |
|---|---|---|
| Bir `<input>`'un `name`'ini sil | `Views/Kitap/Ekle.cshtml` | Alan sunucuya **hiç gelmez** — hata da yok |
| `name="Baslik"` → `name="Balsik"` | aynı dosya | Özellik boş kalır, **hata yok** |
| `method="post"` → `method="get"` | aynı dosya | Değerler **adres çubuğuna** çıkar |
| `[HttpGet]`/`[HttpPost]` satırlarını sil | `KitapController` | `AmbiguousMatchException` |
| Fiyat alanına **harf** yazıp gönder | tarayıcı | `Fiyat = 0`, form hatalarla geri gelir |
| **"PRG'siz"** düğmesiyle gönderip **F5** | tarayıcı | Kayıt **ikinci kez** eklenir |
| `/Form/Tuzak`'ta ikinci formu gönder | tarayıcı | `Onayli = True` — formda o alan **yoktu** |
| `Ekle(KitapEkleVM)` → `Ekle(Kitap)` | `KitapController` | Over-posting **açılır** |

> Son satır bu haftanın güvenlik dersidir: entity'yi doğrudan action parametresi
> yapmak kod çalışırken bile yanlıştır.

---

## 🏠 Ev görevi

1. Ana varlığınız için **kayıt formunu elle** kurun (Tag Helper **yok**).
2. **GET/POST çifti** + `[HttpGet]` / `[HttpPost]`.
3. **F12 → Network → Payload** ekran görüntüsü; hangi anahtar hangi alandan geldi?
4. Bir alanın `name`'ini **bilerek yanlış** yazın, gönderin, sonucu gösterin. Sonra düzeltin.
5. Sayısal alana **harf** yazıp gönderin; parametrenin aldığı değeri ve `ModelState.IsValid`'i ekrana basıp görüntü alın.
6. POST action'ını **PRG** ile kapatın. Önce PRG'siz hâlinde F5 ile kaydın arttığını, sonra PRG ile artmadığını gösterin.
7. Bir **ViewModel** yazın; ona koymadığınız bir alanı el ile gönderip **bağlanmadığını** kanıtlayın.

**Teslim:** proje klasörü + 7 maddenin ekran görüntüleri + **elle yazdığınız formun
ürettiği HTML'i** kaydettiğiniz dosya (Hafta 5'te karşılaştıracağız) + şu soruya
**iki cümlelik** yazılı cevap: *"4. maddede neden hata almadınız?"*

---

## 🔜 Hafta 5

Bu hafta `name` eşleşmesinden **siz** sorumluydunuz ve yazım hatası yaptığınızda
kimse uyarmadı. Haftaya bu işi çatıya devrediyoruz: **Tag Helper'lar**.

**Düşünerek gelin:** `name="Baslik"` yazmak yerine çatıya *"bu alan modelin `Baslik`
özelliği için"* diyebilseydik ne kazanırdık? Yazım hatası **ne zaman** yakalanırdı?
