public class AuthorRepositoryUnitTests
{
    private readonly SqliteConnection? _connection; //Connection to the database
    private readonly ChirpDBContext _context; //Context for the database
    private readonly AuthorRepository repository; //Repository for the database

    public AuthorRepositoryUnitTests()
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

        /* Creates a author to add to the database. The objects are used in each test 
        Is the same author as in the restrictedCheepsUnitTests, so we know he is there*/
        var testAuthor = new Author
        {
            AuthorId = 1,
            Name = "TestName",
            Email = "TestEmail",
            Cheeps = new List<Cheep>(),
            Followed = new List<Follow>(),
            Followers = new List<Follow>()
        };

        //Creates and adds aauthor to the database
        context.Authors.Add(testAuthor);

        context.SaveChanges();
        _context = context;
        repository = new AuthorRepository(_context);
    }

    [Fact] //Test the method to get author by email - should be possible
    public void UnitTestFindAuthorByEmail()
    {
        //Act
        var author = repository.GetAuthorByEmail("TestEmail");

        //Assert
        //Should pass since they're the same
        author.Should().BeEquivalentTo(new Author { AuthorId = 1, 
            Name = "TestName", 
            Email = "TestEmail", 
            Cheeps = new List<Cheep>(), 
            Followed = new List<Follow>(), 
            Followers = new List<Follow>() 
        });
    }

    [Fact] //Test the method to get Author by a wrong email - shouldn't be possible
    public void UnitTestFindAuthorByWrongEmail(){
        //Act
        Action act = () => repository.GetAuthorByEmail("TestEmailWrong");

        //Assert
        //Should throw an exception since the email doesn't exist in our database
        act.Should().Throw<ArgumentException>().WithMessage("Author with email TestEmailWrong does not exist");
    }

    [Fact] //Test method to get Author by name - should be possible
    public void UnitTestFindAuthorByName()
    {
        //Act
        var author = repository.GetAuthorByName("TestName");

        //Assert
        //Should pass since they're the same
        author.Should().BeEquivalentTo(new Author { 
            AuthorId = 1, 
            Name = "TestName", 
            Email = "TestEmail", 
            Cheeps = new List<Cheep>(), 
            Followed = new List<Follow>(), 
            Followers = new List<Follow>() 
        });
    }

    [Fact] //Test method to get an author by the wrong name - shouldn't be possible
    public void UnitTestFindAuthorByWrongName()
    {
        //Act
        Action act = () => repository.GetAuthorByName("TestNameWrong");

        //Assert
        //Should throw an exception since the name doesn't exist in our database
        act.Should().Throw<ArgumentException>().WithMessage("Author with name TestNameWrong does not exist");
    }

    [Fact] //Test method to get an author by id - should be possible
    public void UnitTestFindAuthorById()
    {
        //Act
        var author = repository.GetAuthorByID(1);

        //Assert
        //Should pass since they're the same
        author.Should().BeEquivalentTo(new Author { 
            AuthorId = 1, 
            Name = "TestName", 
            Email = "TestEmail", 
            Cheeps = new List<Cheep>(),
            Followed = new List<Follow>(),
            Followers = new List<Follow>() 
        });
    }

    [Fact] //Test method to get an author by wrong ide - shouldn't be possible
    public void UnitTestFindAuthorByWrongId()
    {
        //Act
        Action act = () => repository.GetAuthorByID(2);

        //Assert
        //Should throw an exception since the id doesn't exist in our database
        act.Should().Throw<ArgumentException>().WithMessage("Author with id 2 does not exist");
    }
}