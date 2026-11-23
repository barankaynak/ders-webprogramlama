# Hafta 11 — EF Core ile LINQ: İlişkiler ve Veritabanında Sorgu

| | |
|---|---|
| 📄 **Ders notu** | [H11-Ders-Notu.pdf](../PDF/H11-Ders-Notu.pdf) — *sınav kapsamı* |
| 🔧 **Uygulama** | [H11-Uygulama.pdf](../PDF/H11-Uygulama.pdf) — derste yapılanların dökümü |
| 📚 **Detay** | [H11-Detay.pdf](../PDF/H11-Detay.pdf) — *sınav kapsamı DIŞI* |

---

## Bu haftanın konusu

- **İlişki** — iki tablo, foreign key, bire-çok; foreign key bir kısıttır
- **Navigation property** — `Kitap.Kategori` ve `Kategori.Kitaplar`: tabloda sütunu yok; migration `KategoriIliskisi` (tablo, seed, index, foreign key)
- **İlişkili veriyi yüklemek** — `Include`; yazılmazsa navigation `null`; tek kayıt için `FirstOrDefaultAsync`
- **Sorgu veritabanında** — `IEnumerable` ↔ `IQueryable`; `Where`, `OrderBy`, `Skip`/`Take` SQL'e çevrilir (`WHERE`, `ORDER BY`, `LIMIT`/`OFFSET`); sonucu isteyen çağrı en sonda
- **Toplama ve gruplama** — `CountAsync`, `SumAsync`, `AverageAsync`…; navigation üzerinden `GroupBy` (JOIN) ve `Count` (alt sorgu)
- **Ne çevrilir, ne çevrilmez** — *could not be translated*; metin karşılaştırmasında veritabanının kuralı
- **N+1** — döngüde sorgu; `Include` ile tek sorgu
- **async / await** — `async Task<IActionResult>`, `…Async` metotlar

🧩 **Haftanın ilkesi — Süzmeyi veritabanına yaptır, belleğe yalnızca göstereceğini getir.**
Sonucu isteyen çağrı (`ToListAsync`, `CountAsync`…) en sonda; ondan önceki her halka SQL'e eklenir.

> **Haftanın tek cümlelik özeti:** Kategoriler artık bir tabloda ve kitaplarla ilişkili; Hafta 9'un
> sorguları değişmeden veritabanında çalışıyor — ama bir sorgunun nerede çalıştığını kod değil,
> konsoldaki SQL söylüyor.

---

## İki proje: Başlangıç ve Bitmiş

| Klasör | Ne | Port |
|---|---|---|
| **`Baslangic/`** | **Buradan başlayın.** Haftanın konusu olan yerler boş bırakıldı ve **`TODO 11.x`** ile işaretlendi. Proje derlenir; `Migrations/` içinde yalnızca Hafta 10'un `Ilk` migration'ı var. `KategoriIliskisi` migration'ını siz üreteceksiniz — o zamana kadar `Update-Database` *PendingModelChangesWarning* ile durur ve sayfalar *no such table* verir. | `5072` |
| **`Bitmis/`** | Aynı projenin tamamlanmış hâli. Doldurulmuş her yerin üstünde **`✔ TODO 11.x`** izi var. `Migrations/` hazır; **ilk çalıştırmadan önce bir kez `Update-Database`** gerekir. | `5073` |

```bash
cd Bitmis                    # karşılaştırmak için
dotnet build                 # komut satırında ilk kez: önce derleyin
dotnet ef database update    # kampuskitap.db oluşur (Visual Studio'da: Update-Database)
dotnet run                   # → http://localhost:5073
```

> ⚠️ **Önce Bitmiş'e bakmayın.** Boşluğu kendiniz doldurduğunuzda neyin değiştiğini
> görürsünüz; hazır çözümü okuduğunuzda yalnızca sonucu görürsünüz.
>
> 📘 Bu hafta Microsoft'un EF Core belgesinin üç başlığını izler: **Relationships**, **Querying Data**
> (*Loading Related Data*, *Client vs. Server Evaluation*) ve **Asynchronous Programming** —
> <https://learn.microsoft.com/ef/core/>
>
> 💡 `Update-Database` sırasında konsolda *"… cannot be executed in a transaction…"* uyarısı
> normaldir: SQLite foreign key eklerken tabloyu yeniden kurar.

### Doldurulacak yerler

Editörde **`TODO 11.`** diye aratın. Her görevden sonra uygulamanın **konsoluna** bakın: EF Core'un
gönderdiği her SQL orada `Executed DbCommand` satırıyla görünür. Bu hafta SQL'leri **sayacaksınız**.

<!-- GOREVLER-BAS -->
| Görev | Ne yapılacak | Dosya | Doldurunca ne görülür |
|---|---|---|---|
| **TODO 11.1** | Bire-çok ilişkiyi kurun: Kitap'a Kategori, Kategori'ye Kitaplar navigation property'si | `Models/Kategori.cs`<br>`Models/Kitap.cs` | Hafta 11 ders notu bölüm 1.3 — Kitap.cs'te `public Kategori? Kategori { get; set; }`, burada `public List<Kitap> Kitaplar { get; set; } = [];`. Foreign key (KategoriId) zaten var; convention ikisini eşler |
| **TODO 11.2** | Kategoriler tablosunu ve ilişkiyi veritabanına taşıyın: Add-Migration KategoriIliskisi, sonra Update-Database | `Data/KampusKitapContext.cs` | Hafta 11 ders notu bölüm 1.4 — migration'ın Up()'ında CreateTable("Kategoriler"), InsertData (3 kategori), CreateIndex ve AddForeignKey görünür. Okumadan uygulamayın (Hafta 10, Klasik Tuzak) |
| **TODO 11.3** | Kitabı kategorisiyle birlikte getirin: Include | `Controllers/KitapController.cs` | Hafta 11 ders notu bölüm 1.5 — `await _db.Kitaplar.Include(k => k.Kategori).FirstOrDefaultAsync(k => k.Id == id)`; kategori adı `kitap.Kategori?.Ad`. Include'u silip deneyin: hata yok, kategori boş |
| **TODO 11.4** | Liste sorgusunu veritabanına taşıyın: IQueryable ile kurun, ToListAsync en sona | `Controllers/KitapController.cs` | Hafta 11 ders notu bölüm 2.3 — `IQueryable<Kitap> sorgu = _db.Kitaplar;` ile başlayın; Where/OrderBy/Skip/Take aynen kalır; sayım `await sorgu.CountAsync()`, sonuç `await sorgu.Skip(…).Take(…).ToListAsync()`. Konsoldaki SQL'de WHERE, ORDER BY ve LIMIT'i görün |
| **TODO 11.5** | İstatistiği veritabanında hesaplayın: toplama metotlarının Async hâlleri; gruplamayı kategori adına göre navigation üzerinden yapın | `Controllers/KitapController.cs` | Hafta 11 ders notu bölüm 2.4 — `await _db.Kitaplar.CountAsync()`, `SumAsync(k => k.Fiyat)` …; gruplama `GroupBy(k => k.Kategori!.Ad)` ve `Select(g => new KategoriOzetVM { … })`, sonda `ToListAsync()`. Konsolda GROUP BY ve INNER JOIN'i görün |
| **TODO 11.6** | Her kategorinin kitap sayısını veritabanında hesaplayın: navigation üzerinden Count | `Controllers/KategoriController.cs` | Hafta 11 ders notu bölüm 2.5 — `_db.Kategoriler.Select(k => new KategoriSatiriVM { Id = k.Id, Ad = k.Ad, Adet = k.Kitaplar.Count })`, sonda `ToListAsync()`. Include gerekmez: Select'in içindeki navigation SQL'e çevrilir |
| **TODO 11.7** | N+1'i düzeltin: kitapları kategorileriyle TEK sorguda getirin | `Controllers/KitapController.cs`<br>`Views/Kitap/Tuzak.cshtml` | Hafta 11 ders notu bölüm 3.1 — `await _db.Kitaplar.Include(k => k.Kategori).OrderBy(k => k.Id).ToListAsync()`. Konsolda 12 SELECT yerine tek bir SELECT … INNER JOIN görün |
<!-- GOREVLER-SON -->

### Bu hafta eklenenler

| Dosya | Ne için |
|---|---|
| `Models/Kategori.cs` | Yeni entity: `Kategoriler` tablosu; `Kitaplar` navigation'ı — TODO 11.1 |
| `Models/Kitap.cs` | `KategoriId` artık foreign key; `Kategori` navigation'ı — TODO 11.1 |
| `Data/KampusKitapContext.cs` | `DbSet<Kategori> Kategoriler`, seed: üç kategori — TODO 11.2 |
| `Data/KitapDeposu.cs` | **Silindi**: kategorilerin iki kopyası (sözlük ve `switch`) tabloya taşındı |
| `Migrations/…_KategoriIliskisi.cs` (yalnızca Bitmiş) | İkinci migration: `CreateTable`, `InsertData`, `CreateIndex`, `AddForeignKey` |
| `Controllers/KitapController.cs` | Bütün action'lar async; liste `IQueryable` ile veritabanında; detay `Include` ile — TODO 11.3 – 11.5, 11.7 |
| `Controllers/KategoriController.cs` | Kategori sayıları alt sorguyla — TODO 11.6 |
| `Views/Kitap/Tuzak.cshtml` | N+1 gösterisi ve düzeltmesi — TODO 11.7 |
| Formlar (`Index`, `EkleTH`, `Duzenle`, `Etiket`) | Kategori listesi artık veritabanından |

### Denemeye değer adresler

| Adres | Ne gösteriyor |
|---|---|
| `/Kitap/Detay/7` | `Include`: konsolda `INNER JOIN "Kategoriler"`; ekranda *Kategori: Edebiyat* |
| `/Kitap?kategori=1&sirala=fiyat&sayfa=2` | Konsolda `WHERE … ORDER BY … COLLATE EF_DECIMAL … LIMIT … OFFSET`; veritabanından tek satır |
| `/Kitap/Istatistik` | Her toplama ayrı bir küçük `SELECT`; gruplama `INNER JOIN … GROUP BY` |
| `/Kategori` | Tek sorgu, içinde alt sorgu: 5 · 3 · 3 |
| `/Kitap/Tuzak` | **12** sorgu (N+1) — sayfa doğru, hata yok |
| `/Kitap/Tuzak?duzelt=true` | **1** sorgu (`Include`) |
| `/Kitap` | Başlığa göre sıralı: *İnce Memed* nerede? (Klasik Tuzak) |

---

## 🧪 Kendiniz deneyin — kodu kasten bozun

Bunları **Bitmiş** projenin bir kopyasında deneyin.

| Deneme | Nerede | Beklenen |
|---|---|---|
| `.Include(k => k.Kategori)` satırını silin | `KitapController.Tuzak` | `/Kitap/Tuzak?duzelt=true`: 1 sorgu, JOIN yok; her satırda "(kategori gelmedi — Include yok)" — hata yok |
| `.Include(…)` ile `.Find(7)`'yi birleştirin | herhangi bir action | Derlenmez: *CS1061 … 'IIncludableQueryable<Kitap, Kategori>' does not contain a definition for 'Find'* |
| `Ara`'yı veritabanına taşıyın: `.Where(k => k.Baslik.Contains(kelime, StringComparison.CurrentCultureIgnoreCase))` | `KitapController.Ara` | **500**: *The LINQ expression … could not be translated* |
| Aynı aramayı `StringComparison`'sız yazın: `Contains("ince")` | `KitapController.Ara` | Hata yok, sonuç **boş**: SQLite `instr` büyük/küçük harf ayırır |
| Yeni bir kitabı `KategoriId = 99` ile kaydedin | kodda | *DbUpdateException* · *SQLite Error 19: 'FOREIGN KEY constraint failed'.* |

---

## 🏠 Ev görevi

0. **Başlangıç** projesinde **TODO 11.1 – 11.7**'yi tamamlayın; `KategoriIliskisi` migration'ını
   kendiniz üretin ve `Up()`'ındaki dört işlemi açıklayın.
1. Kendi projenize bir **bire-çok ilişki** ekleyin (navigation + foreign key + migration) ve ilişkili
   veriyi bir detay sayfasında `Include` ile gösterin.
2. Liste ekranınızı veritabanında süzülür, sıralanır ve sayfalanır yapın: `IQueryable`, sonuç en
   sonda `ToListAsync`. Konsoldaki SQL'in ekran görüntüsünü alın (`WHERE` ve `LIMIT` görünsün).
3. Projenizde bir N+1 bulun (ya da bilerek yazın), konsolda sorgu sayısını gösterin ve düzeltin.
4. Veritabanına giden bütün action'larınızı async yapın.

**Teslim:** proje klasörü (`*.db` hariç) + 2. ve 3. maddelerin ekran görüntüleri + şu soruya **iki
cümlelik** cevap: *"Bir sorgunun bellekte mi veritabanında mı çalıştığını nereden anlarsınız?"*

---

## 🔜 Gelecek hafta: Kimlik doğrulama ve yetkilendirme

Uygulamamız artık kalıcı veriyle, ilişkilerle ve veritabanında çalışan sorgularla çalışıyor. Ama
herkes her kitabı düzenleyip silebiliyor. Gelecek hafta: kullanıcı hesabı, giriş, roller — "bu
sayfayı yalnızca yönetici açabilir".

**Düşünerek gelin:** Hafta 7'de rolü cookie'de tutan sayfanın neden güvensiz olduğunu hatırlayın.
Giriş yapmış kullanıcının kim olduğunu sunucu nereden bilmeli?
