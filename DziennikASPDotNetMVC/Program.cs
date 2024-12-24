using DziennikASPDotNetMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//£¹czenie z baz¹ danych
var connectionString = builder.Configuration.GetConnectionString("MyDbContext");
builder.Services.AddDbContext<MyDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Czas, po którym sesja wygaœnie
    options.Cookie.HttpOnly = true; // Ustawienie ciasteczka jako dostêpne tylko dla HTTP
    options.Cookie.IsEssential = true; // Ciastka s¹ niezbêdne do dzia³ania aplikacji
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Czas ¿ycia ciasteczka
    options.SlidingExpiration = true; // Ciasteczko jest odœwie¿ane przy ka¿dym ¿¹daniu
});

builder.Services.AddAuthentication("SessionAuthentication")
    .AddCookie("SessionAuthentication", options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.LoginPath = "/Home"; // Œcie¿ka do logowania, jeœli u¿ytkownik nie jest zalogowany
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
    options.AddPolicy("ParentOnly", policy => policy.RequireRole("parent"));
    options.AddPolicy("StudentOnly", policy => policy.RequireRole("student"));
    options.AddPolicy("TeacherOnly", policy => policy.RequireRole("teacher"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
