using System.Security.Cryptography.X509Certificates;
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
                    Author = author,
                    Text = text,
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

    public async Task DeleteCheepsFromAuthor(int authorid){
        //Deletes all cheeps from a given author
        try
        {
            var author = GetAuthorById(authorid); //Gets the author from the database
            if(author != null){ //Deletes all the cheeps from the author
                _db.RemoveRange(_db.Cheeps.Where(cheep => cheep.Author == author));
                await _db.SaveChangesAsync();
            }
        }
        catch (System.Exception)
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
            AuthorId = cheep.Author.AuthorId,
            Author = cheep.Author.Name,
            Message = cheep.Text,
            Timestamp = cheep.TimeStamp
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
            AuthorId = joinResult.Author.AuthorId,
            Author = joinResult.Author.Name,
            Message = joinResult.Cheep.Text,
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
            AuthorId = CheepDTO.Author.AuthorId,
            Author = CheepDTO.Author.Name,
            Message = CheepDTO.Text,
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
                AuthorId = CheepDTO.Author.AuthorId,
                Author = CheepDTO.Author.Name,
                Message = CheepDTO.Text,
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
            Author = user,
            Text = cheep.Text,
            TimeStamp = DateTime.Now
        });
      
        await _db.SaveChangesAsync();
    }
}
