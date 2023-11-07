namespace Chirp.Infrastructure;

public class Cheep
{
    //The cheep needs to have an author, a text and a timestamp and should not exitst without any of these.
    public int CheepId { get; set; }
    public required Author Author { get; set; }
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
}
