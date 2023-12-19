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
   */

    [Test]
    public static async Task Main()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();

        await page.GotoAsync("https://bdsagroup4chirprazor.azurewebsites.net/");

        await page.GetByRole(AriaRole.Button, new() { Name = "1", Exact = true }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "2", Exact = true }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Icon1Chirp!" }).ClickAsync();

        await page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "2", Exact = true }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

        await page.GotoAsync("https://bdsagroup4chirprazor.azurewebsites.net/");

        await page.GetByRole(AriaRole.Link, new() { Name = "Profile page" }).ClickAsync();

        await page.GetByText("Welcome to your profile page").ClickAsync();

        await page.GetByRole(AriaRole.Heading, new() { Name = "Following" }).ClickAsync();

        await page.GetByRole(AriaRole.Heading, new() { Name = "Privacy Policy" }).ClickAsync();

        await page.GetByRole(AriaRole.Heading, new() { Name = "Authentication Type:" }).ClickAsync();

        await page.GetByRole(AriaRole.Heading, new() { Name = "Claims:" }).ClickAsync();

        await page.GetByText("Deletion of Account When").ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "logout" }).ClickAsync();

        await page.GetByText("You have successfully signed").ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await page.GotoAsync("https://bdsagroup4chirprazor.azurewebsites.net/Helge");

        await page.GetByText("Hello, BDSA students!").ClickAsync();

        await page.GotoAsync("https://bdsagroup4chirprazor.azurewebsites.net/Rasmus");

        await page.GetByText("Hej, velkommen til kurset.").ClickAsync();

        await page.GotoAsync("https://bdsagroup4chirprazor.azurewebsites.net/Jacqualine%20Gilcoine");

        await page.GetByText("Starbuck now is what we hear").ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await page.Locator("p").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Link).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "12", Exact = true }).ClickAsync();

        await page.GetByText("Upon making known our desires").ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "1", Exact = true }).ClickAsync();

        await page.Locator("p").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Link).ClickAsync();

    }
}
