using Initializer;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc().AddRazorPagesOptions(options =>
{
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
    
});


var connectionString = $"Data source={Path.Combine(Path.GetTempPath() + "chirp.db")}";
builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString));

builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<CheepRepository>();

/* Lecture notes for reference for later work. 
builder.service.addScoped<ICheeprepository, CheepRepository()
Connections

In database add 
[StringLenghth(160. MinimumLength = 5)] -- NOT SUPPORTED IN SQLITE

dotnet ef migrations add limitCheepSize


builder.Services.AddDbContext<ChirpDBContext>(
    options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Chirp")));
builder.Services.AddScoped<ICheepRepository, CheepRepository>();


Lecture notes stops here */

//Seed data into database. 
// string DbPath;
// if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CHIRPDBPATH")))
//         {
//             DbPath = Path.GetTempPath() + "chirp.db";
//         }
//         else
//         {
//             DbPath = Environment.GetEnvironmentVariable("CHIRPDBPATH");
//         }
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