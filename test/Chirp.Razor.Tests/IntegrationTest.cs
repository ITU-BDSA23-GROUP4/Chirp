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
    
    [Theory]
    [InlineData("/")]
    [InlineData("/Jacqualine Gilcoine/")]
    public async void Check32Cheeps(string path)
    {
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        //https://stackoverflow.com/questions/3016522/count-the-number-of-times-a-string-appears-within-a-string
        MatchCollection matches = Regex.Matches(content, "<li>");
        int count = matches.Count;
        Assert.Equal(32, count);
    }
    // [Theory]
    // [InlineData("/")]
    // [InlineData("/Jacqualine Gilcoine/")]
    // public async void Check32CheepsOnPage2(string path)
    // {
    //     //arange 
    //     var response = await _client.GetAsync(path+"?page=2");
    //     response.EnsureSuccessStatusCode();
    //     //Act
    //     var content = await response.Content.ReadAsStringAsync();
    //     MatchCollection matches = Regex.Matches(content, "<li>");
    //     int count = matches.Count;
    //     //Assert
    //     Assert.Equal(32, count);
    // }
    
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
    [Theory]
    [InlineData("/")]
    [InlineData("/Jacqualine Gilcoine/")]
    public async void CheepsOnPage1IsNotTheSameAsPage2(string path)
    {
        //Content from standard page 1
        var response = await _client.GetAsync(path+"?page=2");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        //Content from page 2
        var responseFromPageTwo = await _client.GetAsync(path+"?page=1");
        responseFromPageTwo.EnsureSuccessStatusCode();
        var contentFromPageTwo = await responseFromPageTwo.Content.ReadAsStringAsync();

        Assert.NotEqual(contentFromPageTwo, content);
    }

  
   
    [Fact]
    public async void CheepsOnPage1ITheSameAsRootAuthor(){
        //Arrange  
        //Root
        var response_Root = await _client.GetAsync("/Jacqualine Gilcoine/");
        response_Root.EnsureSuccessStatusCode();
        //Page 1
        var response_Page1 = await _client.GetAsync("/Jacqualine Gilcoine/?page=1");
        response_Page1.EnsureSuccessStatusCode();
        //Act 
        var content_Root = await response_Root.Content.ReadAsStringAsync();
        var content_Page1 = await response_Page1.Content.ReadAsStringAsync();
        //Assert
        
        Assert.Contains(content_Root, content_Page1);
    }
}
