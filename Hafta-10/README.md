# Hafta 10 — ORM ve Entity Framework Core: Veriyi Kalıcı Yapmak

| | |
|---|---|
| 📄 **Ders notu** | [H10-Ders-Notu.pdf](../PDF/H10-Ders-Notu.pdf) — *sınav kapsamı* |
| 🔧 **Uygulama** | [H10-Uygulama.pdf](../PDF/H10-Uygulama.pdf) — derste yapılanların dökümü |
| 📚 **Detay** | [H10-Detay.pdf](../PDF/H10-Detay.pdf) — *sınav kapsamı DIŞI* |

---

## Bu haftanın konusu

- **Veri nerede yaşıyor** — bellek neden veri deposu olamaz; veritabanı, DBMS; tablo, satır, sütun, primary key
- **SQL** — `SELECT … WHERE … ORDER BY`, `INSERT`, `UPDATE`, `DELETE`; her birinin Hafta 9'daki LINQ karşılığı
- **ORM** — veritabanı ↔ `DbContext`, tablo ↔ `DbSet`, sütun ↔ özellik, satır ↔ nesne; okuma yolu (satır → nesne) ve yazma yolu (değişiklik → SQL); SQL'i elle çalıştırmakla karşılaştırma; Code First ve Database First
- **EF Core'u bağlamak** — provider paketi (10.0.x), `DbContext`, `DbSet`, `AddDbContext`, bağlantı dizesi
- **Migration** — convention ve data annotation'lar (`[Key]`, `[Table]`, `[Required]`, `[MaxLength]`, `[Column]`, `[Precision]`, `[NotMapped]`, `[Index]`), `Add-Migration` → `Update-Database`, `Up`/`Down`, seed (`HasData`)
- **CRUD** — `ToList`, `Find`, `Add`, `Remove`, `SaveChanges`; change tracking

🧩 **Haftanın ilkesi — Şemanın tarifi koddadır.**
Tabloyu elle değiştirmeyiz: sınıfı değiştiririz, migration farkı yazar, `Update-Database` uygular.

> **Haftanın tek cümlelik özeti:** Kitaplar artık uygulama kapanınca kaybolmuyor; Hafta 9'un
> sorguları veritabanından gelen listede aynen çalışıyor — ama bütün tablo belleğe gelerek.
> Sorguyu veritabanına yaptırmak Hafta 11'in konusu.

---

## İki proje: Başlangıç ve Bitmiş

| Klasör | Ne | Port |
|---|---|---|
| **`Baslangic/`** | **Buradan başlayın.** Haftanın konusu olan yerler boş bırakıldı ve **`TODO 10.x`** ile işaretlendi. Proje derlenir ve çalışır; boşken veritabanı kullanan sayfalar **500** verir (`AddDbContext` yok) — bilerek. `Migrations/` klasörü yok: ilk migration'ı siz üreteceksiniz. | `5072` |
| **`Bitmis/`** | Aynı projenin tamamlanmış hâli. Doldurulmuş her yerin üstünde **`✔ TODO 10.x`** izi var. `Migrations/` hazır; **ilk çalıştırmadan önce bir kez `Update-Database`** gerekir. | `5073` |

```bash
cd Bitmis                    # karşılaştırmak için
dotnet build                 # komut satırında ilk kez: önce derleyin
dotnet ef database update    # kampuskitap.db oluşur (Visual Studio'da: Update-Database)
dotnet run                   # → http://localhost:5073
```

> ⚠️ **Önce Bitmiş'e bakmayın.** Boşluğu kendiniz doldurduğunuzda neyin değiştiğini
> görürsünüz; hazır çözümü okuduğunuzda yalnızca sonucu görürsünüz.
>
> 📘 Bölüm 4 – 6, Microsoft'un **Getting Started with EF Core** öğreticisinin adımlarını izler: paket → model →
> veritabanı (migration) → CRUD — <https://learn.microsoft.com/ef/core/get-started/overview/first-app>
>
> 💡 **Paket sürümü:** projede EF Core **10.0.15** kurulu. Kendi projenize paket eklerken sürümü
> açıkça verin: sürümsüz kurulum EF Core 11'i seçer ve .NET 10 projesine kurulmaz (NU1202).
>
> 💡 **Komut satırı aracı:** `dotnet tool install --global dotnet-ef --version 10.0.15`.
> Visual Studio kullanıyorsanız gerekmez: *Tools → NuGet Package Manager → Package Manager Console*.
>
> 💡 `kampuskitap.db` depoda **yok** ve olmamalı: depoda şemanın tarifi (migration'lar) durur,
> veritabanı her bilgisayarda o tariften kurulur.

### Doldurulacak yerler

Editörde **`TODO 10.`** diye aratın. Her görevden sonra uygulamanın **konsoluna** bakın: EF Core'un
gönderdiği her SQL orada `Executed DbCommand` satırıyla görünür.

<!-- GOREVLER-BAS -->
| Görev | Ne yapılacak | Dosya | Doldurunca ne görülür |
|---|---|---|---|
| **TODO 10.1** | DbContext'i DI'ya kaydedin: provider SQLite, bağlantı dizesi appsettings.json'dan | `Program.cs` | Hafta 10 ders notu bölüm 4.4 — `builder.Services.AddDbContext<KampusKitapContext>(o => o.UseSqlite(…))`; bağlantı dizesini `builder.Configuration.GetConnectionString("KampusKitap")` verir. Doldurunca /Kitap/Istatistik'teki hata değişir: "no such table: Kitaplar" (tablo henüz yok — TODO 10.2) |
| **TODO 10.2** | İlk migration'ı üretin ve veritabanına uygulayın | `Data/KampusKitapContext.cs` | Hafta 10 ders notu bölüm 5.2 — Package Manager Console'da `Add-Migration Ilk`, sonra `Update-Database` (CLI: `dotnet ef migrations add Ilk`, `dotnet ef database update`). Migrations klasörü ve kampuskitap.db oluşur; /Kitap/Istatistik 11 kitabı sayar |
| **TODO 10.3** | Kitapları veritabanından okuyun: liste için ToList, tek kitap için Find | `Controllers/KitapController.cs` | Hafta 10 ders notu bölüm 6.1 — `_db.Kitaplar.ToList()` ve `_db.Kitaplar.Find(id)`. Konsoldaki "Executed DbCommand" satırlarında SELECT'i görün; Find bulamazsa null döner → 404 |
| **TODO 10.4** | Yeni kitabı veritabanına kaydedin: Add, sonra SaveChanges | `Controllers/KitapController.cs` | Hafta 10 ders notu bölüm 6.3 — `_db.Kitaplar.Add(kitap)` tek başına bir şey yazmaz; `_db.SaveChanges()` INSERT'i gönderir. Uygulamayı yeniden başlatın: kitap listede durur (Hafta 9'a kadar kayboluyordu) |
| **TODO 10.5** | Kitabı güncelleyin: Find ile getirin, özelliklerini değiştirin, SaveChanges | `Controllers/KitapController.cs` | Hafta 10 ders notu bölüm 6.4 — `Update()` çağırmayın; Find'ın getirdiği nesneyi context zaten izliyor. Konsolda UPDATE'in yalnızca değiştirdiğiniz sütunları yazdığını görün |
| **TODO 10.6** | Kitabı silin: Find, Remove, SaveChanges | `Controllers/KitapController.cs` | Hafta 10 ders notu bölüm 6.5 — `_db.Kitaplar.Remove(kitap)`, sonra `_db.SaveChanges()`. Konsolda DELETE FROM "Kitaplar" WHERE "Id" = … satırına bakın |
<!-- GOREVLER-SON -->

### Bu hafta eklenenler

| Dosya | Ne için |
|---|---|
| `KampusKitap.csproj` | EF Core paketleri: `Sqlite` (provider), `Design`, `Tools` — hepsi 10.0.15 |
| `appsettings.json` | Bağlantı dizesi: `ConnectionStrings:KampusKitap` → `Data Source=kampuskitap.db` |
| `Data/KampusKitapContext.cs` | `DbContext`, `DbSet<Kitap> Kitaplar`, seed: Hafta 9'un on bir kitabı |
| `Program.cs` | `AddDbContext` — TODO 10.1 |
| `Models/Kitap.cs` | Artık bir entity: `Kitaplar` tablosunun bir satırı |
| `Data/KitapDeposu.cs` | Kitaplar çıktı; yalnızca kategoriler kaldı (Hafta 11'de tabloya geçecek) |
| `Controllers/KitapController.cs` | Okumak, eklemek, düzenlemek, silmek — TODO 10.3 – 10.6 |
| `Views/Kitap/Duzenle.cshtml`, `Views/Kitap/Sil.cshtml` | Düzenleme formu; silme onay sayfası |
| `Migrations/` (yalnızca Bitmiş) | İlk migration: `Ilk` |

### Denemeye değer adresler

| Adres | Ne gösteriyor |
|---|---|
| `/Kitap/Istatistik` | Başlangıç'ta görevlerin ilerleyişi: DI hatası → *no such table* → 11 kitap |
| `/Kitap/Detay/7` | `Find`: konsolda `WHERE "k"."Id" = @p LIMIT 1` |
| `/Kitap/EkleTH` | `Add` + `SaveChanges`: konsolda `INSERT … RETURNING "Id"`; uygulamayı yeniden başlatın, kitap duruyor |
| `/Kitap/Duzenle/43` | Yalnızca fiyatı değiştirin: `UPDATE "Kitaplar" SET "Fiyat" = @p0` |
| `/Kitap/Sil/43` | Önce onay sayfası (GET), silme formun POST'unda: `DELETE FROM "Kitaplar"` |
| `/Kitap?kategori=1&sirala=fiyat&sayfa=2` | Hafta 9'un süzgeci aynen çalışıyor — konsoldaki SQL'de `WHERE` yok |

---

## 🧪 Kendiniz deneyin — kodu kasten bozun

Bunları **Bitmiş** projenin bir kopyasında deneyin.

| Deneme | Nerede | Beklenen |
|---|---|---|
| `AddDbContext` satırını yorum yapın | `Program.cs` | **500**: *Unable to resolve service for type 'KampusKitap.Data.KampusKitapContext'…* |
| `kampuskitap.db`'yi silin, `Update-Database` yapmadan çalıştırın | proje klasörü | **500**: *SQLite Error 1: 'no such table: Kitaplar'.* — dosya 0 baytla yeniden oluşur |
| `_db.SaveChanges();` satırını silin, kitap ekleyin | `KitapController.EkleTH` | "eklendi" mesajı gelir, kitap listede yok — hata yok |
| `Kitap`'a `public int SayfaSayisi { get; set; }` ekleyin, migration'sız `Update-Database` | `Models/Kitap.cs` | *PendingModelChangesWarning … has pending changes* |
| `Yazar`'ı `[MaxLength(60)] YazarAdi` yapın, `Add-Migration` | `Models/Kitap.cs` | *An operation was scaffolded that may result in the loss of data* — `Up()`'ı okuyun: `DropColumn` + `AddColumn` |

---

## 🏠 Ev görevi

0. **Başlangıç** projesinde **TODO 10.1 – 10.6**'yı tamamlayın; `Ilk` migration'ını kendiniz üretin
   ve `Up()`'ını okuyun.
1. Kendi projenizin verisini EF Core'a taşıyın: entity, `DbContext`, `AddDbContext`, seed, ilk migration.
2. En az bir entity için **tam CRUD**: listele, ayrıntı, ekle, düzenle (`Update()` kullanmadan), sil
   (onay sayfası + POST).
3. Entity'nize bir özellik ekleyip ikinci migration'ı üretin; `Up()`'ın ekran görüntüsünü alın.
4. Bir özelliği yeniden adlandırın; migration'ın `RenameColumn` ürettiğini doğrulayın.

**Teslim:** proje klasörü (`*.db` hariç) + 3. ve 4. maddelerin ekran görüntüleri + şu soruya
**iki cümlelik** cevap: *"Migration'ı uygulamadan önce neden okursunuz?"*

---

## 🔜 Gelecek hafta: EF Core ile LINQ

Bu hafta okurken hep bütün tabloyu aldık: `_db.Kitaplar.ToList()`, sonra Hafta 9'un LINQ'i
bellekte. Gelecek hafta aynı `Where`, `OrderBy`, `Skip`/`Take`'i **veritabanına** yaptıracağız;
kategoriler de bir tabloya geçecek ve kitapla **ilişki** kuracağız.

**Düşünerek gelin:** `_db.Kitaplar.ToList()`'ten `.ToList()`'i silseydiniz konsoldaki `SELECT`'te
bir `WHERE` belirir miydi?
