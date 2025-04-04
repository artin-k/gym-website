using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MyBackend.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Add services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=users.db;Mode=ReadWriteCreate;Cache=Shared").EnableSensitiveDataLogging());


// 🔹 Add cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/users/login"; // Login route
        options.LogoutPath = "/api/auth/logout"; // Logout route
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Expiration
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Enable static files
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 Middleware order matters
app.UseRouting();
app.UseAuthentication();  // Add authentication middleware
app.UseAuthorization();   // Add authorization middleware

app.MapControllers(); // 🔹 Ensure controller routes are mapped correctly

app.Run();
