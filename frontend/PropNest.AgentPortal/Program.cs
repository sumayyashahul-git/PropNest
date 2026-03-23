var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("AuthService", c =>
    c.BaseAddress = new Uri("http://localhost:5002"));

builder.Services.AddHttpClient("PropertyService", c =>
    c.BaseAddress = new Uri("http://localhost:5001"));

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", opt =>
    {
        opt.LoginPath = "/Account/Login";
        opt.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddSession();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();