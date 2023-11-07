using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

public class CheepRepositoryUnitTests
{
    private readonly SqliteConnection? _connection; //Connection to the database
    private readonly ChirpDBContext _context; //Context for the database
    private readonly CheepRepository repository; //Repository for the database
    private readonly CheepCreateValidator validator; //Validator for the database
    private readonly CheepCreateDTO cheepCreateDto; //DTO for the database

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

        cheepCreateDto = new("TestAuthor", "TestMessage");
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
    public async Task UnitTestCreateMethod()
    {
        //Act
        Cheep result = await repository.Create(cheepCreateDto);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("TestAuthor", result.Author.Name);
        Assert.Equal("TestMessage", result.Text);
    }
}