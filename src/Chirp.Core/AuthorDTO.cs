namespace Chirp.Core;

public class AuthorDTO
{
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<CheepDTO> Cheeps { get; set; }
    public required List<AuthorDTO> Following { get; set; }
    public required List<AuthorDTO> Followers { get; set; }
}