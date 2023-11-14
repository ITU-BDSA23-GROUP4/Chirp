using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

public class CheepRepositoryUnitTests
{
    private readonly SqliteConnection? _connection; //Connection to the database
    private readonly ChirpDBContext _context; //Context for the database
    private readonly CheepRepository repository; //Repository for the database
    private readonly CheepCreateValidator validator; //Validator for the database

    public CheepRepositoryUnitTests()
    {
        //Arrange
        //Creates a database in memory - Makkes connection string before opening the connection
        var builder = new DbContextOptionsBuilder<ChirpDBContext>();
        builder.UseSqlite("Filename=:memory:");
        ChirpDBContext context = new(builder.Options);
        _connection = context.Database.GetDbConnection() as SqliteConnection;
        if (_connection != null)  //Takes care of the null exception
        {
            _connection.Open();
        }
        context.Database.EnsureCreated();

        var testAuthor = new Author {
            AuthorId = 1, 
            Name = "TestAuthor", 
            Email = "TestEmail", 
            Cheeps = new List<Cheep>()
            };

        context.Authors.Add(testAuthor); 
        
        validator = new CheepCreateValidator();
        if (validator == null)
            {
                throw new Exception("Validator is null");
            }

        context.SaveChanges();
        _context = context;
        repository = new CheepRepository(_context, validator);
    }

    [Fact]
    public void UnitTestCreateMethod()
    {
        //Arrange
        string Message = "TestMessage";
        CheepCreateDTO cheepCreateDto = new CheepCreateDTO("TestAuthor", Message);

        //Act
        repository.Create(cheepCreateDto); //Adds the cheep to the database

         var cheeps = repository.GetCheeps(1);
        cheeps.Should().NotBeNull(); //Makes sure the page is not empty

        //Assert
        cheeps.Should().Contain(c => c.Author == "TestAuthor" && c.Message == "TestMessage");
    }

    [Fact]
    public void UnitTestCreateMethodAddedCheepIsInDatabase()
    {
        //Arrange
        var initialCount = _context.Cheeps.Count();
        string Message = "TestMessage2";
        CheepCreateDTO cheepCreateDto = new CheepCreateDTO("TestAuthor", Message);

        //Act
        repository.Create(cheepCreateDto); //Adds the cheep to the database
        
        var cheeps = repository.GetCheeps(1);
        cheeps.Should().NotBeNull(); //Makes sure the page is not empty

        //Assert
        cheeps.Should().Contain(c => c.Author == "TestAuthor" && c.Message == "TestMessage2");
    }

    [Fact]
    public void UnitTestCreateMethodAddedCheepShouldNotBeInDatabase1()
    {
        //Arrange
        string Message = "";
        CheepCreateDTO cheepCreateDto = new CheepCreateDTO("TestAuthor", Message);

        var act = () => repository.Create(cheepCreateDto); //Adds the cheep to the database

        //Assert
        //Should throw an exception to pass
        act.Should().Throw<ValidationException>().WithMessage("Exception of type 'System.ComponentModel.DataAnnotations.ValidationException' was thrown.");
    }

    [Fact]
    public void UnitTestCreateMethodAddedCheepShouldNotBeInDatabase2()
    {
        //Arrange
        string Message = "This string should be way over 160 characters, just so we can check that its not possible to make a message that is longer than nessesary.This will because of that, become a very long message.";
        CheepCreateDTO cheepCreateDto = new CheepCreateDTO("TestAuthor", Message);

        var act = () => repository.Create(cheepCreateDto); //Adds the cheep to the database

        //Assert
        //Should throw an exception to pass
        act.Should().Throw<ValidationException>().WithMessage("Exception of type 'System.ComponentModel.DataAnnotations.ValidationException' was thrown.");
    }
}