using KampusKitap.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------
// 2. EVRE — servis kaydı
// Bu satırlar builder.Build()'dan ÖNCE olmak ZORUNDA.
// Sonrasına yazarsanız derlenir ama hiçbir etkisi olmaz (ders notu §5.2).
// ---------------------------------------------------------------
builder.Services.AddControllersWithViews();

// Yaşam döngüsünü değiştirip farkı gözleyin (ders notu §5.3):
//   AddSingleton → sayaç artarak gider   (1, 2, 3, ...)
//   AddScoped    → her istekte 1'den başlar
//   AddTransient → her enjeksiyonda yeni nesne
builder.Services.AddSingleton<IZiyaretSayaci, ZiyaretSayaci>();

var app = builder.Build();

// ---------------------------------------------------------------
// 4. EVRE — Middleware Pipeline
// SIRA ÖNEMLİDİR. Bozmak çoğu zaman hata vermez; sessizce yanlış çalışır.
// ---------------------------------------------------------------

// Günlükleyen middleware (ders notu §4.4).
// next() çağrısını yorum satırı yapıp short-circuit'i gözleyin:
// sayfa bomboş gelir, durum kodu 200 olur, HATA ALMAZSINIZ.
app.Use(async (context, next) =>
{
    Console.WriteLine($"→ GİRİŞ: {context.Request.Method} {context.Request.Path}");
    await next();
    Console.WriteLine($"← ÇIKIŞ: {context.Response.StatusCode}");
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// DOĞRU SIRA: Routing → (Authentication) → Authorization
// UseAuthorization'ı UseRouting'den ÖNCE koyarsanız uygulama çalışmaya devam eder,
// ama yetkilendirme hedefi bilmediği için Hafta 11'de koruma çalışmaz.
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

// {id:int?} — route kısıtı (ders notu §3.3).
// Kısıt sayesinde /Kitap/Detay/abc bu route'a EŞLEŞMEZ ve temiz bir 404 alırsınız.
// Kısıtı kaldırıp deneyin: "abc" değeri int'e çevrilemediği için farklı bir hata çıkar.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id:int?}")
    .WithStaticAssets();

app.Run();
