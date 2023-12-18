/*
<Summary>
This is the Cheep Repository interface
The Cheep Repository will implement the methods from this interface.
</Summary>
*/

namespace Chirp.Core
{
    public interface ICheepRepository
    {
        Task AddCheep(Guid authorId, string text);
        List<CheepDTO> GetCheeps(int? pageNum);
        List<CheepDTO> GetCheepsFromAuthor(string author, int? pageNum);
        int GetCountOfAllCheeps();
        int GetCountOfAllCheepFromAuthor(string author);
        int GetCountOfAllCheepsFromCombinedAuthor(string author);
        Task DeleteCheepsFromAuthor(Guid authorid);
        Task Create(CheepCreateDTO cheep);
        Task IncreaseLikeAttributeInCheep(Guid cheepId);
        List<CheepDTO> CombineCheepsAndFollowerCheeps(string AuthorName, int? pageNum);
    }
}
