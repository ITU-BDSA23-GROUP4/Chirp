namespace Chirp.Infrastructure;
/*
<Summary>
This class models the Author entity in the database.
This class is also responsible for the creation of the AuthorAuthor entity.
This is due to two list of Author objects, Followed and Follwers,
as EF Core maps this to a many-to-many self referencing relationship.
</Summary>
*/
public class Author
{
    public required Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<Cheep> Cheeps = new List<Cheep>();
    public List<Author>? Followed { get; set;}
    public List<Author>? Followers { get; set; }
}