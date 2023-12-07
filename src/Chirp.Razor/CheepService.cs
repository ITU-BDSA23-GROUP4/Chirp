using Chirp.Core;

public class CheepService : ICheepService
{
    public readonly ICheepRepository _cheepRepository;
    public readonly IAuthorRepository _authorRepository;

    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    public async Task AddCheep(int authorId, string text)
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

    public async Task IncreaseLikeAttributeInCheep(int cheepId) {
        await _cheepRepository.IncreaseLikeAttributeInCheep(cheepId);
    }

    public async Task AddAuthor(string name, string email)
    {
        await _authorRepository.AddAuthor(name, email);
    }

    public async Task AddFollowee(int AuthorId, int FolloweeId) {
        await _authorRepository.AddFollowee(AuthorId, FolloweeId);
    }

    public async Task RemoveFollowee(int AuthorId, int FolloweeId) {
        await _authorRepository.RemoveFollowee(AuthorId, FolloweeId);
    }

    public async Task<bool?> DoesAuthorExist(string email) {
        return await _authorRepository.DoesAuthorExist(email);
    }
}