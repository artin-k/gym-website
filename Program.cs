using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using MySqlConnector;
using gymWebsite.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add controllers and views
builder.Services.AddControllersWithViews();

// ✅ Configure MySQL
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// ✅ Swagger + API tools
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Configure cookie auth to use actual static login page
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login.html";   // 🔥 serve login.html from wwwroot
        options.LogoutPath = "/logout";
    });

var app = builder.Build();

// ✅ Static files from wwwroot
app.UseStaticFiles();

// ✅ Error page in dev
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// ✅ Middleware pipeline (order matters)
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();  // 🔥 Must come before Authorization
app.UseAuthorization();

// ✅ Default controller route (for API + MVC)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
