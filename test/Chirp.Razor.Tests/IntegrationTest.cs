using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Text.RegularExpressions;
using Testcontainers.MsSql;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Initializer;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

public class TestAPI : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _fixture;
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();

    private readonly HttpClient _client;

    public TestAPI(WebApplicationFactory<Program> fixture)
    {
        
        _msSqlContainer.StartAsync().Wait();

        var connectionString = new SqlConnection(_msSqlContainer.GetConnectionString());
        connectionString.OpenAsync().Wait();

        _fixture = fixture.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ChirpDBContext>));

                services.Remove(dbContextDescriptor);

                var dbConnectionStringDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(string));

                services.Remove(dbConnectionStringDescriptor);

                services.AddSingleton(_msSqlContainer);

                services.AddDbContext<ChirpDBContext>(options =>
                {
                    options.UseSqlServer(_msSqlContainer.GetConnectionString());
                });

            });
        });
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
        // In the course removing the Token element from the pages as it is unique to all pages we used chatgpt to help out
        // It refined the method
        // Load the HTML into HtmlDocuments
        var htmlDoc = new HtmlAgilityPack.HtmlDocument();
        htmlDoc.LoadHtml(content);
        var htmlDocFromPageOne = new HtmlAgilityPack.HtmlDocument();
        htmlDocFromPageOne.LoadHtml(contentFromPageOne);

        // Select the nodes using XPath
        var node = htmlDoc.DocumentNode.SelectSingleNode("//input[@name='__RequestVerificationToken']");
        var nodeFromPageOne = htmlDocFromPageOne.DocumentNode.SelectSingleNode("//input[@name='__RequestVerificationToken']");

        // Remove the nodes
        if (node != null)

        {
            node.Remove();
        }
        if (nodeFromPageOne != null)
        {
            nodeFromPageOne.Remove();
        }

        // Get the cleaned HTML
        var cleanedHtml = htmlDoc.DocumentNode.OuterHtml;
        var cleanedHtmlFromPageOne = htmlDocFromPageOne.DocumentNode.OuterHtml;

        //Assert
        Assert.Contains(cleanedHtmlFromPageOne, cleanedHtml);
    }


    [Theory]
    [InlineData("/")]
    [InlineData("/Jacqualine Gilcoine/")]
    public async void CheepsOnPage1IsNotTheSameAsPage2(string path)
    {
        //Arange
        //Content from standard page 1
        var response = await _client.GetAsync(path + "?page=1");
        response.EnsureSuccessStatusCode();

        //Content from page 2
        var responseFromPageTwo = await _client.GetAsync(path + "?page=2");
        responseFromPageTwo.EnsureSuccessStatusCode();

        //Act 
        // page 1 
        var content = await response.Content.ReadAsStringAsync();
        // page 2
        var contentFromPageTwo = await responseFromPageTwo.Content.ReadAsStringAsync();

        //Assert
        Assert.NotEqual(contentFromPageTwo, content);
    }
    [Theory]
    [InlineData("/")]
    [InlineData("/Jacqualine Gilcoine/")]
    public async void CheepsAreInOrder(string path)
    {
        //Arrange
        var response = await _client.GetAsync(path);
        response.EnsureSuccessStatusCode();

        //Act
        var content = await response.Content.ReadAsStringAsync();
        MatchCollection matches = Regex.Matches(content, "2023-[0-1][0-9]-[03][0-9] [0-2][0-9]:[0-6][0-9]:[0-6][0-9]");
        Boolean InOrder = true;
        for (int i = 1; i < matches.Count; i++)
        {
            if (DateTime.Parse("" + matches[i - 1]).CompareTo(DateTime.Parse("" + matches[i])) == -1)
            {
                InOrder = false;
            }
        }
        //Assert 
        Assert.True(InOrder);
    }
}