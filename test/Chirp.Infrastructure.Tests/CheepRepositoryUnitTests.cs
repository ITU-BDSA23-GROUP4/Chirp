public class CheepRepositoryUnitTests {
        private readonly SqliteConnection? _connection; // Connection to the database
    private readonly ChirpDBContext _context; // Context for the database
    private readonly CheepRepository repository; // The repository contains the methods, tested by the unit tests

    public CheepRepositoryUnitTests()
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
            AuthorId = new Guid(), 
            Name = "TestAuthor", 
            Email = "TestEmail", 
            Cheeps = new List<Cheep>(),
        };        
        var testCheep = new Cheep {
            CheepId = new Guid(1,0,0,new byte[]{0,0,0,0,0,0,0,0}),
            Author = testAuthor,
            Text = "This is a test",
            Likes = 0,
            TimeStamp = DateTime.Now
        };
        
        var testAuthor2 = new Author {
            AuthorId = new Guid(), 
            Name = "TestAuthor2", 
            Email = "TestEmail2", 
            Cheeps = new List<Cheep>(),
        };
        var testCheep2 = new Cheep {
            CheepId = new Guid(2,0,0,new byte[]{0,0,0,0,0,0,0,0}),
            Author = testAuthor2,
            Text = "This is a test2",
            Likes = 0,
            TimeStamp = DateTime.Now
        };

        var testAuthor3 = new Author {
            AuthorId = new Guid(3,0,0,new byte[]{0,0,0,0,0,0,0,0}), 
            Name = "TestAuthor3", 
            Email = "TestEmail3", 
            Cheeps = new List<Cheep>(),
        };
        var testCheep3 = new Cheep {
            CheepId = new Guid(3,0,0,new byte[]{0,0,0,0,0,0,0,0}),
            Author = testAuthor3,
            Text = "This is a test3",
            Likes = 0,
            TimeStamp = DateTime.Now
        };

        testAuthor.Followed = new List<Author>
        {
            testAuthor2
        };
       

        context.Authors.Add(testAuthor); 
        context.Cheeps.Add(testCheep);
        context.Authors.Add(testAuthor2);
        context.Cheeps.Add(testCheep2);
        context.Authors.Add(testAuthor3);
        context.Cheeps.Add(testCheep3);
        context.SaveChanges();

        _context = context;
        repository = new CheepRepository(_context);
    }

    [Fact]
    public async void UnitTestLikeIncreaseMethodIncreasesLikeAttribute() 
    {
        // Act
        await repository.IncreaseLikeAttributeInCheep(new Guid(1,0,0,new byte[]{0,0,0,0,0,0,0,0}));
        CheepDTO cheep = repository.GetCheepsFromAuthor("TestAuthor", null)[0];

        // Assert
        cheep.Likes.Should().Be(1);
    }

    [Fact]
    public async void UnitTestTwoIncreaseMethodCallsEqualsValueOfTwo() 
    {
        // Act
        await repository.IncreaseLikeAttributeInCheep(new Guid(1,0,0,new byte[]{0,0,0,0,0,0,0,0}));
        await repository.IncreaseLikeAttributeInCheep(new Guid(1,0,0,new byte[]{0,0,0,0,0,0,0,0}));
        CheepDTO cheep = repository.GetCheepsFromAuthor("TestAuthor", null)[0];

        // Assert
        cheep.Likes.Should().Be(2);
    }

    //Test get cheeps method
    [Fact]
    public void UnitTestGetCheepsFromAuthorReturnsCorrectCheep() 
    {
        // Act
        List<CheepDTO> cheep = repository.GetCheeps(null);

        // Assert
        cheep.Should().Contain(c => c.AuthorName == "TestAuthor" && c.Message == "This is a test");
    }

    //Test get cheeps method with author
    [Fact]
    public void UnitTestGetCheepsFromAuthorReturnsCorrectCheepWithAuthor() 
    {
        // Act
        List<CheepDTO> cheep = repository.GetCheepsFromAuthor("TestAuthor", null);

        // Assert
        cheep.Should().Contain(c => c.AuthorName == "TestAuthor" && c.Message == "This is a test");
    }

    // Test get count of all cheeps
    [Fact]
    public void UnitTestGetCountOfAllCheepsReturnsCorrectCount() 
    {
        // Act
        int count = repository.GetCountOfAllCheeps();

        // Assert
        count.Should().Be(3);
    }

    // Test get count of all cheeps from author
    [Fact]
    public void UnitTestGetCountOfAllCheepsFromAuthorReturnsCorrectCount() 
    {
        // Act
        int count = repository.GetCountOfAllCheepFromAuthor("TestAuthor");

        // Assert
        count.Should().Be(1);
    }

    // Test CombineCheepsAndFollowerCheeps
    [Fact]
    public void UnitTestCombineCheepsAndFollowerCheepsReturnsCorrectCheep() 
    {
        // Act
        List<CheepDTO> cheep = repository.CombineCheepsAndFollowerCheeps("TestAuthor", null);

        // Assert
        cheep.Should().Contain(c => c.AuthorName == "TestAuthor" && c.Message == "This is a test");
        cheep.Should().Contain(c => c.AuthorName == "TestAuthor2" && c.Message == "This is a test2");
    }

    // Test GetCountOfAllCheepsFromCombinedAuthor with author
    [Fact]
    public void UnitTestGetCountOfAllCheepsFromCombinedAuthorReturnsCorrectCount() 
    {
        // Act
        int count = repository.GetCountOfAllCheepsFromCombinedAuthor("TestAuthor");

        // Assert
        count.Should().Be(2);
    }
}