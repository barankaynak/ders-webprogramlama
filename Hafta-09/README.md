# Hafta 9 — LINQ: Koleksiyonları Sorgulamak

| | |
|---|---|
| 📄 **Ders notu** | [H09-Ders-Notu.pdf](../PDF/H09-Ders-Notu.pdf) — *sınav kapsamı* |
| 🔧 **Uygulama** | [H09-Uygulama.pdf](../PDF/H09-Uygulama.pdf) — derste yapılanların dökümü |
| 📚 **Detay** | [H09-Detay.pdf](../PDF/H09-Detay.pdf) — *sınav kapsamı DIŞI* |

---

## Bu haftanın konusu

- **Döngüden sorguya** — aynı `foreach`'i tek satırlık bir LINQ sorgusuyla yazmak; projede Hafta 3'ten beri duran `FirstOrDefault`, `Where`, `Max`, `Any`
- **Dil zemini** — lambda, `Func<T, bool>`, extension method, `IEnumerable<T>`, anonim tip; query syntax ve method syntax
- **Süzmek, sıralamak, sayfalamak** — `Where`, `Select`, `OrderBy`/`ThenBy`, `Skip`/`Take`; sorguyu adım adım kurmak
- **Tek eleman ve evet/hayır** — `First`, `FirstOrDefault`, `Single`, `Any`, `All`, `Contains`; hangisi ne zaman istisna fırlatır
- **Özetlemek** — `Count`, `Sum`, `Average`, `Min`, `Max`, `GroupBy`, `Distinct`
- **Sorgu ne zaman çalışır** — deferred execution ve `ToList()`

🧩 **Haftanın ilkesi — Ne istediğini söyle, nasıl yapılacağını değil.**
Bir döngü bilgisayara nasıl yapacağını anlatır; bir LINQ sorgusu ne istediğinizi söyler.

> **Haftanın tek cümlelik özeti:** Bu hafta veri hâlâ bellekte; sorgu yazmayı burada
> öğreniyoruz ki Hafta 10'da veritabanına geçtiğimizde, Hafta 11'de aynı sorguları
> değiştirmeden veritabanına yazabilelim.

---

## İki proje: Başlangıç ve Bitmiş

| Klasör | Ne | Port |
|---|---|---|
| **`Baslangic/`** | **Buradan başlayın.** Haftanın konusu olan yerler boş bırakıldı ve **`TODO 9.x`** ile işaretlendi. Proje derlenir ve çalışır; boşken liste süzülmez ve sıralanmaz, `/Kitap/Istatistik` sıfır gösterir, `/Kategori` her kategoride "0 kitap" der — bilerek. | `5072` |
| **`Bitmis/`** | Aynı projenin tamamlanmış hâli. Doldurulmuş her yerin üstünde **`✔ TODO 9.x`** izi var — takıldığınızda ya da bitirince karşılaştırmak için. | `5073` |

```bash
cd Baslangic        # önce bu
dotnet run          # → http://localhost:5072

cd Bitmis           # karşılaştırmak için, ayrı bir terminalde
dotnet run          # → http://localhost:5073
```

> ⚠️ **Önce Bitmiş'e bakmayın.** Boşluğu kendiniz doldurduğunuzda neyin değiştiğini
> görürsünüz; hazır çözümü okuduğunuzda yalnızca sonucu görürsünüz.
>
> 💡 Bu hafta **veritabanı yok** ve kurulacak bir paket yok. Bütün veri `Data/KitapDeposu.cs`'teki
> on bir kitap; uygulamayı yeniden başlatınca eklediğiniz kitaplar kaybolur.

### Doldurulacak yerler

Editörde **`TODO 9.`** diye aratın. Her görevden sonra ilgili sayfayı yenileyin; parametreleri
adres çubuğundan değiştirerek deneyin (`/Kitap?kategori=3&sirala=fiyat-azalan&sayfa=2`).

<!-- GOREVLER-BAS -->
| Görev | Ne yapılacak | Dosya | Doldurunca ne görülür |
|---|---|---|---|
| **TODO 9.1** | Listeyi seçilen kategoriye göre süzün | `Controllers/KitapController.cs` | Hafta 9 ders notu bölüm 3.1 — `sorgu = sorgu.Where(k => …)`; kategori seçilmediyse (null) sorguya dokunmayın. Deneyin: /Kitap?kategori=3 |
| **TODO 9.2** | Listeyi sıralayın: "fiyat" artan, "fiyat-azalan" azalan, ikisi de değilse başlığa göre; fiyatı eşit olanlar başlığa göre | `Controllers/KitapController.cs` | Hafta 9 ders notu bölüm 3.3 — `OrderBy`, `OrderByDescending`, ikinci ölçüt için `ThenBy`. `switch` ifadesi Hafta 3'teki `KategoriAdi`'nda var |
| **TODO 9.3** | Sayfa başına 4 kitap gösterin: toplam sayfa sayısını hesaplayın, istenen sayfanın kitaplarını alın | `Controllers/KitapController.cs` | Hafta 9 ders notu bölüm 3.5 — önce `Count()`, sonra `Skip((sayfa - 1) * 4).Take(4)`. Sayfa 0 ya da 99 gelirse `Math.Clamp` ile sınırlara çekin |
| **TODO 9.4** | Aynı kategoriden, bu kitabın kendisi hariç, en ucuz üç kitabı önerin | `Controllers/KitapController.cs` | Hafta 9 ders notu bölüm 3.6 — `Where` (iki koşul, `&&`), `OrderBy`, `Take(3)`. Sonuna `ToList()`: view `List<Kitap>` bekliyor; view'a tarif değil sonuç gider |
| **TODO 9.5** | Özet sayıları hesaplayın: kitap sayısı, toplam/ortalama/en düşük/en yüksek fiyat, 150 TL altı kitap sayısı, 500 TL üstü kitap var mı, hepsi onaylı mı | `Controllers/KitapController.cs` | Hafta 9 ders notu bölüm 5.1 ve 4.3 — `Count()`, `Sum(k => k.Fiyat)`, `Average`, `Min`, `Max`; koşullu sayım `Count(k => …)`; evet/hayır için `Any` ve `All` |
| **TODO 9.6** | Kitapları kategoriye göre gruplayın; her grup için kategori adı, kitap sayısı ve ortalama fiyat; en kalabalık kategori üstte | `Controllers/KitapController.cs` | Hafta 9 ders notu bölüm 5.2 — `GroupBy(k => k.KategoriId)`; her grup `g.Key` ve kitaplarıyla gelir. `Select(g => new KategoriOzetVM { … })` içinde `g.Count()`, `g.Average(…)`; adı `KitapDeposu.KategoriAdi(g.Key)` verir |
| **TODO 9.7** | Yazar adlarını tekrarsız ve alfabetik sırayla listeleyin | `Controllers/KitapController.cs` | Hafta 9 ders notu bölüm 5.3 — önce `Select(k => k.Yazar)` ile kitaptan yazara geçin, sonra `Distinct()`, `OrderBy(y => y)`, `ToList()` |
| **TODO 9.8** | Her kategorinin yanında o kategorideki kitap sayısı yazsın | `Controllers/KategoriController.cs` | Hafta 9 ders notu bölüm 5.4 — koşullu sayım: `KitapDeposu.Hepsi.Count(k => …)`. İçteki lambda dıştaki lambda'nın `kv`'sini görür; kategorinin numarası `kv.Key` |
| **TODO 9.9** | Bu kategorideki kitapları başlığa göre sıralı listeleyin | `Controllers/KategoriController.cs` | Hafta 9 ders notu bölüm 5.4 — `Where`, `OrderBy`, `ToList()`. /Kitap?kategori=2 ile aynı süzgeç; burada sayfalama yok |
<!-- GOREVLER-SON -->

### Bu hafta eklenenler

| Dosya | Ne için |
|---|---|
| `Data/KitapDeposu.cs` | Beş kitap on bire çıktı; `Ara` Türkçe harfleri doğru eşliyor (`CurrentCultureIgnoreCase`) |
| `Controllers/KitapController.cs`, `ViewModels/KitapListeVM.cs` | `/Kitap`: süzgeç, sıralama, sayfalama — TODO 9.1 – 9.3; detayda benzer kitaplar — TODO 9.4 |
| `Views/Kitap/Istatistik.cshtml`, `ViewModels/IstatistikVM.cs` | Toplama metotları, `GroupBy`, `Distinct` — TODO 9.5 – 9.7 |
| `Controllers/KategoriController.cs`, `ViewModels/KategoriSatiriVM.cs` | Kategori sayfaları tek kaynaktan; kitap sayıları ve kategori listesi — TODO 9.8, 9.9 |
| `Controllers/SorguController.cs`, `Views/Sorgu/` | LINQ laboratuvarı; Hata Köşesi ve Klasik Tuzak sayfaları |

### Denemeye değer adresler

| Adres | Ne gösteriyor |
|---|---|
| `/Sorgu` | LINQ laboratuvarı: her kutuda kod ve o kodun ürettiği sonuç |
| `/Kitap?kategori=1&sirala=fiyat-azalan` | Süzgeç ve sıralama birlikte |
| `/Kitap?sayfa=2` · `/Kitap?sayfa=99` | Sayfalama; sınır dışı sayfa son sayfaya çekilir |
| `/Kitap/Detay/1` | "Aynı kategoriden": `Where` + `OrderBy` + `Take(3)` |
| `/Kitap/Istatistik` | `Count`, `Sum`, `Average`, `Min`, `Max`, `Any`, `All`, `GroupBy`, `Distinct` |
| `/Kategori` · `/Kategori/Detay/2` | Koşullu sayım ve `Where` başka bir sayfada |
| `/Kitap/Ara?kelime=ince` | Türkçe harfli arama: *İnce Memed* |
| `/Sorgu/Bos?yazar=Tolstoy` | 🐛 Hata Köşesi: `First` bulamayınca |
| `/Sorgu/Tuzak` · `/Sorgu/Tuzak?duzelt=true` | 🕳️ Klasik Tuzak: indirim işaretleri nereye gitti? |

---

## 🧪 Kendiniz deneyin — kodu kasten bozun

Bunları **Bitmiş** projede deneyin.

| Deneme | Nerede | Beklenen |
|---|---|---|
| TODO 9.4'teki `.ToList()`'i silin | `KitapController.Detay` | "Aynı kategoriden" bölümü **kaybolur** — hata yok |
| `ThenBy`'ı `OrderBy` yapın, `?sirala=fiyat` | `KitapController.Index` | Fiyat sırası bozulur: liste yalnızca başlığa göre sıralanır |
| `Skip((vm.Sayfa - 1) * …)`'yi `Skip(vm.Sayfa * …)` yapın | `KitapController.Index` | 1. sayfa ilk dört kitabı atlar; son sayfa boş |
| `Average`'ı `Where(k => k.Fiyat > 1000)` sonrasına uygulayın | `KitapController.Istatistik` | **500**: *Sequence contains no elements* |
| `First`'ü `Single` yapın, `/Sorgu/Bos?yazar=Dostoyevski` | `SorguController.Bos` | **500**: *Sequence contains more than one matching element* |
| `Ara`'daki `CurrentCultureIgnoreCase`'i `OrdinalIgnoreCase` yapın, `/Kitap/Ara?kelime=ince` | `Data/KitapDeposu.cs` | "sonuç yok" |

---

## 🏠 Ev görevi

0. **Başlangıç** projesinde **TODO 9.1 – 9.9**'u tamamlayın.
1. `H09-Uygulama.pdf`'in sonundaki **sekiz LINQ alıştırmasını** method syntax ile yazın;
   üçünü query syntax ile de yazın.
2. Kendi projenizin **listeleme ekranını** süzülebilir, sıralanabilir (`ThenBy` ile iki ölçüt) ve
   sayfalanabilir yapın; parametreler query string'de dursun.
3. Projenize `GroupBy` kullanan bir **özet sayfası** ekleyin.

**Teslim:** proje klasörü + alıştırmaların kodu ve çıktıları + şu soruya **iki cümlelik**
cevap: *"Listeleme ekranınızda `ToList()`'i neden en sona yazdınız?"*

---

## 🔜 Gelecek hafta: ORM ve Entity Framework Core

Bu hafta bütün sorgular bellekteki on bir kitap üzerinde çalıştı ve eklediğiniz kitaplar
yeniden başlatınca kayboluyor. Gelecek hafta ilk kez gerçek bir **veritabanı** kuruyoruz:
verinin tablolarda nasıl durduğu, bir C# sınıfının bir tabloya nasıl eşlendiği (ORM) ve
Entity Framework Core.

**Düşünerek gelin:** SQL'in `SELECT … FROM … WHERE … ORDER BY` kalıbını hatırlayın. Bu haftanın
`Where`, `Select` ve `OrderBy`'ı hangisine karşılık geliyor?
