namespace Chirp.Core;

public class AuthorDTO
{
    public Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<CheepDTO>? Cheeps { get; set; }

    public List<AuthorDTO>? Followed { get; set; }
    public List<AuthorDTO>? Followers {get; set; }
}