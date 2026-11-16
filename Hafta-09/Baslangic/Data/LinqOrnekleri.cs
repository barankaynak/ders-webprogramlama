using KampusKitap.Models;

namespace KampusKitap.Data;

// HAFTA 9 — aynı soru, iki yazım (Hafta 9 ders notu bölüm 1.1 – 1.2).
// Soru: "150 TL'den ucuz kitapların başlıkları hangileri?"
// /Sorgu sayfasının ilk iki kutusu bu iki metodun sonucunu gösterir.
public static class LinqOrnekleri
{
    // 1) foreach ile: NASIL yapılacağını adım adım yazıyoruz
    public static List<string> DonguIle(List<Kitap> kitaplar)
    {
        var ucuzlar = new List<string>();      // boş liste aç
        foreach (var k in kitaplar)             // kitapları tek tek gez
        {
            if (k.Fiyat < 150)                  // koşula bak
                ucuzlar.Add(k.Baslik);          // tutanı ekle
        }
        return ucuzlar;
    }

    // 2) LINQ ile: NE istediğimizi yazıyoruz
    public static List<string> LinqIle(List<Kitap> kitaplar)
    {
        return kitaplar
            .Where(k => k.Fiyat < 150)          // koşul
            .Select(k => k.Baslik)              // ne alınacak
            .ToList();                          // sonucu listeye çevir
    }
}
