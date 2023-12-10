using Chirp.Core;

public interface ICheepService
{
    List<CheepDTO> GetCheeps(int? pageNum);
    List<CheepDTO> GetCheepsFromAuthor(string author, int? pageNum);
    Task AddCheep(Guid authorId, string text);
    Task<AuthorDTO> GetAuthorByName(string name);
    int GetCountOfAllCheepFromAuthor(string author);
    Task<AuthorDTO> GetAuthorByEmail(string email);
    int GetCountOfAllCheeps();
    Task Create(CheepCreateDTO cheep);            
    Task IncreaseLikeAttributeInCheep(Guid cheepId);
    Task AddAuthor(string name, string email);
    Task AddFollowee(string AuthorName, string FolleweeName);
    Task RemoveFollowee(string AuthorName, string FolleweeName);
    Task<bool?> DoesAuthorExist(string email);
    Task DeleteCheepsFromAuthor(int authorid);
    Task DeleteAuthor(int authorId);
}