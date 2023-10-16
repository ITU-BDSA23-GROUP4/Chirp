namespace Chirp.Core;

public class AuthorDTO
{
    public required int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required List<CheepDTO> Cheeps { get; set; }
}