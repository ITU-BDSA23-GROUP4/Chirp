using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

public class RestrictedCheepTests
{
    private readonly SqliteConnection? _connection;
    private readonly ChirpDBContext _context;
    private readonly CheepRepository repository;
    private readonly CheepDTO cheep;
    public RestrictedCheepTests()
    {
        //Arrange  -  Runs in memory and is the arrange part of tests
        //Creates a database in memory - Makkes connection string before opening the connection
        var builder = new DbContextOptionsBuilder<ChirpDBContext>();
        builder.UseSqlite("Filename=:memory:");
         ChirpDBContext context = new(builder.Options);
        _connection = context.Database.GetDbConnection() as SqliteConnection;
        if(_connection != null){
            _connection.Open();
        }
        context.Database.EnsureCreated();

        //Act  -  Makes a cheepDTO object, which we can modify the message with in the tests
        cheep = new()
        {
            AuthorId = 3,
            Author = "fjkd",
            Timestamp = DateTime.Now,
            Message = ""
        };

        context.SaveChanges();
        _context = context;
        repository = new CheepRepository(_context);
    }

    [Fact] //Should not be possible to add a cheep over 160 characters
    public void TestRestrictedCreationOfCheepOver160Char()
    {
        //Act  -  Sets the message and adds an action to add the cheep to the in memory database
        cheep.Message = "This string should be way over 160 characters, just so we can check that its not possible to make a message that is longer than nessesary.This will because of that, become a very long message.";

        Action act = () => repository.AddCheep(cheep.AuthorId, cheep.Message);

        //Assert  -  This test should throw an exception to pass
        act.Should().Throw<Exception>().WithMessage("Message is too long or short");
    }

    [Fact] //Should be possible to add a cheep at exactly 160 characters
    public void TestRestrictedCreationOfCheepAt160Char()
    {
        //Act  -  Sets the message and adds an action to add the cheep to the in memory database
        cheep.Message = " This string should be at exactly 160 characters, so that we know its possible. There should not be a character more or less, so we'll have a very precise test.";

        Action act = () => repository.AddCheep(cheep.AuthorId, cheep.Message);

        //Assert  -  This test should not throw an exception to pass
        act.Should().NotThrow<Exception>();
    }

    [Fact] //Should be possible to add a cheep that is under 160 characters
    public void TestRestrictedCreationOfCheepUnder160Char()
    {
        //Act  -  Sets the message and adds an action to add the cheep to the in memory database
        cheep.Message = "This string is very much under 160 characters";

        Action act = () => repository.AddCheep(cheep.AuthorId, cheep.Message);

        //Assert
        act.Should().NotThrow<Exception>();
    }

    [Fact] //Should not be possible to add a cheep that is empty
    public void TestRestrictedCreationOfCheepOf0Char()
    {
        //Act  -  Sets an action to add the cheep to the in memory database
        //Message is already empty here

        Action act = () => repository.AddCheep(cheep.AuthorId, cheep.Message);

        //Assert
        act.Should().Throw<Exception>().WithMessage("Message is too long or short");
    }
}