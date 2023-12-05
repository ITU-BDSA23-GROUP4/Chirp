namespace Chirp.Core
{
    public interface IAuthorRepository
    {
        Task AddAuthor(string name, string email);
        Task<AuthorDTO> GetAuthorByID(int ID);
        Task<AuthorDTO> GetAuthorByName(string name);
        Task<AuthorDTO> GetAuthorByEmail(string email);
        Task AddFollowee(int AuthorId, int FolloweeId);
        Task RemoveFollowee(int AuthorId, int FolloweeId);
    }
}