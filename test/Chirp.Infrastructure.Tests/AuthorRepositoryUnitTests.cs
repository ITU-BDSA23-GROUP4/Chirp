public class AuthorRepositoryUnitTests
{
    private readonly SqliteConnection? _connection; // Connection to the database
    private readonly ChirpDBContext _context; // Context for the database
    private readonly AuthorRepository repository; // The repository contains the methods, tested by the unit tests

    // The constructor is executed before each test
    public AuthorRepositoryUnitTests()
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
        var testAuthor = new Author
        {
            AuthorId = new Guid(1,0,0, new byte[] {0,0,0,0,0,0,0,0}),
            Name = "TestName",
            Email = "TestEmail",
            Cheeps = new List<Cheep>(),
            Followed = new List<Author>(),
            Followers = new List<Author>()
        };
        var testAuthor2 = new Author
        {
            AuthorId = new Guid(2,0,0, new byte[] {0,0,0,0,0,0,0,0}),
            Name = "TestName2",
            Email = "TestEmail2",
            Cheeps = new List<Cheep>(),
        };
        var testAuthor3 = new Author {
            AuthorId = new Guid(3,0,0, new byte[] {0,0,0,0,0,0,0,0}),
            Name = "TestName3",
            Email = "TestEmail3",
            Cheeps = new List<Cheep>(),
        };

        context.Authors.Add(testAuthor);
        context.Authors.Add(testAuthor2);
        context.Authors.Add(testAuthor3);
        context.SaveChanges();

        _context = context;
        repository = new AuthorRepository(_context);
    }

    [Fact]
    public async void UnitTestFindAuthorByEmail()
    {
        //Act
        var author = await repository.GetAuthorByEmail("TestEmail");

        //Assert
        author.Should().BeEquivalentTo(new Author {
            AuthorId = new Guid(1,0,0, new byte[] {0,0,0,0,0,0,0,0}), 
            Name = "TestName", 
            Email = "TestEmail", 
            Cheeps = new List<Cheep>(), 
            Followed = new List<Author>(),
            Followers = new List<Author>()
        });
    }

    [Fact]
    public void UnitTestFindAuthorByWrongEmail(){
        //Act
        Func<Task> act = async () => await repository.GetAuthorByEmail("TestEmailWrong");

        //Assert
        act.Should().ThrowAsync<ArgumentException>().WithMessage("Author with email TestEmailWrong does not exist");
    }

    [Fact]
    public async void UnitTestFindAuthorByName()
    {
        //Act
        var author = await repository.GetAuthorByName("TestName");

        //Assert
        author.Should().BeEquivalentTo(new Author { 
            AuthorId = new Guid(1,0,0, new byte[] {0,0,0,0,0,0,0,0}), 
            Name = "TestName", 
            Email = "TestEmail", 
            Cheeps = new List<Cheep>(),
            Followed = new List<Author>(),
            Followers = new List<Author>()
        });
    }

    [Fact]
    public void UnitTestFindAuthorByWrongName()
    {
        //Act
        Func<Task> act = async () => await repository.GetAuthorByName("TestNameWrong");

        //Assert
        act.Should().ThrowAsync<ArgumentException>().WithMessage("Author with name TestNameWrong does not exist");
    }

    [Fact]
    public async void UnitTestAddFolloweeAddsToTheAuthorsFollowedList()
    {
        //Act
        await repository.AddFollowee("TestName","TestName2");
        AuthorDTO author1 = await repository.GetAuthorByName("TestName");

        //Assert
        author1.Followed?[0].AuthorId.Should().Be(new Guid(2,0,0, new byte[] {0,0,0,0,0,0,0,0}));
    }

    [Fact]
    public void UnitTestAddIncorrectFollowee()
    {
        //Act
        Func<Task> act = async () => await repository.AddFollowee("TestName","TestName4");
        
        //Assert
        act.Should().ThrowAsync<NullReferenceException>().WithMessage("Obejct _Author or _Followee of type Author is null");
    }

    [Fact]
    public async void UnitTestRemoveFolloweeRemovesFromTheAuthorsFollowedList() 
    {
        // Arrange
        await repository.AddFollowee("TestName","TestName2");

        //Act
        await repository.RemoveFollowee("TestName","TestName2");
        AuthorDTO author1 = await repository.GetAuthorByName("TestName");

        //Assert
        author1.Followed?.Count.Should().Be(0);
    }

    [Fact]
    public void UnitTestRemoveIncorrectFollowee() 
    {
        //Act
        Func<Task> act = async () => await repository.RemoveFollowee("TestName","TestName4");
        //Assert
        act.Should().ThrowAsync<NullReferenceException>().WithMessage("FollowerRelationship is null");
    }

    [Fact]
    public async void UnitTestFollowTheSameAuthorTwice() 
    {
        //Act
        await repository.AddFollowee("TestName","TestName2");
        Func<Task> act = async () => await repository.AddFollowee("TestName","TestName2");

        //Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async void UnitTestDeleteAuthorAsync()
    {
        //Act
        await repository.DeleteAuthor(new Guid(1, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }));

        //Assert
        _context.Authors.Should().NotContain(a => a.AuthorId == new Guid(1, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }));
    }
    
    [Fact]
    public async void UnitTestFollowAuthorUpdatesOtherAuthorsFollowersList() 
    {
        //Act
        await repository.AddFollowee("TestName","TestName2");
        AuthorDTO author2 = await repository.GetAuthorByName("TestName2");

        //Assert
        author2.Followers?[0].AuthorId.Should().Be(new Guid(1,0,0, new byte[] {0,0,0,0,0,0,0,0}));
    }

    [Fact]
    public async void UnitTestUnFollowAuthorUpdatesOtherAuthorsFollowersList() 
    {
        // Arrange
        await repository.AddFollowee("TestName","TestName2");

        //Act
        await repository.RemoveFollowee("TestName","TestName2");
        AuthorDTO author = await repository.GetAuthorByName("TestName2");
        
        // Assert
        author.Followers?.Count.Should().Be(0);
    }

    [Fact]
    public async void UnitTestAddFollowDoesNotAffactOtherAuthors() 
    {
        //Act
        await repository.AddFollowee("TestName","TestName2");

        //Assert
        AuthorDTO author = await repository.GetAuthorByName("TestName3");

        author.Followed?.Count.Should().Be(0);
        author.Followers?.Count.Should().Be(0);
    }

    [Fact]
    public async void UnitTestEnsureFollowAndFollowedListAreEmptyBeDefault() 
    {
        //Assert
        AuthorDTO author = await repository.GetAuthorByName("TestName");

        author.Followed?.Count.Should().Be(0);
        author.Followers?.Count.Should().Be(0);
    }

    [Fact]
    public async void UnitTestAddFollowDoesNotAffectWrongList() 
    {
        //Act
        await repository.AddFollowee("TestName","TestName2");

        //Assert
        AuthorDTO author1 = await repository.GetAuthorByName("TestName");
        AuthorDTO author2 = await repository.GetAuthorByName("TestName2");

        author1.Followers?.Count.Should().Be(0);
        author2.Followed?.Count.Should().Be(0);
    }

    [Fact]
    public async void UnitTestFollowYourSelfThrowsException()
    {
        // Act
        Func<Task> act= async () => await repository.AddFollowee("TestName", "TestName");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("TestName can not follow TestName, as TestName can not follow itself");
    }
}