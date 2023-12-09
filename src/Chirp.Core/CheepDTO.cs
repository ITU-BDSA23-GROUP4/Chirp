namespace Chirp.Core;

/*Cheep DTO is the information that we want the client to know
In the future, this will by example not include password*/
public class CheepDTO
{
    public required Guid CheepId { get; set; }
    public required string AuthorName { get; set; }
    public required string Message { get; set; }
    public required int Likes { get; set; }
    public required DateTime Timestamp { get; set; }
}
