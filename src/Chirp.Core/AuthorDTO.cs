namespace Chirp.Core;
/*
<Summary>
This is the Author DTO
The DTO is the Data Transfer Object, which is used to transfer data between the packages in the domain logic.
</Summary>
*/
public class AuthorDTO
{
    public required Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<CheepDTO>? Cheeps { get; set; }

    public List<AuthorDTO>? Followed { get; set; }
    public List<AuthorDTO>? Followers {get; set; }
}