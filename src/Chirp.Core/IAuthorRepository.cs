/*
<Summary>
This is the author repository interface
The AuthorRepository implements this interface.
The methods handles all interactions with the Author entity,
and the AuthorAthor entity in the database.
</Summary>
*/

namespace Chirp.Core
{
    public interface IAuthorRepository
    {
        Task AddAuthor(string name, string email);
        Task<AuthorDTO> GetAuthorByName(string name);
        Task<AuthorDTO> GetAuthorByEmail(string email);
        Task AddFollowee(string AuthorName, string FolloweeName);
        Task RemoveFollowee(string AuthorName, string FolloweeName);
        Task<bool> DoesAuthorExist(string email);
        Task DeleteAuthor(Guid authorId);
    }
}