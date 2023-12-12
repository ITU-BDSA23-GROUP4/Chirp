using Initializer;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Chirp.Core;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Logging;
using FluentValidation;

/*
<Summary>
This is the program file
This is where we configure our services and the database.
This is the main entry point of the program, that is, 
to run the program you have to be in this folder.
</Summary>
*/

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();

builder.Configuration.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true).AddJsonFile($"appSettings.{builder.Environment.EnvironmentName}.json", optional: true);


builder.Services.AddDbContext<ChirpDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChirpDB"));
});


builder.Services.AddScoped<AbstractValidator<CheepCreateDTO>, CheepCreateValidator>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<ICheepService, CheepService>();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ChirpDBContext>();

    // We need to migrate everytime the program starts because of the razor tests
    // will fail otherwise. But azure crashes on deployment if we run migrations
    // when the database already exists. Therefore this step is introduced.
    if(app.Environment.IsDevelopment()){
        dbContext.Database.Migrate();
    }
    
    if (!dbContext.Authors.Any())
    {
        if(!app.Environment.IsDevelopment()){
            dbContext.Database.Migrate();  
        }
        DbInitializer.SeedDatabase(dbContext);
    }
    
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseCookiePolicy(new CookiePolicyOptions()
    {
        MinimumSameSitePolicy = SameSiteMode.None,
        Secure = CookieSecurePolicy.Always
    });

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();

public partial class Program { }