using Chirp.Core;

/*
<Summary>
This is the Cheep Service
This uses the Cheep Repository and the Author Repository, so in implementation we only need to use this service.
It contains the methods from both the Cheep Repository and the Author Repository.
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

    public async Task AddCheep(Guid authorId, string text)
    {
       await _cheepRepository.AddCheep(authorId, text);
    }

    public List<CheepDTO> GetCheeps(int? pageNum)
    {
        return _cheepRepository.GetCheeps(pageNum);
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int? pageNum)
    {
        return _cheepRepository.GetCheepsFromAuthor(author, pageNum);
    }

    public Task<AuthorDTO> GetAuthorByName(string name)
    {
        return _authorRepository.GetAuthorByName(name);
    }

    public Task<AuthorDTO> GetAuthorByEmail(string email){
        return _authorRepository.GetAuthorByEmail(email);
    }

    public int GetCountOfAllCheepFromAuthor(string author)
    {
        return _cheepRepository.GetCountOfAllCheepFromAuthor(author);
    }

    public int GetCountOfAllCheeps()
    {
        return _cheepRepository.GetCountOfAllCheeps();
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

    public async Task<bool?> DoesAuthorExist(string email) {
        return await _authorRepository.DoesAuthorExist(email);
    }
    public List<CheepDTO> CombineCheepsAndFollowerCheeps(string Authorname, int? pageNum){
        return _cheepRepository.CombineCheepsAndFollowerCheeps(Authorname,pageNum);
    }
}
