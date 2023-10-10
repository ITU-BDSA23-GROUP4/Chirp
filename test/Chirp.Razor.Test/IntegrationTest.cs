using CheepDB;
using CustomComparer;
using Repository;
using Xunit.Sdk;

public class IntegrationTest
{
    [Fact]
    public void TestGetCheeps() {
        CheepRepository repository = new CheepRepository();
        
        Assert.Equal(32, repository.GetCheeps(1).Count());
    }

    [Fact]
    public void TestGetCheepsByAuthor() {
        CheepRepository repository = new CheepRepository();
        List<CheepViewModel> expected = new List<CheepViewModel> { new CheepViewModel(656,"Helge", "Hello, BDSA students!", "2023-08-01 12:16:48")};

        Assert.True(Enumerable.SequenceEqual(expected, repository.GetCheepsFromAuthor("Helge", 1)));
    }
}