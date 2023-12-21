using Chirp.Core;

/*
<Summary>
This is the CheepService

It combines the two IAuthorRepository interface and the ICheepRepository into one.
It implements the ICheepService, and implements the methods, calling method,
from the two referenced interfaces.
</Summary>
*/

public class CheepService : ICheepService
{
    public readonly ICheepRepository _cheepRepository;
    public readonly IAuthorRepository _authorRepository;

    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    public List<CheepDTO> GetCheeps(int? pageNum)
    {
        return _cheepRepository.GetCheeps(pageNum);
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int? pageNum)
    {
        return _cheepRepository.GetCheepsFromAuthor(author, pageNum);
    }

    public async Task<AuthorDTO> GetAuthorByName(string name)
    {
        return await _authorRepository.GetAuthorByName(name);
    }

    public async Task<AuthorDTO> GetAuthorByEmail(string email){
        return await _authorRepository.GetAuthorByEmail(email);
    }

    public int GetCountOfAllCheepFromAuthor(string author)
    {
        return _cheepRepository.GetCountOfAllCheepFromAuthor(author);
    }

    public int GetCountOfAllCheeps()
    {
        return _cheepRepository.GetCountOfAllCheeps();
    }

    public int GetCountOfAllCheepsFromCombinedAuthor(string author)
    {
        return _cheepRepository.GetCountOfAllCheepsFromCombinedAuthor(author);
    }
    

    public async Task Create(CheepCreateDTO cheep)
    {
        await _cheepRepository.Create(cheep);
    }

    public async Task IncreaseLikeAttributeInCheep(Guid cheepId) {
        await _cheepRepository.IncreaseLikeAttributeInCheep(cheepId);
    }

    public async Task AddAuthor(string name, string email)
    {
        await _authorRepository.AddAuthor(name, email);
    }   
    public async Task DeleteCheepsFromAuthor(Guid authorid){
       await _cheepRepository.DeleteCheepsFromAuthor(authorid);
    }
    public async Task DeleteAuthor(Guid authorId){
       await _authorRepository.DeleteAuthor(authorId);
    }

    public async Task AddFollowee(string AuthorName, string FolloweeName) {
        await _authorRepository.AddFollowee(AuthorName, FolloweeName);
    }

    public async Task RemoveFollowee(string AuthorName, string FolloweeName) {
        await _authorRepository.RemoveFollowee(AuthorName, FolloweeName);
    }

    public async Task<bool> DoesAuthorExist(string email) {
        return await _authorRepository.DoesAuthorExist(email);
    }
    public List<CheepDTO> CombineCheepsAndFollowerCheeps(string Authorname, int? pageNum){
        return _cheepRepository.CombineCheepsAndFollowerCheeps(Authorname,pageNum);
    }
}
