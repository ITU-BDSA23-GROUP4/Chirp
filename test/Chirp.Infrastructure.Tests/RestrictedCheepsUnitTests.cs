using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

public class RestrictedCheepTests{
    private readonly ChirpDBContext context;
    private readonly CheepRepository repository;
    public RestrictedCheepTests(){
        //Runs in memory and is the arrange part of tests
        using var connection = new SqliteConnection("Filename=:memory:");
        var builder = new DbContextOptionsBuilder<ChirpDBContext>();
        builder.UseSqlite(connection);
        context = new ChirpDBContext(builder.Options);
        context.Database.EnsureCreated();
        repository = new CheepRepository(context);
        connection.Open();
    }

    [Fact]
    public void TestRestrictedCreationOfCheepOver160Char(){
        //Act
        CheepDTO cheep = new()
        {
            AuthorId = 3,
            Author = "fjkd",
            Timestamp = DateTime.Now.ToString(),
            Message = "This string should be way over 160 characters, just so we can check that its not possible to make a message that is longer than nessesary.This will because of that, become a very long message."
        };

        Action act = () => repository.AddCheep(cheep.AuthorId, cheep.Message);

        //Assert
        act.Should().Throw<Exception>().WithMessage("Message is too long");
    }

    [Fact]
    public void TestRestrictedCreationOfCheepAt160Char(){
        //Act
        CheepDTO cheep = new()
        {
            AuthorId = 3,
            Author = "fjkd",
            Timestamp = DateTime.Now.ToString(),
            Message = " This string should be at exactly 160 characters, so that we know its possible. There should not be a character more or less, so we'll have a very precise test."
        };

        Action act = () => repository.AddCheep(cheep.AuthorId, cheep.Message);

        //Assert
        act.Should().NotThrow<Exception>();
    }

     [Fact]
    public void TestRestrictedCreationOfCheepUnder160Char(){
        //Act
        CheepDTO cheep = new()
        {
            AuthorId = 3,
            Author = "fjkd",
            Timestamp = DateTime.Now.ToString(),
            Message = "This string is very much under 160 characters"
        };

        Action act = () => repository.AddCheep(cheep.AuthorId, cheep.Message);

        //Assert
        act.Should().NotThrow<Exception>();
    }

     [Fact]
    public void TestRestrictedCreationOfCheepOf0Char(){
        //Act
        CheepDTO cheep = new()
        {
            AuthorId = 3,
            Author = "fjkd",
            Timestamp = DateTime.Now.ToString(),
            Message = ""
        };

        Action act = () => repository.AddCheep(cheep.AuthorId, cheep.Message);

        //Assert
        act.Should().Throw<Exception>().WithMessage("Message is empty");
    }
}