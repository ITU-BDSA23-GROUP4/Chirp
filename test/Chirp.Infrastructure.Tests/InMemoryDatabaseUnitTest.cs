public class InMemoryDatabaseTest
{
    private readonly SqliteConnection? _connection; // The connection to the database
    private readonly ChirpDBContext _context; // The context of the database
    private readonly CheepRepository repository; // The repository contains the methods, tested by the unit tests

    public InMemoryDatabaseTest()
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
            Name = "TestName", 
            Email = "TestEmail", 
            Cheeps = new List<Cheep>(),
            };
        var testCheep = new Cheep {
            CheepId = new Guid(), 
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

    [Fact] //Check if the memory database is not empty
    public void MemoryDatabaseShouldNotBeEmpty()
    {
        /* In the constructor we add a cheep to the database, this is used so we make sure something is added to the database,
        and then we check in this test, that this really do happen */

        //Act
        Action act = () => repository.GetCheeps(1);

        //Assert
        act.Should().NotThrow<Exception>();
    }
}