
using System.Collections.Generic;
using System.Threading.Tasks;
using Chirp.Infrastructure;
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
    void DeleteCheepsFromAuthor(int authorid);
    void DeleteAuthor(int authorId);
    Task AddAuthor(string name, string email);
    void AddFollowee(int AuthorId, int FolleweeId);
    void RemoveFollowee(int AuthorId, int FolleweeId);
}
public class CheepService : ICheepService
{
    public readonly ICheepRepository _cheepRepository;
    public readonly IAuthorRepository _authorRepository;

    public CheepService(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    public void AddCheep(int authorId, string text)
    {
        _cheepRepository.AddCheep(authorId, text);
    }

    public List<CheepDTO> GetCheeps(int? pageNum)
    {
        return _cheepRepository.GetCheeps(pageNum);
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int? pageNum)
    {
        return _cheepRepository.GetCheepsFromAuthor(author, pageNum);
    }

    public AuthorDTO GetAuthorByName(string name)
    {
        return _authorRepository.GetAuthorByName(name);
    }

    public AuthorDTO GetAuthorByEmail(string email){
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

    public void Create(CheepCreateDTO cheep)
    {
        _cheepRepository.Create(cheep);
    }
    public async Task AddAuthor(string name, string email)
    {
        _authorRepository.AddAuthor(name, email);
    }   
    public void DeleteCheepsFromAuthor(int authorid){
        _cheepRepository.DeleteCheepsFromAuthor(authorid);
    }
    public void DeleteAuthor(int authorId){
       _authorRepository.DeleteAuthor(authorId);
    }
    public void AddFollowee(int AuthorId, int FolloweeId) {
        _authorRepository.AddFollowee(AuthorId, FolloweeId);
    }

    public void RemoveFollowee(int AuthorId, int FolloweeId) {
        _authorRepository.RemoveFollowee(AuthorId, FolloweeId);
    }
}