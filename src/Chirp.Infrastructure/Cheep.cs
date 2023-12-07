namespace Chirp.Infrastructure;

//<Summary>
//The cheep is the main entity of the application. 
//It is the main object that is being created, read, updated and deleted.
//</Summary>

public class Cheep
{
    //The cheep needs to have an author, a text and a timestamp and should not exitst without any of these.
    public int CheepId { get; set; }
    public required Author Author { get; set; }
    public required string Text { get; set; }
    public required int Likes { get; set; }
    public required DateTime TimeStamp { get; set; }
}
