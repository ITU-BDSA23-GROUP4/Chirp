using Chirp.Razor.Pages;
using CheepDB;
using Repository;
using Initializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ICheepService, CheepService>();

// Seed data into database. Is it correct to have this code here?
using (var context = new ChirpDBContext())
{
    context.Database.EnsureCreated();
    DbInitializer.SeedDatabase(context);
}

var app = builder.Build();

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
