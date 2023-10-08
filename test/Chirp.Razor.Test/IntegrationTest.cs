using CustomComparer;

public class IntegrationTest
{
    [Fact]
    public void TestGetCheeps() {
   
    }

    [Fact]
    public void TestGetCheepsByAuthor() {
        ICheepService service = new CheepService();
        List<CheepViewModel> expected = new List<CheepViewModel> { new CheepViewModel("Helge", "Hello, BDSA students!", "08/01/23 12:16:48")};

        Assert.True(Enumerable.SequenceEqual(expected, service.GetCheepsFromAuthor(1,"Helge"), new CheepViewModelComparer()));
    }
}