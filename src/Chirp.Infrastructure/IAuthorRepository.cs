namespace Chirp.Core
{
    public interface IAuthorRepository
    {
        void AddAuthor(string name, string email);
        AuthorDTO GetAuthorByID(int ID);
        AuthorDTO GetAuthorByName(string name);
        AuthorDTO GetAuthorByEmail(string email);
        void AddFollowee(int AuthorId);
        void RemoveFollowee(int AuthorId);
    }
}