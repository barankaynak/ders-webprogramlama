namespace KampusKitap.Services;

public class ZiyaretSayaci : IZiyaretSayaci
{
    private int _sayi;

    public int Arttir() => ++_sayi;
    public int Oku() => _sayi;
}
