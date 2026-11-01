using System.Text.Json;

namespace KampusKitap.Services;

/// <summary>
/// HAFTA 7 — session'a NESNE yazmak (Hafta 7 ders notu bölüm 3.5).
///
/// ISession yalnızca üç tip bilir: byte[] (Set/TryGetValue), string
/// (SetString/GetString) ve int (SetInt32/GetInt32). "SetObject" diye bir metot
/// YOKTUR. Nesneyi önce JSON metnine çevirip string olarak saklarız; okurken
/// metni geri nesneye çeviririz.
///
/// Kullanımı:
///   HttpContext.Session.NesneYaz("Sepet", sepet);
///   var sepet = HttpContext.Session.NesneOku&lt;List&lt;SepetSatiri&gt;&gt;("Sepet") ?? [];
/// </summary>
public static class SessionUzantilari
{
    public static void NesneYaz<T>(this ISession session, string anahtar, T deger)
    {
        // ✔ TODO 7.5 · Nesneyi JSON'a çevirip session'a yazın; okurken geri çevirin
        session.SetString(anahtar, JsonSerializer.Serialize(deger));
    }

    public static T? NesneOku<T>(this ISession session, string anahtar)
    {
        // ✔ TODO 7.5 · Nesneyi JSON'a çevirip session'a yazın; okurken geri çevirin
        var json = session.GetString(anahtar);
        return json is null ? default : JsonSerializer.Deserialize<T>(json);
    }
}
