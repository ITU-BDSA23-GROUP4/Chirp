using Chirp.Core;

/*
<Summary>
This is the cheep service interface
The CheepService implements this interface.
The methods are the combined methods of the author and cheep interfaces.
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