namespace Chirp.Infrastructure;

/*
<Summary>
This class models the Cheep entity in the database.
A Cheep has a reference to an Author class.
</Summary>
*/
public class Cheep
{
    public required Guid CheepId { get; set; }
    public required Author Author { get; set; }
    public required string Text { get; set; }
    public required int Likes { get; set; }
    public required DateTime TimeStamp { get; set; }
}
