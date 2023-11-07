using Initializer;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvc().AddRazorPagesOptions(options =>
{
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
    
});

builder.Services.AddDbContext<ChirpDBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChirpDB"));
});

builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<CheepRepository>();

var dbContext = services.GetRequiredService<ChirpDBContext>();
DbInitializer.SeedDatabase(dbContext);

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

app.UseAuthorization();

app.Run();

public partial class Program { }