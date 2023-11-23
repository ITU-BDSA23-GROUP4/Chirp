namespace Chirp.Core;

/*Cheep DTO is the information that we want the client to know
In the future, this will by example not include password*/
public class CheepDTO
{
    public required int AuthorId { get; set; }
    public required string Author { get; set; }
    public required string Message { get; set; }
    public required DateTime Timestamp { get; set; }
}
