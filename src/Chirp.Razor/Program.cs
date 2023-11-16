using Initializer;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Environment.EnvironmentName = "Development";

var connectionString = $"Data source={Path.Combine(Path.GetTempPath() + "chirp.db")}";
builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString));

builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<CheepRepository>();
// builder.Configuration.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
//     .AddJsonFile($"appSettings.{builder.Environment.EnvironmentName}.json", optional: true);


using (var context = new ChirpDBContext())
{
    context.Database.EnsureCreated();
    DbInitializer.SeedDatabase(context);
}

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
    app.UseExceptionHandler("/Error");
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

app.UseRouting();
// Add the ASP.NET Core authentication service
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

public partial class Program { }