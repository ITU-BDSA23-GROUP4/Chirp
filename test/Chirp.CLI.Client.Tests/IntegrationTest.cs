using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Options;
using Xunit;
using Chirp.Razor.Pages;
using CheepRecord;
using System.Text.RegularExpressions;
public class TestAPI : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _fixture;
    private readonly HttpClient _client;

    public TestAPI(WebApplicationFactory<Program> fixture)
    {
        _fixture = fixture;
        _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
    }

    [Fact]
    public async void CanSeePublicTimeline()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains("Public Timeline", content);
    }

    [Theory]
    [InlineData("Helge")]
    [InlineData("Rasmus")]
    public async void CanSeePrivateTimeline(string author)
    {
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Contains("Chirp!", content);
        Assert.Contains($"{author}'s Timeline", content);
    }
    [Fact]
    public async void Check32Cheeps()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        //https://stackoverflow.com/questions/3016522/count-the-number-of-times-a-string-appears-within-a-string
        MatchCollection matches = Regex.Matches(content, "<li>");
        int count = matches.Count;
        Assert.Equal(32, count);
    }

    [Fact]
    public async void CheepsOnPage0IsTheSameAsPage1()
    {
        //Content from standard page: Localhost/
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        //Content from page 1
        var responseFromPageOne = await _client.GetAsync("/?page=1");
        responseFromPageOne.EnsureSuccessStatusCode();
        var contentFromPageOne = await responseFromPageOne.Content.ReadAsStringAsync();
        
        Assert.Contains(contentFromPageOne, content);
    }

    [Fact]
    public async void CheepsOnPage1IsNotTheSameAsPage2()
    {
        //Content from standard page 1
        var response = await _client.GetAsync("/?page=2");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        //Content from page 2
        var responseFromPageTwo = await _client.GetAsync("/?page=1");
        responseFromPageTwo.EnsureSuccessStatusCode();
        var contentFromPageTwo = await responseFromPageTwo.Content.ReadAsStringAsync();

        Assert.NotEqual(contentFromPageTwo, content);
    }

}