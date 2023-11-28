namespace Chirp.Infrastructure;
public class Author
{
    //As for now, all the variables need to be required since an Author needs to have a name, email and a list of cheeps, even if the list is empty.
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<Cheep> Cheeps;
}
