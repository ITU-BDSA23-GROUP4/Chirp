using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
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
        //Arrange 
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();

        //Act 
        var content = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Contains("Chirp!", content);
        Assert.Contains("Public Timeline", content);
    }

    [Theory]
    [InlineData("Helge")]
    [InlineData("Rasmus")]
    public async void CanSeePrivateTimeline(string author)
    {
        //Arrange
        var response = await _client.GetAsync($"/{author}");
        response.EnsureSuccessStatusCode();

        //Act 
        var content = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Contains("Chirp!", content);
        Assert.Contains($"{author}'s Timeline", content);
    }
    
    [Theory]
    [InlineData("/")]
    [InlineData("/?page=2")]
    [InlineData("/Jacqualine Gilcoine/")]
    [InlineData("/Jacqualine Gilcoine/?page=2")]
    public async void Check32Cheeps(string path)
    {
        //Arrange
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();

        //Act
        var content = await response.Content.ReadAsStringAsync();
        //https://stackoverflow.com/questions/3016522/count-the-number-of-times-a-string-appears-within-a-string
        MatchCollection matches = Regex.Matches(content, "<li>");
        int count = matches.Count;

        //Assert 
        Assert.Equal(32, count);
    }
    [Theory]
    [InlineData("/")]
    [InlineData("/Jacqualine Gilcoine/")]
    public async void CheepsOnPage0IsTheSameAsPage1(string path)
    {
        //Assert
        //Content from standard page: Localhost/
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        //Content from page 1
        var responseFromPageOne = await _client.GetAsync(path + "?page=1");
        responseFromPageOne.EnsureSuccessStatusCode();

        //Act
        var content = await response.Content.ReadAsStringAsync();
        var contentFromPageOne = await responseFromPageOne.Content.ReadAsStringAsync();

        //Assert
        Assert.Contains(contentFromPageOne, content);
    }
    [Theory]
    [InlineData("/")]
    [InlineData("/Jacqualine Gilcoine/")]
    public async void CheepsOnPage1IsNotTheSameAsPage2(string path)
    {
        //Arange
        //Content from standard page 1
        var response = await _client.GetAsync(path+"?page=1");
        response.EnsureSuccessStatusCode();

        //Content from page 2
        var responseFromPageTwo = await _client.GetAsync(path+"?page=2");
        responseFromPageTwo.EnsureSuccessStatusCode();

        //Act 
        // page 1 
        var content = await response.Content.ReadAsStringAsync();
        // page 2
        var contentFromPageTwo = await responseFromPageTwo.Content.ReadAsStringAsync();

        //Assert
        Assert.NotEqual(contentFromPageTwo, content);
    }
}


