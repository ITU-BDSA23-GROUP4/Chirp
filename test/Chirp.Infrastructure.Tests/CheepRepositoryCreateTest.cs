using System.ComponentModel.DataAnnotations;

public class CheepRepositoryCreateUnitTests
{
    private readonly SqliteConnection? _connection; // Connection to the database
    private readonly ChirpDBContext _context; // Context for the database
    private readonly CheepRepository repository; // The repository contains the methods, tested by the unit tests
    public CheepRepositoryCreateUnitTests()
    {
        // Arrange
        
        // Creates a SQLite database in memory
        var builder = new DbContextOptionsBuilder<ChirpDBContext>();
        builder.UseSqlite("Filename=:memory:");
        ChirpDBContext context = new(builder.Options);

        _connection = context.Database.GetDbConnection() as SqliteConnection;
        if (_connection != null)
        {
            _connection.Open();
        }
        context.Database.EnsureCreated();

        // Test data
        var testAuthor = new Author {
            AuthorId = new Guid(1,0,0, new byte[] {0,0,0,0,0,0,0,0}), 
            Name = "TestAuthor", 
            Email = "TestEmail", 
            Cheeps = new List<Cheep>(),
            };

        context.Authors.Add(testAuthor);
        context.SaveChanges();

        _context = context;
        repository = new CheepRepository(_context);
    }

    [Fact]
    public async void UnitTestCreateMethod()
    {
        //Arrange
        string Message = "TestMessage";
        CheepCreateDTO cheepCreateDto = new CheepCreateDTO("TestAuthor", Message);

        //Act
        await repository.Create(cheepCreateDto);
        var cheeps = repository.GetCheeps(1);
        cheeps.Should().NotBeNull();

        //Assert
        cheeps.Should().Contain(c => c.AuthorName == "TestAuthor" && c.Message == "TestMessage");
    }

    [Fact]
    public async void UnitTestCreateMethodAddedCheepIsInDatabase()
    {
        //Arrange
        var initialCount = _context.Cheeps.Count();
        string Message = "TestMessage2";
        CheepCreateDTO cheepCreateDto = new CheepCreateDTO("TestAuthor", Message);

        //Act
        await repository.Create(cheepCreateDto);
        var cheeps = repository.GetCheeps(1);
        cheeps.Should().NotBeNull();

        //Assert
        cheeps.Should().Contain(c => c.AuthorName == "TestAuthor" && c.Message == "TestMessage2");
    }

    [Fact]
    public void UnitTestCreateMethodAddedCheepShouldNotBeInDatabase1()
    {
        //Arrange
        string Message = "";
        CheepCreateDTO cheepCreateDto = new CheepCreateDTO("TestAuthor", Message);

        var act = async () => await repository.Create(cheepCreateDto);

        //Assert
        act.Should().ThrowAsync<ValidationException>().WithMessage("Exception of type 'System.ComponentModel.DataAnnotations.ValidationException' was thrown.");
    }

    [Fact]
    public void UnitTestCreateMethodAddedCheepShouldNotBeInDatabase2()
    {
        //Arrange
        string Message = "This string should be way over 160 characters, just so we can check that its not possible to make a message that is longer than nessesary.This will because of that, become a very long message.";
        CheepCreateDTO cheepCreateDto = new CheepCreateDTO("TestAuthor", Message);
        var act = () => repository.Create(cheepCreateDto);

        //Assert
        act.Should().ThrowAsync<ValidationException>().WithMessage("Exception of type 'System.ComponentModel.DataAnnotations.ValidationException' was thrown.");
    }

    [Fact]
    public async void UnitTestDeleteCheepsFromAuthor()
    {
        //Arrange
        string Message = "TestMessage";
        CheepCreateDTO cheepCreateDto = new CheepCreateDTO("TestAuthor", Message);
        await repository.Create(cheepCreateDto);

        //Act
        await repository.DeleteCheepsFromAuthor(new Guid(1, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }));

        //Assert
        _context.Cheeps.Should().BeEmpty();
    }
}