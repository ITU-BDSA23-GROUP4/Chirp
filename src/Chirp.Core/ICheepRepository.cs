namespace Chirp.Core
{
    public interface ICheepRepository
    {
        Task AddCheep(Guid authorId, string text);
        List<CheepDTO> GetCheeps(int? pageNum);
        List<CheepDTO> GetCheepsFromAuthor(string author, int? pageNum);
        int GetCountOfAllCheeps();
        int GetCountOfAllCheepFromAuthor(string author);
        Task Create(CheepCreateDTO cheep);
        
    }
}
