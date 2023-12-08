using Chirp.Core;

public interface ICheepService
{
    List<CheepDTO> GetCheeps(int? pageNum);
    List<CheepDTO> GetCheepsFromAuthor(string author, int? pageNum);
    void AddCheep(Guid authorId, string text);
    AuthorDTO GetAuthorByName(string name);
    int GetCountOfAllCheepFromAuthor(string author);
    AuthorDTO GetAuthorByEmail(string email);
    int GetCountOfAllCheeps();
    void Create(CheepCreateDTO cheep);
    Task AddAuthor(string name, string email);
    void AddFollowee(string AuthorName, string FolleweeName);
    void RemoveFollowee(string AuthorName, string FolleweeName);
}