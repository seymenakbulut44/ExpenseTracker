using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;

var builder = WebApplication.CreateBuilder(args);

// servis kayıtları
var connectionString = GetConnectionString(builder);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
	options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// http istek hattı
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // varsayılan hsts 30 gün; üretimde ihtiyaca göre güncellenebilir
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// bağlantı bilgisini güvenli şekilde alır
static string GetConnectionString(WebApplicationBuilder builder)
{
    var conn = builder.Configuration.GetConnectionString("DefaultConnection");
    return conn ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}
