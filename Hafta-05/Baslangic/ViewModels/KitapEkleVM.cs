using System.ComponentModel.DataAnnotations;

namespace KampusKitap.ViewModels;

/// <summary>
/// HAFTA 4 — ViewModel (ViewModel), Hafta 4 ders notu bölüm 4.4.
///
/// Formun alabileceği alanlar BURADA, açıkça yazılıdır. Entity'yi (Kitap)
/// doğrudan action parametresi yapsaydık kullanıcı formda olmayan alanları da
/// gönderebilirdi — over-posting (Hafta 4 ders notu bölüm 4.3).
///
/// Dikkat: Kitap sınıfındaki Onayli özelliği burada YOK. Kullanıcı isteğe
/// Onayli=true eklese bile bağlanacak bir özellik bulunmadığı için düşer.
/// Entity'yi controller'da BİZ oluşturuyoruz ve Onayli'yı BİZ belirliyoruz.
/// </summary>
public class KitapEkleVM
{
    // HAFTA 5 — [Display] bir DOĞRULAMA attribute'ü değildir; yalnızca
    // <label asp-for> etiketinin yazacağı metni belirler.
    // TODO 5.5 · Dört özelliğe `[Display(Name = "…")]` ekleyin: Başlık, Yazar, Fiyat (TL), Kategori
    //      ipucu: ders notu bölüm 3.3 — `/Kitap/EkleTH`'deki etiketler "Baslik", "KategoriId" yerine Türkçe adları gösterir
    public string Baslik { get; set; } = "";

    public string Yazar { get; set; } = "";

    public decimal Fiyat { get; set; }

    public int KategoriId { get; set; }

    // Doğrulama attribute'leri ([Required], [Range] …) HAFTA 6'nın konusu.
    // Bu hafta yalnızca bağlamanın başarısız olabileceğini ve bunun
    // ModelState'e yazıldığını biliyoruz (Hafta 4 ders notu bölüm 3.7).
}
