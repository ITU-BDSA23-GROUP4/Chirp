
public class RestrictedCheepTests
{
    private readonly SqliteConnection? _connection; // Connection to the database
    private readonly ChirpDBContext _context; // Context for the database
    private readonly CheepRepository repository; // The repository contains the methods, tested by the unit tests
    public RestrictedCheepTests()
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

        // Tests data
        var testAuthor = new Author
        {
            AuthorId = new Guid(1,0,0, new byte[] {0,0,0,0,0,0,0,0}),
            Name = "TestName",
            Email = "TestEmail",
            Cheeps = new List<Cheep>(),
        };
        var testCheep = new Cheep
        {
            CheepId = new Guid(1,0,0, new byte[] {0,0,0,0,0,0,0,0}),
            Author = testAuthor,
            Likes = 0,
            TimeStamp = DateTime.Now,
            Text = "This is a cheep for testing"
        };

        context.Authors.Add(testAuthor);
        context.Cheeps.Add(testCheep);
        context.SaveChanges();

        _context = context;
        repository = new CheepRepository(_context);
    }

    [Fact]
    public void TestIfAuthorIsThere()
    {
        //Act
        var author = _context.Authors.Where(author => author.AuthorId == new Guid(1,0,0, new byte[] {0,0,0,0,0,0,0,0})).FirstOrDefault();

        //Assert
        author.Should().NotBeNull();
    }

    [Fact]
    public void TestRestrictedCreationOfCheepOver160Char()
    {
        //Act
        string Message = "This string should be way over 160 characters, just so we can check that its not possible to make a message that is longer than nessesary.This will because of that, become a very long message.";

        Func<Task> act = async () => await repository.AddCheep(new Guid(1,0,0, new byte[] {0,0,0,0,0,0,0,0}), Message);
        //Assert
        act.Should().ThrowAsync<ArgumentException>().WithMessage("Message is above 160 characters or empty");
    }

    [Fact]
    public void TestRestrictedCreationOfCheepAt160Char()
    {
        //Act
        string Message = " This string should be at exactly 160 characters, so that we know its possible. There should not be a character more or less, so we'll have a very precise test.";

        Func<Task> act = async () => await repository.AddCheep(new Guid(1,0,0, new byte[] {0,0,0,0,0,0,0,0}), Message);

        //Assert
        act.Should().NotThrowAsync<ArgumentException>();
    }

    [Fact]
    public void TestRestrictedCreationOfCheepUnder160Char()
    {
        //Act
        string Message = "This string is very much under 160 characters";

        Func<Task> act = async () => await repository.AddCheep(new Guid(1,0,0, new byte[] {0,0,0,0,0,0,0,0}), Message);

        //Assert
        act.Should().NotThrowAsync<ArgumentException>();
    }

    [Fact]
    public void TestRestrictedCreationOfCheepOf0Char()
    {
        //Act
        Func<Task> act = async () => await repository.AddCheep(new Guid(1,0,0, new byte[] {0,0,0,0,0,0,0,0}), "");

        //Assert
        act.Should().ThrowAsync<ArgumentException>().WithMessage("Message is above 160 characters or empty");
    }

    [Fact]
    public void TestRestrictedCheepIsAddedToMemoryDatabase()
    {
        //Act
        Action act2 = () => repository.GetCheeps(1);
        act2.Should().NotThrow<Exception>();
        
        var cheeps = repository.GetCheeps(1);
        cheeps.Should().NotBeNull();

        //Assert
        cheeps.Should().Contain(c => c.CheepId == new Guid(1,0,0, new byte[] {0,0,0,0,0,0,0,0}) && c.Message == "This is a cheep for testing");
    }
}