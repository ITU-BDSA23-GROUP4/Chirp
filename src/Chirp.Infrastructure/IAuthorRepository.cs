namespace Chirp.Core
{
    public interface IAuthorRepository
    {
        Task AddAuthor(string name, string email);
        AuthorDTO GetAuthorByID(int ID);
        AuthorDTO GetAuthorByName(string name);
        AuthorDTO GetAuthorByEmail(string email);
        void DeleteAuthor(int authorId);
        void DeleteAuthorsFollowing(int authorId);
        void DeleteAuthorsFollowers(int authorId);
        void AddFollowee(int AuthorId, int FolloweeId);
        void RemoveFollowee(int AuthorId, int FolloweeId);
    }
}