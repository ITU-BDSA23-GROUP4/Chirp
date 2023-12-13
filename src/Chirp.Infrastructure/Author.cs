namespace Chirp.Infrastructure;
/*
<Summary>
Author is another main entity of the application
The main object Cheep will be created by an Author. 
These authors can follow each other and see each others cheeps. as well as their own.
</Summary>
*/
public class Author
{
    //As for now, all the variables need to be required since an Author needs to have a name, email and a list of cheeps, even if the list is empty.
    public required Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<Cheep> Cheeps = new List<Cheep>();
    public List<Author>? Followed { get; set;}
    public List<Author>? Followers { get; set; }
}