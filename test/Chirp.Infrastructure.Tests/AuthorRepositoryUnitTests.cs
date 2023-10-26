using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

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

        /* Creates a author to add to the database. The objects are used in each test */
        var testAuthor = new Author
        {
            AuthorId = 1,
            Name = "TestName",
            Email = "TestEmail",
            Cheeps = new List<Cheep>()
        };

        //Creates and adds aauthor to the database
        context.Authors.Add(testAuthor);

        context.SaveChanges();
        _context = context;
        repository = new AuthorRepository(_context);
    }

    [Fact]
    public void UnitTestFindAuthorByEmail()
    {

    }

    [Fact]
    public void UnitTestFindAuthorByWrongEmail(){
        
    }

    [Fact]
    public void UnitTestFindAuthorByName()
    {

    }

    [Fact]
    public void UnitTestFindAuthorByWrongName()
    {

    }

    [Fact]
    public void UnitTestCreateAuthorWithSucess(){

    }
}