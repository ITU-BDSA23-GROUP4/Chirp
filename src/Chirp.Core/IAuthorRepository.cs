namespace Chirp.Core
{
    public interface IAuthorRepository
    {
        Task AddAuthor(string name, string email);
        AuthorDTO GetAuthorByID(int ID);
        AuthorDTO GetAuthorByName(string name);
        AuthorDTO GetAuthorByEmail(string email);
        void AddFollowee(string AuthorName, string FolloweeName);
        void RemoveFollowee(string AuthorName, string FolloweeName);
    }
}