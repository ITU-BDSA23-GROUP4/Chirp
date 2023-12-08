using Chirp.Core;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Chirp.Infrastructure;
public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _db; //Needed to get our CheepDTO
    private readonly AbstractValidator<CheepCreateDTO> _validator; //Needed to validate our CheepDTO

    public CheepRepository(ChirpDBContext db, AbstractValidator<CheepCreateDTO>? validator) //If we want to use an existing db
    {
        _db = db;
        if (validator == null) {
            throw new NullReferenceException();
        }
        _validator = validator;
    }

    public async Task AddCheep(int authorId, string text)
    {
        try
        {    
            int TLength = text.Length; //Sets a scalable length that we can use for if statement
            var author = GetAuthorById(authorId);
            if (TLength <= 160 && TLength > 0 && author != null)
            {
                _db.Add(new Cheep
                {
                    CheepId = Guid.NewGuid(),
                    Author = author,
                    Text = text,
                    Likes = 0,
                    TimeStamp = DateTime.Now
                });
            }
            else
            {
                throw new ArgumentException("Message is above 160 characters or empty");
            }
            await _db.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public List<CheepDTO> GetCheeps(int? pageNum)
    {
        //Creates a list of max 32 CheepDTO sorted by recent cheep

        List<CheepDTO> cheepsToReturn = new();

        var cheepsDTO = _db.Cheeps.Include(c => c.Author)
        .ToList()
        .OrderByDescending(c => c.TimeStamp.Ticks)
        .Select(cheep => new CheepDTO
        {
            CheepId = cheep.CheepId,
            AuthorName = cheep.Author.Name,
            Message = cheep.Text,
            Likes = cheep.Likes,
            Timestamp = cheep.TimeStamp
        });

        cheepsToReturn.AddRange(cheepsDTO);

        Console.WriteLine("\n\n\n");
        Console.WriteLine("HER:");
        Console.WriteLine("størrelse cheepsToReturn: " + cheepsToReturn.Count);
        Console.WriteLine("størrelse cheepsDTO: " + cheepsDTO.Count());
        Console.WriteLine("\n\n\n");
        
        foreach (CheepDTO cheep in cheepsToReturn) {
            Console.WriteLine(cheep.CheepId + cheep.AuthorName);
        }

        int? page = (pageNum - 1) * 32;

        if (cheepsToReturn.Count < 32)
        {
            return cheepsToReturn.GetRange(0, cheepsToReturn.Count);
        }
        if (page == null)
        {
            return cheepsToReturn.GetRange(0, 32);
        }
        else
        {
            int endIndex = Math.Min((int)page + 32, (int)cheepsToReturn.Count);
            return cheepsToReturn.GetRange((int)page, endIndex - (int)(page));
        }
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int? pageNum)
    {
        //Creates a list of max 32 CheepDTO sorted by recent cheep and only for the given author

        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheepsDTO = _db.Cheeps.ToList()
        .Join(
            _db.Authors,
            cheep => cheep.Author.AuthorId,
            author => author.AuthorId,
            (cheep, author) => new { Cheep = cheep, Author = author }
        )
        .Where(joinResult => joinResult.Author.Name == author)
        .OrderByDescending(joinResult => joinResult.Cheep.TimeStamp)
        .Select(joinResult => new CheepDTO
        {
            //Sets the properties of the Cheep
            CheepId = joinResult.Cheep.CheepId,
            AuthorName = joinResult.Author.Name,
            Message = joinResult.Cheep.Text,
            Likes = joinResult.Cheep.Likes,
            Timestamp = joinResult.Cheep.TimeStamp
        });
        cheepsToReturn.AddRange(cheepsDTO);

        int? page = (pageNum - 1) * 32;

        if (cheepsToReturn.Count < 32)
        {
            return cheepsToReturn.GetRange(0, cheepsToReturn.Count);
        }
        if (page == null)
        {
            return cheepsToReturn.GetRange(0, 32);
        }
        else
        {
            int endIndex = Math.Min((int)page + 32, (int)cheepsToReturn.Count);
            return cheepsToReturn.GetRange((int)page, endIndex - (int)(page));
        }
    }

    //This method is needed for the dynamic buttons as we only have methods for returning 32 cheeps at a time
    public int GetCountOfAllCheeps()
    {
        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheepsDTO = _db.Cheeps.Select(CheepDTO => new CheepDTO
        {
            //Sets the properties of the Cheep
            CheepId = CheepDTO.CheepId,
            AuthorName = CheepDTO.Author.Name,
            Message = CheepDTO.Text,
            Likes = CheepDTO.Likes,
            Timestamp = CheepDTO.TimeStamp
        }
        ).Count();
        return cheepsDTO;
    }

    //This method is needed for the dynamic buttons as we only have methods for returning 32 cheeps at a time
    public int GetCountOfAllCheepFromAuthor(string author)
    {
        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheepsDTO = _db.Cheeps
            .Where(cheep => cheep.Author != null && cheep.Author.Name != null && cheep.Author.Name.Equals(author))
            .Select(CheepDTO => new CheepDTO
            {
                //Sets the properties of the Cheep
                CheepId = CheepDTO.CheepId,
                AuthorName = CheepDTO.Author.Name,
                Message = CheepDTO.Text,
                Likes = CheepDTO.Likes,
                Timestamp = CheepDTO.TimeStamp
            }
        ).Count();
        return cheepsDTO;
    }
    // A method to get an Author class representation with an id
    private Author? GetAuthorById(int id)
    {
        return _db.Authors.Where(author => author.AuthorId == id).FirstOrDefault();
    }
    
    // Code directly from lecture
    public async Task Create(CheepCreateDTO cheep)
    {
        //NullReferenceException is handled in the constructor - CheepRepository()
        ValidationResult result = _validator.Validate(cheep);

        if (!result.IsValid)
        {
            throw new ValidationException();
        }
        
        var user = _db.Authors.Single(u => u.Name == cheep.Author);

        _db.Add(new Cheep
        {
            CheepId = Guid.NewGuid(),
            Author = user,
            Text = cheep.Text,
            Likes = 0,
            TimeStamp = DateTime.Now
        });
      
        await _db.SaveChangesAsync();
    }

    public async Task IncreaseLikeAttributeInCheep(Guid cheepId) {

        var CheepDTO = _db.Cheeps.Where(c => c.CheepId == cheepId)
            .Select(c => new CheepDTO {
                CheepId = c.CheepId,
                AuthorName = c.Author.Name,
                Message = c.Text,
                Likes = c.Likes,
                Timestamp = c.TimeStamp
            }).FirstOrDefault();

        if (CheepDTO != null)
            CheepDTO.Likes++;

        await _db.SaveChangesAsync();
    }
}
