using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

class Program
{
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

        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        await page.Locator("p").Filter(new() { HasText = "Jacqualine Gilcoine Starbuck" }).GetByRole(AriaRole.Link).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "8" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "Go to page" }).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Icon1Chirp!" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "21" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "20" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "21" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "20" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "19" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "12" }).ClickAsync();

        await page.GetByRole(AriaRole.Button, new() { Name = "1", Exact = true }).ClickAsync();

        await page.Locator("p").Filter(new() { HasText = "Lukan707 hej 2 — 12/05/2023 23:13:" }).GetByRole(AriaRole.Link).ClickAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Icon1Chirp!" }).ClickAsync();

    }
}
