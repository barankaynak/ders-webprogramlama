# Hafta 9 — Entity Framework Core: Code First, Migration ve İlişkiler

| | |
|---|---|
| 📄 **Ders notu** | [H09-Ders-Notu.pdf](../PDF/H09-Ders-Notu.pdf) — *sınav kapsamı (final)* |
| 🔧 **Uygulama** | [H09-Uygulama.pdf](../PDF/H09-Uygulama.pdf) — derste yapılanların dökümü |
| 📚 **Detay** | [H09-Detay.pdf](../PDF/H09-Detay.pdf) — *sınav kapsamı DIŞI* |

> ⚠️ **Bu haftanın projesi bir veritabanı kullanır ve veritabanı depoda yoktur.**
> Depoda yalnızca şemanın tarifi (migration'lar) durur; veritabanı sizin bilgisayarınızda
> o tariften kurulur. **Bitmiş**'i çalıştırmadan önce bir kez:
>
> - Visual Studio → Package Manager Console: `Update-Database`
> - ya da komut satırında: `dotnet build`, sonra `dotnet ef database update`
>
> **Başlangıç**'ta migration'ları siz üretirsiniz; `Update-Database` TODO 9.2'nin parçasıdır.
> Komut satırı aracı projeyle aynı sürümde olmalı:
> `dotnet tool install --global dotnet-ef --version 10.0.15`

---

## Bu haftanın konusu

- **Veri nerede yaşıyor** — bellekteki liste neden yetmez; projede iki ayrı kategori listesinin birbirini tutmaması
- **ORM ve EF Core** — nesneden tabloya çeviri; Code First ve Database First; provider ve SQLite
- **Bağlamak** — doğru sürümde paketler, bağlantı dizesi, `AddDbContext`, `DbContext` ve `DbSet`
- **CRUD** — `ToList`, `Find`, `Add`, `Remove`, `SaveChanges`; change tracking ve entity durumları
- **Migration** — `Add-Migration` → `Update-Database`; `Up`/`Down`'u okumak; seeding; bekleyen model değişikliği
- **İlişkiler** — bire-çok ve çoka-çok; navigation property, foreign key, `Include`; silme davranışı

🧩 **Haftanın ilkesi — Tek doğruluk kaynağı.**
Aynı bilgi iki yerde tutulursa er geç iki yer birbirini tutmaz. Veri tek bir yerde durur;
şema tek bir yerden, koddan türetilir.

> **Haftanın tek cümlelik özeti:** Hafta 7'de sepet uygulama yeniden başlayınca kayboldu.
> Bu hafta veri kalıcı oluyor — ve veritabanına giden her adımın (kayıt, şema, sorgu)
> eksikken ne yaptığını tek tek görüyoruz.

> 📅 Hafta 8 ara sınav haftasıydı. Önceki notlarda "Hafta 8'de EF Core" diye anılan konu bu haftadır.

---

## İki proje: Başlangıç ve Bitmiş

| Klasör | Ne | Port |
|---|---|---|
| **`Baslangic/`** | **Buradan başlayın.** Haftanın konusu olan yerler boş bırakıldı ve **`TODO 9.x`** ile işaretlendi. Proje derlenir ve çalışır; veritabanına dokunan sayfalar TODO 9.1 ve 9.2'ye kadar **500** döner — bilerek. `Migrations/` klasörü **yoktur**: onu siz üreteceksiniz. | `5072` |
| **`Bitmis/`** | Aynı projenin tamamlanmış hâli, üç migration'ıyla (`Ilk`, `KategoriIliskisi`, `DersKitap`). Doldurulmuş her yerin üstünde **`✔ TODO 9.x`** izi var. **Önce `Update-Database`.** | `5073` |

```bash
cd Baslangic        # önce bu
dotnet run          # → http://localhost:5072

cd Bitmis           # karşılaştırmak için, ayrı bir terminalde
dotnet build
dotnet ef database update
dotnet run          # → http://localhost:5073
```

> ⚠️ **Önce Bitmiş'e bakmayın.** Boşluğu kendiniz doldurduğunuzda neyin değiştiğini
> görürsünüz; hazır çözümü okuduğunuzda yalnızca sonucu görürsünüz.
>
> ⚠️ **İki proje, iki veritabanı:** her proje kendi klasöründeki `kampuskitap.db`'yi
> kullanır. Birinde eklediğiniz kitap ötekinde görünmez.

### Doldurulacak yerler

Editörde **`TODO 9.`** diye aratın. Her görevden sonra uygulamanın **konsoluna** bakın:
EF Core'un çalıştırdığı her SQL orada `Executed DbCommand` satırıyla görünür.

<!-- GOREVLER-BAS -->
| Görev | Ne yapılacak | Dosya | Doldurunca ne görülür |
|---|---|---|---|
| **TODO 9.1** | DbContext'i DI'ya kaydedin: provider SQLite, bağlantı dizesi appsettings.json'dan | `Program.cs` | Hafta 9 ders notu bölüm 2.3 — `builder.Services.AddDbContext<KampusKitapContext>(o => o.UseSqlite(…))`; bağlantı dizesini `builder.Configuration.GetConnectionString("KampusKitap")` verir. Doldurunca /Kategori'deki hata değişir: "no such table: Kategoriler" (tablolar henüz yok — TODO 9.2) |
| **TODO 9.2** | İlk migration'ı üretin ve veritabanına uygulayın | `Data/KampusKitapContext.cs` | Hafta 9 ders notu bölüm 4.1 — Package Manager Console'da `Add-Migration Ilk`, sonra `Update-Database` (CLI: `dotnet ef migrations add Ilk`, `dotnet ef database update`). Migrations klasörü ve kampuskitap.db oluşur; /Kategori üç kategoriyi listeler |
| **TODO 9.3** | Kitapları veritabanından okuyun: liste için ToList, tek kitap için Find | `Controllers/KitapController.cs` | Hafta 9 ders notu bölüm 3.3 — `_db.Kitaplar.ToList()` ve `_db.Kitaplar.Find(id)`. Konsoldaki "Executed DbCommand" satırlarında SELECT'i görün; Find bulamazsa null döner → 404 |
| **TODO 9.4** | Yeni kitabı veritabanına kaydedin: Add, sonra SaveChanges | `Controllers/KitapController.cs` | Hafta 9 ders notu bölüm 3.4 — `_db.Kitaplar.Add(kitap)` tek başına bir şey yazmaz; `_db.SaveChanges()` INSERT'i gönderir. Uygulamayı yeniden başlatın: kitap listede durur (Hafta 7'deki sepet durmuyordu) |
| **TODO 9.5** | Kitabı güncelleyin: Find ile getirin, özelliklerini değiştirin, SaveChanges | `Controllers/KitapController.cs` | Hafta 9 ders notu bölüm 3.5 — `Update()` çağırmayın; Find'ın getirdiği nesneyi context zaten izliyor. Konsolda UPDATE'in yalnızca değiştirdiğiniz sütunları yazdığını görün |
| **TODO 9.6** | Kitabı silin: Find, Remove, SaveChanges | `Controllers/KitapController.cs` | Hafta 9 ders notu bölüm 3.6 — `_db.Kitaplar.Remove(kitap)`, sonra `_db.SaveChanges()`. Konsolda DELETE FROM "Kitaplar" WHERE "Id" = … satırına bakın |
| **TODO 9.7** | Bire-çok ilişkiyi kurun: Kitap'a Kategori, Kategori'ye Kitaplar navigation property'si; sonra Add-Migration KategoriIliskisi ve Update-Database | `Models/Kategori.cs`<br>`Models/Kitap.cs`<br>`Views/Kategori/Index.cshtml` | Hafta 9 ders notu bölüm 5.1 — migration dosyasında AddForeignKey ve CreateIndex görünür; /Kategori sayfası her kategoride "0 kitap" der: kitaplar Include ile gelir (TODO 9.8) |
| **TODO 9.8** | Kategorileri kitaplarıyla birlikte getirin: Include | `Controllers/KategoriController.cs` | Hafta 9 ders notu bölüm 5.2 — `_db.Kategoriler.Include(k => k.Kitaplar).ToList()`. Önce Include'suz çalıştırın: her kategoride 0 kitap görünür, hata yok. Sonra konsolda LEFT JOIN'i görün |
| **TODO 9.9** | Çoka-çok ilişkiyi kurun: Kitap'a Dersler, Ders'e Kitaplar; sonra Add-Migration DersKitap ve Update-Database | `Controllers/DersController.cs`<br>`Data/KampusKitapContext.cs`<br>`Models/Ders.cs`<br>`Models/Kitap.cs`<br>`Views/Ders/Index.cshtml` | Hafta 9 ders notu bölüm 5.3 — migration'da DersKitap tablosu (DerslerId + KitaplarId) oluşur; /Ders sayfası her dersin kitaplarını listeler |
| **TODO 9.10** | Kitabı derse ekleyin: dersi kitaplarıyla getirin, kitabı listeye ekleyin, SaveChanges | `Controllers/DersController.cs` | Hafta 9 ders notu bölüm 5.4 — `ders.Kitaplar.Add(kitap)`; ara tabloya kimse elle yazmaz, konsolda INSERT INTO "DersKitap" satırını görün |
<!-- GOREVLER-SON -->

### Bu hafta eklenenler

| Dosya | Ne için |
|---|---|
| `KampusKitap.csproj` | EF Core paketleri: Sqlite, Design, Tools — **10.0.15** |
| `appsettings.json` | Bağlantı dizesi: `ConnectionStrings:KampusKitap` |
| `Program.cs` | `AddDbContext` — TODO 9.1 |
| `Data/KampusKitapContext.cs` | `DbContext`, üç `DbSet`, başlangıç verisi (`HasData`) — TODO 9.2, 9.9 |
| `Models/Kategori.cs`, `Models/Ders.cs` | Yeni entity'ler; `Kitap.cs`'e navigation property'ler — TODO 9.7, 9.9 |
| `Controllers/KitapController.cs` | Okuma, ekleme, düzenleme, silme — TODO 9.3 – 9.6 |
| `Views/Kitap/Duzenle.cshtml`, `Views/Kitap/Sil.cshtml` | CRUD'un U'su ve D'si |
| `Controllers/KategoriController.cs` | Kategoriler tablodan, kitaplarıyla birlikte — TODO 9.8 |
| `Controllers/DersController.cs`, `Views/Ders/` | Çoka-çok ilişki: derslerin kitapları — TODO 9.9, 9.10 |
| `Migrations/` (yalnızca Bitmiş) | Şemanın geçmişi: `Ilk`, `KategoriIliskisi`, `DersKitap` |
| ~~`Data/KitapDeposu.cs`~~ | **Silindi.** Veri artık veritabanında |

### Denemeye değer adresler

| Adres | Ne gösteriyor |
|---|---|
| `/Kitap` | Kitaplar veritabanından; konsolda `SELECT … FROM "Kitaplar"` |
| `/Kitap/EkleTH` → `/Kitap/Detay/43` → Düzenle / Sil | CRUD; konsolda `INSERT`, yalnız değişen sütunu yazan `UPDATE`, `DELETE` |
| `/Kategori` | Kategoriler kitaplarıyla birlikte: `Include` ve `LEFT JOIN` |
| `/Kategori/Detay/99` | `Find` bulamayınca `null` → 404 |
| `/Ders` | Çoka-çok: hangi kitap hangi derste okutuluyor; derse kitap ekleme |
| `/Kitap/Tuzak` | 🕳️ Hafta 3'ün N+1 tuzağı, bu kez gerçek sorgularla: konsolda sorguları sayın |

---

## 🧪 Kendiniz deneyin — kodu kasten bozun

Bunları **Bitmiş** projede deneyin.

| Deneme | Nerede | Beklenen |
|---|---|---|
| `AddDbContext` satırını silin | `Program.cs` | `/Kitap` **500**: *Unable to resolve service for type 'KampusKitap.Data.KampusKitapContext'…* |
| Uygulamayı durdurup `kampuskitap.db*` dosyalarını silin, `Update-Database` yapmadan çalıştırın | proje klasörü | `/Kategori` **500**: *no such table: Kategoriler* |
| `Kitap`'a `public int SayfaSayisi { get; set; }` ekleyin, migration'sız çalıştırın | `Models/Kitap.cs` | `/Kitap` **500**: *no such column: k.SayfaSayisi*; `Update-Database` → *PendingModelChangesWarning* |
| `Include(k => k.Kitaplar)`'ı silin | `KategoriController.Index` | Her kategoride **"0 kitap"** — hata yok |
| `_db.SaveChanges();`'i silin, kitap ekleyin | `KitapController.EkleTH` | "eklendi" mesajı gelir, kitap listede **yok** |
| `Kategori?`'yi `Kategori Kategori { get; set; } = null!;` yapın | `Models/Kitap.cs` | Şema değişmez (`has-pending-model-changes` "No changes…"); entity bir forma bağlansaydı *"The Kategori field is required."* |

---

## 🏠 Ev görevi

0. **Başlangıç** projesinde **TODO 9.1 – 9.10**'u tamamlayın; üç migration'ı kendiniz üretin
   ve her birinin `Up()`'ını okuyun.
1. Kendi projenizi EF Core'a taşıyın: `DbContext`, `AddDbContext`, ilk migration.
2. En az bir entity için **tam CRUD**: listele, ayrıntı, ekle, düzenle, sil.
3. En az **bir bire-çok** ve **bir çoka-çok** ilişki; ilişkili veriyi `Include` ile gösterin.
4. Bir özelliği yeniden adlandırın ve migration'ının `RenameColumn` olduğunu gösteren
   ekran görüntüsünü alın.

**Teslim:** proje klasörü (`*.db` hariç) + 4. maddenin ekran görüntüsü + şu soruya **iki
cümlelik** cevap: *"Migration'ı uygulamadan önce neden okursunuz?"*

---

## 🔜 Hafta 10 — LINQ ile Veri Sorgulama

Bu hafta veri kalıcı oldu, ama okurken hep **bütün tabloyu** aldık: `ToList()`.
Hafta 10'da LINQ ile filtrelemeyi, sıralamayı, sayfalamayı — ve bunları **veritabanına**
yaptırmayı göreceğiz.

**Düşünerek gelin:** `/Kitap/Ara` şu an bütün kitapları belleğe alıp C#'ta süzüyor.
Tabloda 10.000 kitap olsaydı veritabanından kaç satır gelirdi? Konsoldaki `SELECT`
satırında bir `WHERE` görüyor musunuz?
