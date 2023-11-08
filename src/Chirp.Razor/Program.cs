using Initializer;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
/* The following code is only executed in a non-development enviroment. 
*  The 'dotnet publish' automatically changes the enviroment to 'Production'.
*  This is to prevent the test failing due to missing authentication when testing in GitHub,
*  and can be done, since the enviroment there, is still 'Developemnt'
*/
if (!app.Environment.IsDevelopment())
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "GitHub";
    })
    .AddCookie()
    .AddGitHub(o =>
    {   
        o.ClientId = NullCheck(builder.Configuration["authentication:github:clienId"]); // NullCheck raises an exception if the given string is null. Returns it if it's not.
        o.ClientSecret = NullCheck(builder.Configuration["authentication:github:clientSecret"]);
        o.CallbackPath = "/signin-github";
    });

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

static string NullCheck(string? stringToCheck) {
    if (stringToCheck == null) {
        throw new NullReferenceException();
    }
    return stringToCheck;
}

public partial class Program { }