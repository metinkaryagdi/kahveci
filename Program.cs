using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Kahveci.Helpers; // SqlLogger sınıfını içe aktarın
using Kahveci.Models; // Models namespace'i

var builder = WebApplication.CreateBuilder(args);

// SQL sorgularını loglama
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging() // Parametre değerlerini gösterir
           .LogTo(message =>
           {
               // SQL sorgusunu log dosyasına yazdır
               SqlLogger.LogUniqueSqlCommand(message); // Sadece benzersiz SQL sorgularını logla
           }, LogLevel.Information); // SQL sorguları dosyasından loglama
});

// MVC desteği ekle
builder.Services.AddControllersWithViews();

// Session ekleme
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Development ortamı için hata sayfası
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware sırası
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Session kullanımı etkinleştiriliyor
app.UseAuthentication();
app.UseAuthorization();

// Route ayarları
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LoginSignup}/{action=Index}/{id?}");

app.Run();
