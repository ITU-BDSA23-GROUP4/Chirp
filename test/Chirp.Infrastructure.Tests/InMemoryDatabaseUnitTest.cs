using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
public class InMemoryDatabaseTest
{   
    private readonly SqliteConnection? _connection;
    private readonly ChirpDBContext _context;
    private readonly CheepRepository repository;
    private readonly CheepDTO cheep;
    public InMemoryDatabaseTest(){
        //Arrrange 

        //Creates a database in memory - Makkes connection string before opening the connection
        var builder = new DbContextOptionsBuilder<ChirpDBContext>();
        builder.UseSqlite("Filename=:memory:");
         ChirpDBContext context = new(builder.Options);
        _connection = context.Database.GetDbConnection() as SqliteConnection;
        if(_connection != null){
            _connection.Open();
        }
        context.Database.EnsureCreated();
        DbInitializer.SeedDatabase(context);

        // Seed test data
        // var authors ...
        // var cheeps ...
        // Context.authors.addRange(authors)
        // context.cheeps.addRange(cheeps)

        context.SaveChanges();
        _context = context;
        repository = new CheepRepository(_context);
        AuthorRepository authorRepository = new(_context);
        authorRepository.AddAuthor("fjkd","fjdk@gmail.com");

        cheep = new()
        {
            AuthorId = authorRepository.GetAuthorByName("fjkd").AuthorId,
            Author = authorRepository.GetAuthorByName("fjkd").Name,
            Timestamp = DateTime.Now,
            Message = ""
        };
    }

    //Each of these needs their own in memory database, since they can corrupt eachother

    [Fact] //Check if the memory database is the same as the current database
    public void MemoryDatabaseShouldNotBeEmpty()
    {
        //Act    Check if the database is empty
        Action act = () => repository.GetCheeps(1);

        //Assert    If not empty it should PASS
        act.Should().NotThrow<Exception>(); //Making sure it is possible
    }


    [Fact] //Check that adding a cheep to the in memory database doesn't affect our current database
    public void MemoryDatabaseShouldntAffectDatabaseTest()
    {
        //Arrange

        //initializes the current database
        CheepRepository repository_ = new();

        cheep.Message = "This is a cheep for testing";

        //Act
        Action act1 = () => repository.AddCheep(cheep.AuthorId, cheep.Message); //Add the cheep to the in memory database

        //Get the cheeps from the current database
        Action act2 = () => repository_.GetCheeps(1); //Get the first page of cheeps
        act2.Should().NotThrow<Exception>(); //Making sure it's possible

        //Assert - If not in the normal database PASS
        var cheeps = repository_.GetCheeps(1); //Get the first page of cheeps 
        cheeps.Should().NotBeNull(); //Making sure it is possible
        //See if the cheep is in the normal database
        cheeps.Should().NotContain(c => c.AuthorId == cheep.AuthorId && c.Message == cheep.Message); 
    }
}