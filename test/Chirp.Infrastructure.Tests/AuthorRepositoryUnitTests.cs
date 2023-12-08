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
        var testAuthor2 = new Author
        {
            AuthorId = 2,
            Name = "TestName2",
            Email = "TestEmail2",
            Cheeps = new List<Cheep>(),
            Followed = new List<Follow>(),
            Followers = new List<Follow>()
        };
        var testAuthor3 = new Author {
            AuthorId = 3,
            Name = "TestName3",
            Email = "TestEmail3",
            Cheeps = new List<Cheep>(),
            Followed = new List<Follow>(),
            Followers = new List<Follow>()
        };

        //Creates and adds aauthor to the database
        context.Authors.Add(testAuthor);
        context.Authors.Add(testAuthor2);
        context.Authors.Add(testAuthor3);

        context.SaveChanges();
        _context = context;
        repository = new AuthorRepository(_context);
    }

    [Fact] //Test the method to get author by email - should be possible
    public async void UnitTestFindAuthorByEmail()
    {
        //Act
        var author = await repository.GetAuthorByEmail("TestEmail");

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
        Func<Task> act = async () => await repository.GetAuthorByEmail("TestEmailWrong");

        //Assert
        //Should throw an exception since the email doesn't exist in our database
        act.Should().ThrowAsync<ArgumentException>().WithMessage("Author with email TestEmailWrong does not exist");
    }

    [Fact] //Test method to get Author by name - should be possible
    public async void UnitTestFindAuthorByName()
    {
        //Act
        var author = await repository.GetAuthorByName("TestName");

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
        Func<Task> act = async () => await repository.GetAuthorByName("TestNameWrong");

        //Assert
        //Should throw an exception since the name doesn't exist in our database
        act.Should().ThrowAsync<ArgumentException>().WithMessage("Author with name TestNameWrong does not exist");
    }

    [Fact] //Test method to get an author by id - should be possible
    public async void UnitTestFindAuthorById()
    {
        //Act
        var author = await repository.GetAuthorByID(1);

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
        Func<Task> act = async () => await repository.GetAuthorByID(4);

        //Assert
        //Should throw an exception since the id doesn't exist in our database
        act.Should().ThrowAsync<ArgumentException>().WithMessage("Author with id 4 does not exist");
    }

    [Fact]
    public async void UnitTestAddFolloweeAddsToTheAuthorsFollowedList(){
        //Arrange
        //Act
        await repository.AddFollowee(1,2);
        AuthorDTO author1 = await repository.GetAuthorByID(1);

        //Assert
        author1.Followed?[0].AuthorId.Should().Be(2);
    }

    [Fact]
    public void UnitTestAddIncorrectFollowee(){
        //Arrange
        //Act
        Func<Task> act = async () => await repository.AddFollowee(1,4);
        
        //Assert
        act.Should().ThrowAsync<NullReferenceException>().WithMessage("Obejct _Author or _Followee of type Author is null");
    }

    [Fact]
    public async void UnitTestRemoveFolloweeRemovesFromTheAuthorsFollowedList() {
        // Arrange
        await repository.AddFollowee(1,2);

        //Act
        await repository.RemoveFollowee(1,2);
        AuthorDTO author1 = await repository.GetAuthorByID(1);

        //Assert
        author1.Followed?.Count.Should().Be(0);
    }

    [Fact]
    public void UnitTestRemoveIncorrectFollowee() {
        //Act
        Func<Task> act = async () => await repository.RemoveFollowee(1,4);
        //Assert
        act.Should().ThrowAsync<NullReferenceException>().WithMessage("FollowerRelationship is null");
    }

    [Fact]
    public async void UnitTestFollowTheSameAuthorTwice() {
        //Act
        await repository.AddFollowee(1,2);
        Func<Task> act = async () => await repository.AddFollowee(1,2);

        //Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async void UnitTestFollowAuthorUpdatesOtherAuthorsFollowersList() {
        //Act
        await repository.AddFollowee(1,2);
        AuthorDTO author2 = await repository.GetAuthorByID(2);

        //Assert
        author2.Followers?[0].AuthorId.Should().Be(1);
    }

    [Fact]
    public async void UnitTestUnFollowAuthorUpdatesOtherAuthorsFollowersList() {
        // Arrange
        await repository.AddFollowee(1,2);

        //Act
        await repository.RemoveFollowee(1,2);
        AuthorDTO author = await repository.GetAuthorByID(2);
        
        // Assert
        author.Followers?.Count.Should().Be(0);
    }

    [Fact]
    public async void UnitTestAddFollowDoesNotAffactOtherAuthors() {
        //Act
        await repository.AddFollowee(1,2);

        //Assert
        AuthorDTO author = await repository.GetAuthorByID(3);

        author.Followed?.Count.Should().Be(0);
        author.Followers?.Count.Should().Be(0);
    }

    [Fact]
    public async void UnitTestEnsureFollowAndFollowedListAreEmptyBeDefault() {
        //Assert
        AuthorDTO author = await repository.GetAuthorByID(1);

        author.Followed?.Count.Should().Be(0);
        author.Followers?.Count.Should().Be(0);
    }

    [Fact]
    public async void UnitTestAddFollowDoesNotAffectWrongList() {
        //Act
        await repository.AddFollowee(1,2);

        //Assert
        AuthorDTO author1 = await repository.GetAuthorByID(1);
        AuthorDTO author2 = await repository.GetAuthorByID(2);

        author1.Followers?.Count.Should().Be(0);
        author2.Followed?.Count.Should().Be(0);
    }
}