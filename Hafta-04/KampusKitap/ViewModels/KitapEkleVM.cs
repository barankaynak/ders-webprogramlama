namespace KampusKitap.ViewModels;

/// <summary>
/// HAFTA 4 — ViewModel (görünüm modeli), ders notu bölüm 3.4.
///
/// Formun alabileceği alanlar BURADA, açıkça yazılıdır. Entity'yi (Kitap)
/// doğrudan action parametresi yapsaydık kullanıcı formda olmayan alanları da
/// gönderebilirdi — over-posting (ders notu bölüm 3.3).
///
/// Dikkat: Kitap sınıfındaki Onayli özelliği burada YOK. Kullanıcı isteğe
/// Onayli=true eklese bile bağlanacak bir özellik bulunmadığı için düşer.
/// Entity'yi controller'da BİZ oluşturuyoruz ve Onayli'yı BİZ belirliyoruz.
/// </summary>
public class KitapEkleVM
{
    public string Baslik { get; set; } = "";
    public string Yazar { get; set; } = "";
    public decimal Fiyat { get; set; }
    public int KategoriId { get; set; }

    // Doğrulama öznitelikleri ([Required], [Range] …) HAFTA 6'nın konusu.
    // Bu hafta yalnızca bağlamanın başarısız olabileceğini ve bunun
    // ModelState'e yazıldığını biliyoruz (ders notu bölüm 2.7).
}
