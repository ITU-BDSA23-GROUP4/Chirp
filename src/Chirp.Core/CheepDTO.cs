namespace Chirp.Core;
/*
<Summary>
This is the Cheep DTO
The DTO is the Data Transfer Object, which is used to transfer data between the client and the server.
</Summary>
*/
public class CheepDTO
{
    public required Guid CheepId { get; set; }
    public required string AuthorName { get; set; }
    public required string Message { get; set; }
    public required int Likes { get; set; }
    public required DateTime Timestamp { get; set; }
}
