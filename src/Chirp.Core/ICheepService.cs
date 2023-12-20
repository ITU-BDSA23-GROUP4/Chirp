using Chirp.Core;

/*
<Summary>
This is the Cheep Service interface
The Cheep Service will implement from this interface.
</Summary>
*/
public interface ICheepService
{
    List<CheepDTO> GetCheeps(int? pageNum);
    List<CheepDTO> GetCheepsFromAuthor(string author, int? pageNum);
    Task<AuthorDTO> GetAuthorByName(string name);
    int GetCountOfAllCheepFromAuthor(string author);
    Task<AuthorDTO> GetAuthorByEmail(string email);
    int GetCountOfAllCheeps();
    int GetCountOfAllCheepsFromCombinedAuthor(string author);
    Task Create(CheepCreateDTO cheep);            
    Task IncreaseLikeAttributeInCheep(Guid cheepId);
    Task AddAuthor(string name, string email);
    Task AddFollowee(string AuthorName, string FolleweeName);
    Task RemoveFollowee(string AuthorName, string FolleweeName);
    Task<bool> DoesAuthorExist(string email);
    List<CheepDTO> CombineCheepsAndFollowerCheeps(string AuthorName, int? pageNum);
    Task DeleteCheepsFromAuthor(Guid authorId);
    Task DeleteAuthor(Guid authorId);
}