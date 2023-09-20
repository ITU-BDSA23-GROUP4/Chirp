using Xunit;
using CheepNS;
using System.Globalization;
namespace Chirp.CLI.Client.Tests;
public class UnitTest1
{
    [Fact]
    public void UnitTest_CheepAuther(){
        //Arange
        long timeStamp = 123;
        var testAuthor = new Cheep { 
            Author ="TestAuthor",
            Message= "Test", 
            Timestamp =timeStamp };
        string comparison = "TestAuthor";
        //Act
        //Assert
        Assert.Equal(comparison, testAuthor.Author);

    }
    [Fact]
    public void UnitTest_CheepTimestamp()
    {
        //Arange
        long timeStamp = 123;
        var testTimeStamp = new Cheep{ 
            Author ="TestAuthor",
            Message= "Test", 
            Timestamp =timeStamp };
        long comparison = 123;
        //Act
        //Assert
        Assert.Equal(comparison, testTimeStamp.Timestamp);

    }
    [Fact]
    public void UnitTest_CheepMessege()
    {
        //Arange
        long timeStamp = 123;
        var testMessege = new Cheep { 
            Author ="TestAuthor",
            Message= "Test", 
            Timestamp =timeStamp };
        string comparison = "Test";
        //Act
        //Assert
        Assert.Equal(comparison, testMessege.Message);
    }
    [Fact]
    public void UnitTest_CheepToString()
    {
        //Arange
        long timeStamp = 123;
        var testToString = new Cheep { 
            Author ="TestAuthor",
            Message= "Test", 
            Timestamp =timeStamp };
        DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(timeStamp);
        string comparison = $"TestAuthor @ {time.ToLocalTime().ToString("G", new CultureInfo("sw-SW"))}: Test";
        //Act
        //Assert
        Assert.Equal(comparison, testToString.ToString());
    }
    
    [Fact]
    public void UnitTest_ConstructCheep(){
        //Arrange
        string input = "User Input";
        Cheep comparison = new Cheep{ 
            Author=Environment.UserName,
            Message="User Input",
            Timestamp= DateTimeOffset.Now.ToUnixTimeSeconds()}; 
        //Act 
        Cheep TestCheep = CLINS.CLI.ConstructCheep(input);

        //Asset
        Assert.Equal(comparison,TestCheep);
    }
}