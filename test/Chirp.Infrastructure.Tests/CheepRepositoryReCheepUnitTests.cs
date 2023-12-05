
public class CheepRepositoryUnitTestsReCheep
{
    private readonly SqliteConnection? _connection; //Connection to the database
    private readonly ChirpDBContext _context; //Context for the database
    private readonly CheepRepository repository; //Repository for the database
    private readonly AuthorRepository authorRepository; //Repository for the database
    private readonly CheepCreateValidator validator; //Validator for the database

    public CheepRepositoryUnitTestsReCheep()
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
            AuthorId = 1, 
            Name = "TestAuthor", 
            Email = "TestEmail", 
            Cheeps = new List<Cheep>(),
            Followed = new List<Follow>(),
            Followers = new List<Follow>()
            };
        
        var testAuthor2 = new Author {
            AuthorId = 2, 
            Name = "TestAuthor2", 
            Email = "TestEmail2", 
            Cheeps = new List<Cheep>(),
            Followed = new List<Follow>(),
            Followers = new List<Follow>()
            };

        context.Authors.Add(testAuthor); 
        context.Authors.Add(testAuthor2);
        
        validator = new CheepCreateValidator();
        if (validator == null)
            {
                throw new Exception("Validator is null");
            }

        context.SaveChanges();
        _context = context;
        repository = new CheepRepository(_context, validator);
        authorRepository = new AuthorRepository(_context);
    }


    [Fact]
    public void UnitTestReCheepMethod(){
        //Arrange
        string Message = "TestMessage";
        CheepCreateDTO cheepCreateDto = new CheepCreateDTO("TestAuthor", Message);
        repository.Create(cheepCreateDto);
        List<CheepDTO> cheeps = repository.GetCheeps(1);
        CheepDTO cheep = cheeps[0];

        //Act
        repository.ReCheep(2, cheep);
        //Check the cheep is added to the database
        Action act = () => repository.GetCountOfAllCheepFromAuthor("TestAuthor2");

        //Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void UnitTestsReCheepAddsToDatabase(){
        //Arrange
        string Message = "TestMessage";
        CheepCreateDTO cheepCreateDto = new CheepCreateDTO("TestAuthor", Message);
        repository.Create(cheepCreateDto);
        List<CheepDTO> cheeps = repository.GetCheeps(1);
        CheepDTO cheep = cheeps[0];

        //Act
        repository.ReCheep(2, cheep);

        //Assert
        repository.GetCountOfAllCheepFromAuthor("TestAuthor2").Should().Be(1);
    }

    [Fact]
    public void UnitTestReCheepMethodOriginalAuthor(){
        //Arrange
        string Message = "TestMessage";
        CheepCreateDTO cheepCreateDto = new("TestAuthor", Message);
        repository.Create(cheepCreateDto);
        List<CheepDTO> cheeps = repository.GetCheeps(1);
        CheepDTO cheep = cheeps[0];

        //Act
        repository.ReCheep(2, cheep);
        AuthorDTO testAuthor = authorRepository.GetAuthorByName("TestAuthor2");
        CheepDTO reCheep = testAuthor.Cheeps[0];

        //Assert
        reCheep.OriginalAuthor.Should().Be("TestAuthor");
    }
}
