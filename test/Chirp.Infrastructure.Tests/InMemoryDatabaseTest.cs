using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
public class InMemoryDatabaseTest
{   
    //Each of these needs their own in memory database, since they can corrupt eachother

    [Fact] //Check if the memory database is the same as the current database
    public void MemoryDatabaseShouldNotBeEmpty()
    {
        //Arrange
        //Creates a database in memory
        using var connection = new SqliteConnection("Filename=:memory:");
        var builder = new DbContextOptionsBuilder<ChirpDBContext>();
        builder.UseSqlite(connection);
        ChirpDBContext context = new(builder.Options);
        context.Database.EnsureCreated();
        CheepRepository repository = new(context);
        connection.Open(); //This needs to be last or an error occurs because of the repository being set

        //Act
        //Check if not empty

        //Assert
        //If not empty PASS
    }


    [Fact] //Check that adding a cheep to the in memory database doesn't affect our current database
    public void MemoryDatabaseShouldntAffectDatabaseTest()
    {
        //Arrange
        //Creates a database in memory
        using var connection = new SqliteConnection("Filename=:memory:");
        var builder = new DbContextOptionsBuilder<ChirpDBContext>();
        builder.UseSqlite(connection);
        ChirpDBContext context = new(builder.Options);
        context.Database.EnsureCreated();
        CheepRepository repository = new(context);
        connection.Open(); //This needs to be last or an error occurs because of the repository being set

        //Somehow initiate the existing database??

        //Create a cheep object
        CheepDTO cheep = new()
        {
            AuthorId = 3,
            Author = "fjkd",
            Timestamp = DateTime.Now.ToString(),
            Message = "This is a cheep for testing"
        };

        //Act
        Action act = () => repository.AddCheep(cheep.AuthorId, cheep.Message);
        //Check if the created message is in the normal database

        //Assert
        //If not in the normal database PASS
    }
}