using Chirp.Razor.Pages;
using SQLDB;
using CheepRecord;

namespace Chirp.Razor.Test;

public class UnitTest1
{
    [Fact]
    public void TestUnixTimeStampToDateTimeString()
    {
        string expected = "05/05/01 0:00:00";

        Assert.Equal(expected, CheepService.UnixTimeStampToDateTimeString(989020800));
    }

    [Fact]
    public void TestQuerySelectMessageIdOne() {
        DB db = DB.GetInstance();

        string expected = "0 10 They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me. 1690895677";
        string actual = "";

        using (var reader = db.Query(
            @"SELECT *
            FROM message
            LIMIT 1"
        )) {
            if (reader != null) {
                while (reader.Read()) {
                    actual = reader.GetString(0) + " " +
                        reader.GetString(1) + " " +
                        reader.GetString(2) + " " +
                        reader.GetString(3);
                }
            }
        }
        
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestGetCheeps() {

    }

    [Fact]
    public void TestGetCheepsByAuthor() {
        ICheepService service = new CheepService();
        List<CheepViewModel> expected = new List<CheepViewModel> { new CheepViewModel("Helge", "Hello, BDSA students!", "08/01/03 12:16:48")};

        //CollectionAssert.Equals(expected, service.GetCheepsFromAuthor(1, "Helge"));
        //Assert.Contains(service.GetCheepsFromAuthor(1,"Helge"));
    }
}