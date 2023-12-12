/*
<Summary>
This is the author Repository interface
The Author Repository will inherit from this interface, and implement the methods.
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
        Task<bool?> DoesAuthorExist(string email);
    }
}