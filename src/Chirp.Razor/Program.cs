using Initializer;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Logging;

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
builder.Configuration.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).AddJsonFile($"appSettings.{builder.Environment.EnvironmentName}.json", optional: true);


using (var context = new ChirpDBContext())
{
    context.Database.EnsureCreated();
    DbInitializer.SeedDatabase(context);
}

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));
//builder.Services.AddRazorPages().AddMicrosoftIdentityUI();
builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
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
app.UseAuthorization();
app.UseAuthentication();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

public partial class Program { }