public class RestrictedCheepTests
{
    private readonly SqliteConnection? _connection; //Connection to the database
    private readonly ChirpDBContext _context; //Context for the database
    private readonly CheepRepository repository; //Repository for the database
    private readonly CheepCreateValidator validator; //Validator for the database

    public RestrictedCheepTests()
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

        /* Creates a cheep and author to add to the database. These objects are used in each test */
        var testAuthor = new Author
        {
            AuthorId = 1,
            Name = "TestName",
            Email = "TestEmail",
            Cheeps = new List<Cheep>(),
            Followed = new List<Follow>(),
            Followers = new List<Follow>()
        };
        var testCheep = new Cheep
        {
            CheepId = 1,
            Author = testAuthor,
            TimeStamp = DateTime.Now,
            Text = "This is a cheep for testing"
        };
        //Creates and adds a cheep and author to the database
        context.Authors.Add(testAuthor);
        context.Cheeps.Add(testCheep);

        validator = new CheepCreateValidator();
        if (validator == null)
            {
                throw new Exception("Validator is null");
            }

        context.SaveChanges();
        _context = context;
        repository = new CheepRepository(_context, validator);
    }

    //A Test which checks if the testAuthor is there
    [Fact]
    public void TestIfAuthorIsThere()
    {
        //Act
        var author = _context.Authors.Where(author => author.AuthorId == 1).FirstOrDefault();

        //Assert
        author.Should().NotBeNull();
    }

    [Fact] //Should not be possible to add a cheep over 160 characters
    public void TestRestrictedCreationOfCheepOver160Char()
    {
        //Act
        //Sets the message and adds an action to add the cheep to the in memory database
        string Message = "This string should be way over 160 characters, just so we can check that its not possible to make a message that is longer than nessesary.This will because of that, become a very long message.";

        Action act = () => repository.AddCheep(1, Message); //Adds the cheep to the database

        //Assert
        act.Should().Throw<ArgumentException>().WithMessage("Message is above 160 characters or empty"); //Should throw an exception to pass
    }

    [Fact] //Should be possible to add a cheep at exactly 160 characters
    public void TestRestrictedCreationOfCheepAt160Char()
    {
        //Act
        //Sets the message and adds an action to add the cheep to the in memory database
        string Message = " This string should be at exactly 160 characters, so that we know its possible. There should not be a character more or less, so we'll have a very precise test.";

        Action act = () => repository.AddCheep(1, Message);

        //Assert
        act.Should().NotThrow<ArgumentException>(); //Should not throw an exception to pass
    }

    [Fact] //Should be possible to add a cheep that is under 160 characters
    public void TestRestrictedCreationOfCheepUnder160Char()
    {
        //Act
        //Sets the message and adds an action to add the cheep to the in memory database
        string Message = "This string is very much under 160 characters";

        Action act = () => repository.AddCheep(1, Message);

        //Assert
        act.Should().NotThrow<ArgumentException>(); //Should not throw an exception to pass
    }

    [Fact] //Should not be possible to add a cheep that is empty
    public void TestRestrictedCreationOfCheepOf0Char()
    {
        //Act
        //Sets an action to add the cheep to the in memory database
        Action act = () => repository.AddCheep(1, "");

        //Assert
        //Should throw an exception to pass
        act.Should().Throw<ArgumentException>().WithMessage("Message is above 160 characters or empty");
    }

    [Fact]
    public void TestRestrictedCheepIsAddedToMemoryDatabase()
    {
        //Act
        //Get the cheeps from the current database
        Action act2 = () => repository.GetCheeps(1);
        act2.Should().NotThrow<Exception>(); //Making sure it's possible

        var cheeps = repository.GetCheeps(1);
        cheeps.Should().NotBeNull(); //Makes sure the page is not empty

        //Assert
        cheeps.Should().Contain(c => c.AuthorId == 1 && c.Message == "This is a cheep for testing");
    }
}