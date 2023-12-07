namespace Chirp.Core;

public class AuthorDTO
{
    public int CheepId { get; set; }
    public required string AuthorName { get; set; }
    public required string Email { get; set; }
    public List<CheepDTO>? Cheeps { get; set; }
    public List<AuthorDTO>? Followed { get; set; }
    public List<AuthorDTO>? Followers { get; set; }
}