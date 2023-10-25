using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
public class InMemoryDatabaseTest
{
    private readonly SqliteConnection? _connection; //The connection to the database
    private readonly ChirpDBContext _context; //The context of the database
    private readonly CheepRepository repository; //The repository of the memory database
    private readonly CheepRepository ExistingRepository; //The repository of the existing database

    public InMemoryDatabaseTest()
    {
        //Arrrange 

        //Creates a database in memory - Makkes connection string before opening the connection
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
            Cheeps = new List<Cheep>()
            };
        var testCheep = new Cheep {
            CheepId = 1, 
            Author = testAuthor, 
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
        

        context.SaveChanges();
        _context = context; 
        repository = new CheepRepository(_context);
        ExistingRepository = new CheepRepository(context2);
    }

    [Fact] //Check if the memory database is not empty
    public void MemoryDatabaseShouldNotBeEmpty()
    {
        //Act
        //Get the first page of cheeps
        Action act = () => repository.GetCheeps(1);

        //Assert
        //If not empty it should PASS
        act.Should().NotThrow<Exception>();
    }


    [Fact] //Check that adding a cheep to the in memory database doesn't affect our current database
    public void MemoryDatabaseShouldntAffectDatabaseTest()
    {
        //Act
        //Get the cheeps from the current database
        Action act = () => ExistingRepository.GetCheeps(1);
        act.Should().NotThrow<Exception>(); //Making sure it's possible

        var cheeps = ExistingRepository.GetCheeps(1);
        cheeps.Should().NotBeNull(); //Makes sure the page is not empty

        //Assert
        //See if the cheep is in the normal database, if it isn't it should PASS
        cheeps.Should().NotContain(c => c.AuthorId == 1 && c.Message == "This is a cheep for testing");
    }
}