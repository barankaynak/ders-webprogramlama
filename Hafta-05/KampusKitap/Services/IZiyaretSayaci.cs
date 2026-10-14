namespace KampusKitap.Services;

/// <summary>
/// Ders notu bölüm 5 — Dependency Injection örneği.
/// Bu arayüz sayesinde controller, somut sınıfı tanımak zorunda kalmaz.
/// </summary>
public interface IZiyaretSayaci
{
    int Arttir();
    int Oku();
}
