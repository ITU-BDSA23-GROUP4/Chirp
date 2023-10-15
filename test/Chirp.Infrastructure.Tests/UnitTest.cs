using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

public class UnitTest
{
    [Fact]
    public void TestRestrictedCreationOfCheep(){
        //Arrange
        //Runs in memory
        using var connection = new SqliteConnection("Filename=:memory:");
        var builder = new DbContextOptionsBuilder<ChirpDBContext>();
        builder.UseSqlite(connection);
        var context = new ChirpDBContext(builder.Options);
        context.Database.EnsureCreated();
        var repository = new CheepRepository(context);
        connection.Open();

        //Act
        CheepDTO cheep = new()
        {
            AuthorId = 3,
            Author = "fjkd",
            Timestamp = DateTime.Now.ToString(),
            Message = "This string should be way over 160 characters, just so we can check that its not possible to make a message that is longer that nessesary.This will because of that, become a very long message."
        };

        Action act = () => repository.AddCheep(cheep.AuthorId, cheep.Message);

        //Assert
        act.Should().Throw<Exception>().WithMessage("Message is too long");
    }





    [Fact]
    public void TestQuery() {
        // string exptected = "0 10 They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me. 1690895677";
        // string actual = "";
        // DB db = DB.GetInstance();
        
        // using (var reader = db.Query(
        //     "SELECT * FROM message LIMIT 1"
        // )) {
        //     if (reader != null) {
        //         while(reader.Read()) {
        //             actual = reader.GetString(0) + " " +
        //                 reader.GetString(1) + " " +
        //                 reader.GetString(2) + " " +
        //                 reader.GetString(3);
        //         }
        //     }
        // }

        // Assert.Equal(exptected, actual);

    }

    [Fact]
    public void TestUnixTimeStampToDateTimeString()
    {
        // string expected = "05/05/01 0:00:00";

        // Assert.Equal(expected, CheepService.UnixTimeStampToDateTimeString(989020800));
    }
}