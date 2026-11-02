# Hafta 7 — State Management: Cookie ve Session

| | |
|---|---|
| 📄 **Ders notu** | [H07-Ders-Notu.pdf](../PDF/H07-Ders-Notu.pdf) — *sınav kapsamı* |
| 🔧 **Uygulama** | [H07-Uygulama.pdf](../PDF/H07-Uygulama.pdf) — derste yapılanların dökümü |
| 📚 **Detay** | [H07-Detay.pdf](../PDF/H07-Detay.pdf) — *sınav kapsamı DIŞI* |

---

## Bu haftanın konusu

- **State nerede tutulur** — HTTP hatırlamaz; query string, form, cookie, session, veritabanı
- **Cookie** — `Set-Cookie` ve `Cookie` başlıkları; `Response.Cookies.Append`, `Request.Cookies["…"]`, `CookieOptions`; geçici ve kalıcı cookie; sınırlar
- **Session** — veri sunucuda, anahtarı tarayıcıda; `AddSession` + `UseSession`; `SetString`/`SetInt32`; nesneyi JSON'a çevirerek saklamak
- **Güvenlik** — cookie kullanıcının elindedir; `HttpOnly`, `Secure`, `SameSite` neyi korur, neyi korumaz
- **Hangi veri nerede** — cookie, session, `TempData`, query string, veritabanı arasında karar

🧩 **Haftanın ilkesi — State bir yerde tutulmak zorundadır; soru "nerede" ve "kime ait" sorusudur.**
Tarayıcıda duran veri kullanıcınındır: okur, değiştirir, siler. Sunucuda duran veri sizindir.

> **Haftanın tek cümlelik özeti:** Hafta 1'de HTTP'nin hiçbir şeyi hatırlamadığını
> gördük. Bu hafta uygulamanın kullanıcıyı nasıl hatırladığını — ve hatırlanan
> bilginin kimin elinde durduğunun neden önemli olduğunu — görüyoruz.

---

## İki proje: Başlangıç ve Bitmiş

| Klasör | Ne | Port |
|---|---|---|
| **`Baslangic/`** | **Buradan başlayın.** Haftanın konusu olan yerler boş bırakıldı ve **`TODO 7.x`** ile işaretlendi. Proje derlenir ve çalışır; session'a dokunan sayfalar TODO 7.3 ve 7.4'e kadar **500** döner — bilerek. | `5072` |
| **`Bitmis/`** | Aynı projenin tamamlanmış hâli. Doldurulmuş her yerin üstünde **`✔ TODO 7.x`** izi var — takıldığınızda ya da bitirince karşılaştırmak için. | `5073` |

```bash
cd Baslangic        # önce bu
dotnet run          # → http://localhost:5072

cd Bitmis           # karşılaştırmak için, ayrı bir terminalde
dotnet run          # → http://localhost:5073
```

> ⚠️ **Önce Bitmiş'e bakmayın.** Boşluğu kendiniz doldurduğunuzda neyin değiştiğini
> görürsünüz; hazır çözümü okuduğunuzda yalnızca sonucu görürsünüz.
>
> ⚠️ **İki projeyi aynı tarayıcıda açarken:** tarayıcı cookie'leri porta göre ayırmaz.
> 5072'de yazılan `SonBakilan` 5073'te de görünür. Karışmaması için F12 → Application →
> Cookies'ten temizleyin.

### Doldurulacak yerler

Editörde **`TODO 7.`** diye aratın. Her görevden sonra F12 → **Network**'te
`Set-Cookie`'ye, F12 → **Application** → **Cookies**'e ve projedeki `/Durum` sayfasına bakın.

<!-- GOREVLER-BAS -->
| Görev | Ne yapılacak | Dosya | Doldurunca ne görülür |
|---|---|---|---|
| **TODO 7.1** | Bakılan kitabın Id'sini 30 gün yaşayan, HttpOnly bir cookie'ye yazın | `Controllers/KitapController.cs` | Hafta 7 ders notu bölüm 2.2 — `Response.Cookies.Append("SonBakilan", …, new CookieOptions { … })`. F12 → Application → Cookies'te Expires ve HttpOnly sütunlarına bakın |
| **TODO 7.2** | Son bakılan kitabı cookie'den okuyun ve view'a verin | `Controllers/KitapController.cs` | Hafta 7 ders notu bölüm 2.3 — `Request.Cookies["SonBakilan"]`; değer metindir ve kullanıcı değiştirebilir: `int.TryParse` ile çevirin, `KitapDeposu.Bul` ile bulun, `ViewData["SonBakilan"]`'a koyun |
| **TODO 7.3** | Session servislerini kaydedin: `AddDistributedMemoryCache` ve `AddSession` | `Program.cs` | Hafta 7 ders notu bölüm 3.2 — `builder.Build()`'dan ÖNCE. Bu satır olmadan /Durum/SessionSayaci "Session has not been configured" ile 500 döner |
| **TODO 7.4** | Session middleware'ini ekleyin: `app.UseSession()` | `Program.cs` | Hafta 7 ders notu bölüm 3.3 — UseRouting ve UseAuthorization'dan sonra, MapControllerRoute'tan önce. AddSession var ama bu satır yoksa yine 500 alınır |
| **TODO 7.5** | Nesneyi JSON'a çevirip session'a yazın; okurken geri çevirin | `Services/SessionUzantilari.cs` | Hafta 7 ders notu bölüm 3.5 — `JsonSerializer.Serialize` ve `SetString`; okurken `GetString` ve `JsonSerializer.Deserialize<T>`. Anahtar yoksa `default` dönün |
| **TODO 7.6** | Kitabı sepete ekleyin: sepeti session'dan okuyun, satırı ekleyin ya da adedi artırın, sepeti geri yazın | `Controllers/SepetController.cs` | Hafta 7 ders notu bölüm 3.5 — `NesneOku<List<SepetSatiri>>`, sonra `NesneYaz`. Geri yazmayı unutursanız sepet hep boş görünür: okunan liste bir KOPYADIR |
| **TODO 7.7** | Rozet sepetteki toplam adedi göstersin: sepeti session'dan okuyun | `ViewComponents/SepetRozetiViewComponent.cs` | Hafta 7 ders notu bölüm 3.6 — View Component'te `HttpContext.Session` doğrudan erişilebilir. Sepet yoksa 0 |
| **TODO 7.8** | Rolü cookie'de değil session'da tutun: girişte session'a yazın, panelde session'dan okuyun | `Controllers/DurumController.cs` | Hafta 7 ders notu, Klasik Tuzak — önce F12'den `Rol` cookie'sini `yonetici` yapıp paneli açın; sonra üç bloğu `SetString`/`GetString`/`Remove` ile değiştirin ve aynı denemeyi tekrarlayın |
<!-- GOREVLER-SON -->

### Bu hafta eklenenler

| Dosya | Ne için |
|---|---|
| `Controllers/KitapController.cs` | Son bakılan kitap cookie'si: yazmak (TODO 7.1) ve okumak (TODO 7.2) |
| `Program.cs` | Session servisleri ve middleware'i — TODO 7.3, 7.4 |
| `Services/SessionUzantilari.cs` | Session'a nesne yazıp okuyan iki uzantı metodu (JSON) — TODO 7.5 |
| `Models/SepetSatiri.cs`, `Controllers/SepetController.cs`, `Views/Sepet/` | Session'da duran sepet — TODO 7.6 |
| `ViewComponents/SepetRozetiViewComponent.cs` | Menüdeki sepet rozeti — TODO 7.7 |
| `Controllers/DurumController.cs`, `Views/Durum/` | Haftanın ölçüm sayfaları; Klasik Tuzak sayfası — TODO 7.8 |

### Denemeye değer adresler

| Adres | Ne gösteriyor |
|---|---|
| `/Durum` | Tarayıcının bu request'le getirdiği cookie'ler ve session'ın içi |
| `/Durum/Cerez` | Geçici, kalıcı, `HttpOnly` ve 5000 baytlık cookie; sunucunun gördüğü ile JavaScript'in gördüğü yan yana |
| `/Durum/AyniIstek` | Yazılan cookie aynı request'te okunabilir mi? |
| `/Durum/SessionSayaci` | Session sayacı — yeni sekmede, gizli pencerede, yeniden başlatınca |
| `/Durum/Panel` | 🕳️ Klasik Tuzak: rol cookie'de |
| `/Kitap/Detay/3` → `/Kitap` | Son bakılan kitap cookie'si |
| `/Sepet` | Session'daki sepet |

---

## 🧪 Kendiniz deneyin — kodu kasten bozun

Bunları **Bitmiş** projede deneyin.

| Deneme | Nerede | Beklenen |
|---|---|---|
| `app.UseSession();` satırını silin | `Program.cs` | `/Durum/SessionSayaci` **500**: *Session has not been configured…* |
| `AddSession`'ı silin, `UseSession`'ı bırakın | `Program.cs` | Uygulama **açılmaz**: *Unable to resolve service for type '…ISessionStore'* |
| `NesneYaz` satırını silin, sepete ekleyin | `SepetController.Ekle` | "Eklendi" mesajı gelir, sepet **boş** — hata yok |
| `int.TryParse`'ı `int.Parse` yapın, `SonBakilan`'ı F12'de `abc` yapın | `KitapController.Index` | `/Kitap` **500** |
| `SonBakilan` cookie'sinden `HttpOnly = true` satırını silin | `KitapController.Detay` | F12 → Console'da `document.cookie` artık `SonBakilan`'ı gösterir |

---

## 🏠 Ev görevi

0. **Başlangıç** projesinde **TODO 7.1 – 7.8**'i tamamlayın.
1. Kendi projenizde bir **tercihi** (tema ya da dil) kalıcı bir cookie'de tutun; okurken
   değere güvenmeyin.
2. Bir **favori listesi** ya da **sepet**i session'da tutun: nesneyi JSON'a çevirerek
   yazın, okurken geri çevirin.
3. Menüde sayısını gösteren bir **View Component** yazın.
4. F12 → Application ekran görüntüsü alın ve işaretleyin: hangi veri tarayıcıda,
   hangisi değil?

**Teslim:** proje klasörü + 4. maddenin ekran görüntüsü + şu soruya **iki cümlelik**
cevap: *"Sepetteki fiyatı neden cookie'de tutmadınız?"*

---

## 🔜 Gelecek hafta ders yok: ara sınav

Ara sınav **Hafta 1–6**'yı kapsar. Bu haftanın konuları (cookie, session) ara sınavda
sorulmaz, **finalde** sorulur.

Bu hafta sepet session'da duruyordu ve uygulama yeniden başlayınca **kayboldu**. Kitap
listesi de baştan beri bellekteki bir liste. Sınavdan sonraki derste veriyi **kalıcı** yapıyoruz:
Entity Framework Core ve Code First ile bir veritabanı.

**Düşünerek gelin:** Uygulama iki ayrı bilgisayarda çalışsaydı, birinde eklenen kitap
ötekinde görünür müydü? *Veri nerede duruyor?* sorusu kitap listesi için ne söylüyor?
