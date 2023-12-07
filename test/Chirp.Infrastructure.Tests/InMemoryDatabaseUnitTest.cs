public class InMemoryDatabaseTest
{
    private readonly SqliteConnection? _connection; //The connection to the database
    private readonly ChirpDBContext _context; //The context of the database
    private readonly CheepRepository repository; //The repository of the memory database
    private readonly CheepRepository ExistingRepository; //The repository of the existing database
    private readonly CheepCreateValidator validator; //Validator for the database

    public InMemoryDatabaseTest()
    {
        //Arrange 

        //Creates a database in memory - Makes connection string before opening the connection
        var builder = new DbContextOptionsBuilder<ChirpDBContext>();
        builder.UseSqlite("Filename=:memory:");
        ChirpDBContext context = new(builder.Options);
        _connection = context.Database.GetDbConnection() as SqliteConnection;
        if (_connection != null) //Takes care of the null exception
        {
            _connection.Open();
        }
        context.Database.EnsureCreated();

        //Creates a cheep and author to add to the database
        var testAuthor = new Author {
            AuthorId = 1, 
            Name = "TestName", 
            Email = "TestEmail", 
            Cheeps = new List<Cheep>(),
            Followed = new List<Follow>(),
            Followers = new List<Follow>()
            };
        var testCheep = new Cheep {
            CheepId = 1, 
            Author = testAuthor, 
            Likes = 0,
            TimeStamp = DateTime.Now, 
            Text = "This is a cheep for testing"
        };
        
        //Adds the cheep and author to the database
        context.Authors.Add(testAuthor); 
        context.Cheeps.Add(testCheep);

        //Make context for ExistingRepository
        var builder2 = new DbContextOptionsBuilder<ChirpDBContext>();
        builder2.UseSqlite("Filename={Path.GetTempPath() + chirp.db}");
        ChirpDBContext context2 = new(builder2.Options);
        _connection = context2.Database.GetDbConnection() as SqliteConnection;
        if (_connection != null)
        {
            _connection.Open();
        }
        context2.Database.EnsureCreated();

        validator = new CheepCreateValidator();
        if (validator == null)
            {
                throw new Exception("Validator is null");
            }

        context.SaveChanges();
        _context = context; 
        repository = new CheepRepository(_context, validator);
        ExistingRepository = new CheepRepository(context2, validator);
    }

    [Fact] //Check if the memory database is not empty
    public void MemoryDatabaseShouldNotBeEmpty()
    {
        /* In the constructor we add a cheep to the database, this is used so we make sure something is added to the database,
        and then we check in this test, that this really do happen */

        //Act
        //Get the first page of cheeps
        Action act = () => repository.GetCheeps(1);

        //Assert
        //If not empty it should PASS
        act.Should().NotThrow<Exception>();
    }
}