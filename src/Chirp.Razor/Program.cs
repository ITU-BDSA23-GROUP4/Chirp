using Initializer;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

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

using (var context = new ChirpDBContext())
{
    context.Database.EnsureCreated();
    DbInitializer.SeedDatabase(context);
}

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureADB2C"));
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else 
{
    app.UseCookiePolicy(new CookiePolicyOptions() {
        MinimumSameSitePolicy = SameSiteMode.None,
        Secure = CookieSecurePolicy.Always
    });

}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Add the Microsoft Identity Web cookie policy
app.UseCookiePolicy();
app.UseRouting();
// Add the ASP.NET Core authentication service
app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

public partial class Program { }