using Chirp.Core;

public interface ICheepService
{
    List<CheepDTO> GetCheeps(int? pageNum);
    List<CheepDTO> GetCheepsFromAuthor(string author, int? pageNum);
    void AddCheep(int authorId, string text);
    AuthorDTO GetAuthorByName(string name);
    int GetCountOfAllCheepFromAuthor(string author);
    AuthorDTO GetAuthorByEmail(string email);
    int GetCountOfAllCheeps();
    void Create(CheepCreateDTO cheep);
    Task AddAuthor(string name, string email);
    void AddFollowee(int AuthorId, int FolleweeId);
    void RemoveFollowee(int AuthorId, int FolleweeId);
}