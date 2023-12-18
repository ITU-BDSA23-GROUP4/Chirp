public class CheepRepositoryUnitTests {
        private readonly SqliteConnection? _connection; //Connection to the database
    private readonly ChirpDBContext _context; //Context for the database
    private readonly CheepRepository repository; //Repository for the database

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
            AuthorId = new Guid(), 
            Name = "TestAuthor", 
            Email = "TestEmail", 
            Cheeps = new List<Cheep>(),
        };

        context.Authors.Add(testAuthor); 
        
        var testCheep = new Cheep {
            CheepId = new Guid(1,0,0,new byte[]{0,0,0,0,0,0,0,0}),
            Author = testAuthor,
            Text = "This is a test",
            Likes = 0,
            TimeStamp = DateTime.Now
        };

        context.Cheeps.Add(testCheep);

        context.SaveChanges();
        _context = context;
        repository = new CheepRepository(_context);
    }

    [Fact]
    public async void UnitTestLikeIncreaseMethodIncreasesLikeAttribute() {
        // Act
        await repository.IncreaseLikeAttributeInCheep(new Guid(1,0,0,new byte[]{0,0,0,0,0,0,0,0}));
        CheepDTO cheep = repository.GetCheepsFromAuthor("TestAuthor", null)[0];

        // Assert
        cheep.Likes.Should().Be(1);
    }

    [Fact]
    public async void UnitTestTwoIncreaseMethodCallsEqualsValueOfTwo() {
        // Act
        await repository.IncreaseLikeAttributeInCheep(new Guid(1,0,0,new byte[]{0,0,0,0,0,0,0,0}));
        await repository.IncreaseLikeAttributeInCheep(new Guid(1,0,0,new byte[]{0,0,0,0,0,0,0,0}));

        CheepDTO cheep = repository.GetCheepsFromAuthor("TestAuthor", null)[0];

        // Assert
        cheep.Likes.Should().Be(2);
    }

}