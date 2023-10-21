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

        //Act    Check if the database is empty
        Action act = () => repository.GetCheeps(1);

        //Assert    If not empty it should PASS
        act.Should().NotThrow<Exception>(); //Making sure it is possible
    }


    [Fact] //Check that adding a cheep to the in memory database doesn't affect our current database
    public void MemoryDatabaseShouldntAffectDatabaseTest()
    {
        //Arrange

        //Creates a database in memory
        using var connection = new SqliteConnection("Filename=:memory:"); //Creates a database in memory
        var builder = new DbContextOptionsBuilder<ChirpDBContext>(); //Creates a new context
        builder.UseSqlite(connection); //Sets the context to use the in memory database
        ChirpDBContext context = new(builder.Options); //Creates a new context
        context.Database.EnsureCreated(); //Makes sure the database is created
        CheepRepository repository = new(context);
        connection.Open(); //This needs to be last or an error occurs because of the repository being set

        //initializes the current database
        CheepRepository repository_ = new();

        //Copy the content of the repository_ object into the repository object
        var cheeps_ = repository_.GetCheeps(1); //NOT WORKING?
        foreach (var cheep_ in cheeps_)
        {
            repository.AddCheep(cheep_.AuthorId, cheep_.Message);
        }

        //Create a cheep object
        CheepDTO cheep = new()
        {
            AuthorId = 3,
            Author = "fjkd",
            Timestamp = DateTime.Now.ToString(),
            Message = "This is a cheep for testing"
        };

        //Act
        Action act1 = () => repository.AddCheep(cheep.AuthorId, cheep.Message); //Add the cheep to the in memory database
        act1.Should().NotThrow<Exception>(); //Making sure it is possible

        //Get the cheeps from the current database
        Action act2 = () => repository_.GetCheeps(1); //Get the first page of cheeps
        act2.Should().NotThrow<Exception>(); //Making sure it is possible

        //Assert
        //If not in the normal database PASS
        var cheeps = repository_.GetCheeps(1); //Get the first page of cheeps 
        cheeps.Should().NotBeNull(); //Making sure it is possible
        //See if the cheep is in the normal database
        cheeps.Should().NotContain(c => c.AuthorId == cheep.AuthorId && c.Message == cheep.Message); 
    }
}