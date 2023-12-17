using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using NUnit.Framework;

class Program
{

    /* RUN THIS LINE TO INSTALL 
    pwsh bin/Debug/net7.0/playwright.ps1 install 
    The playwrighttest tries to access features that are only available to authenticated users
    for now the developer has to manually login when running the test. Simply run dotnet run, and
    a private firefox window will open and playwright will direct the developer to login wherein you
    enter your credentials and the test will run as normal. your credentials are NOT SAVED anywhere
    insert your github username into the variable at line 18*/

    [Test]
    public static async Task Main()
    {
        string username = "Faberen";
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://bdsagroup4chirprazor.azurewebsites.net/");

        await page.GetByRole(AriaRole.Link, new() { Name = "Icon1Chirp!" }).ClickAsync();

        await page.Locator("div").Filter(new() { HasText = "public timeline | login |" }).Nth(1).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();

        await page.Locator("div").Filter(new() { HasText = "my timeline | public timeline" }).Nth(1).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await page.Locator("form").ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "1", Exact = true }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "21" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "20" }).ClickAsync();

        await page.GotoAsync("https://bdsagroup4chirprazor.azurewebsites.net/Helge");

        await page.GetByText("Helge Hello, BDSA students").ClickAsync();

        await page.GetByRole(AriaRole.Heading, new() { Name = "Helge's Timeline" }).ClickAsync();

        await page.GotoAsync("https://bdsagroup4chirprazor.azurewebsites.net/Rasmus");

        await page.GetByRole(AriaRole.Heading, new() { Name = "Rasmus's Timeline" }).ClickAsync();

        await page.GetByText("Rasmus Hej, velkommen til").ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Profile page" }).ClickAsync();

        await page.GetByText("Welcome to your profile page").ClickAsync();

        await page.GetByRole(AriaRole.Heading, new() { Name = "Privacy policy" }).ClickAsync();

        await page.GetByRole(AriaRole.Heading, new() { Name = "Authentication Type:" }).ClickAsync();

        await page.GotoAsync("https://bdsagroup4chirprazor.azurewebsites.net/");

        await page.GetByRole(AriaRole.Link, new() { Name = "Icon1Chirp!" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "logout ["+username+"]" }).ClickAsync();

        await page.GetByRole(AriaRole.Heading, new() { Name = "Signed out" }).ClickAsync();

        await page.GetByText("You have successfully signed").ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Icon1Chirp!" }).ClickAsync();

        await page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" }).ClickAsync();

    }
}
