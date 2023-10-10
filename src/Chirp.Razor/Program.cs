using Chirp.Razor.Pages;
using CheepDB;
using Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

new CheepRepository().InitDB();

// Add services to the container.
builder.Services.AddSingleton<ICheepService, CheepService>();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ChirpDBContext>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ChirpDBContext>();
        db.Database.Migrate();
    }

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();

public partial class Program { }
