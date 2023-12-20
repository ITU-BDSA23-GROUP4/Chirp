using Chirp.Core;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

/*
<Summary>
This is the CheepRepository, where we can work with our cheeps
Here we can create, read, update and delete cheeps, as well as get a list of cheeps from the page or from a specific author.
</Summary>
*/
namespace Chirp.Infrastructure;
public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _db; // The db this repository works on
    private readonly CheepCreateValidator _validator = new(); // Needed to validate our CheepDTO in the Create method
    
    public CheepRepository(ChirpDBContext db)
    {
        _db = db;
    }

    public async Task AddCheep(Guid authorId, string text)
    {
        try
        {    
            int TLength = text.Length;
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

    public async Task DeleteCheepsFromAuthor(Guid authorid){
        try
        {
            var author = GetAuthorById(authorid);
            if(author != null)
            {
                _db.RemoveRange(_db.Cheeps.Where(cheep => cheep.Author == author));
                await _db.SaveChangesAsync();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    // Returns 32 cheeps for a given pagenumber (each webpage displays 32 cheeps)
    public List<CheepDTO> GetCheeps(int? pageNum)
    { 
        List<CheepDTO> cheepsToReturn = new();

        // Retrieve all cheeps, sorted by date and time
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

        // Calculate starting index from the given pagenumber
        int? page = (pageNum - 1) * 32;

        // Skip further calculations if unecessary
        if (cheepsToReturn.Count < 32)
        {
            return cheepsToReturn.GetRange(0, cheepsToReturn.Count);
        }
        if (page == null)
        {
            return cheepsToReturn.GetRange(0, 32);  // Return cheeps for page 1, if no pagenumber is given

        }
        else
        {
            // Calculate ending index given the current page and number of cheeps
            int endIndex = Math.Min((int)page + 32, cheepsToReturn.Count);
            return cheepsToReturn.GetRange((int)page, endIndex - (int)page);
        }
    }

    /* Returns 32 cheeps for a given author and pagenumber 
    (each webpage related to a single author, displays 32 cheeps) */
    public List<CheepDTO> GetCheepsFromAuthor(string authorName, int? pageNum)
    {
        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        // Retrieve all cheeps related to the given author, sorted by date and time
        var cheepsDTO = _db.Cheeps.ToList()
        .Join(
            _db.Authors,
            cheep => cheep.Author.AuthorId,
            author => author.AuthorId,
            (cheep, author) => new { Cheep = cheep, Author = author }
        )
        .Where(joinResult => joinResult.Author.Name == authorName)
        .OrderByDescending(joinResult => joinResult.Cheep.TimeStamp)
        .Select(joinResult => new CheepDTO
        {
            CheepId = joinResult.Cheep.CheepId,
            AuthorName = joinResult.Author.Name,
            Message = joinResult.Cheep.Text,
            Likes = joinResult.Cheep.Likes,
            Timestamp = joinResult.Cheep.TimeStamp
        });
        cheepsToReturn.AddRange(cheepsDTO);

        // Calculate starting index from the given pagenumber
        int? page = (pageNum - 1) * 32;

        // Skip further calculations if unecessary
        if (cheepsToReturn.Count < 32)
        {
            return cheepsToReturn.GetRange(0, cheepsToReturn.Count);
        }
        if (page == null)
        {
            return cheepsToReturn.GetRange(0, 32); // Return cheeps for page 1, if no pagenumber is given
        }
        else
        {
            // Calculate ending index given the current page and number of cheeps
            int endIndex = Math.Min((int)page + 32, cheepsToReturn.Count);
            return cheepsToReturn.GetRange((int)page, endIndex - (int)page);
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

    //This method is used by the razor pages to add dynamic buttons for the pagination
    public int GetCountOfAllCheepFromAuthor(string authorName)
    {
        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheepsDTO = _db.Cheeps
            .Where(cheep => cheep.Author != null && cheep.Author.Name != null && cheep.Author.Name.Equals(authorName))
            .Select(CheepDTO => new CheepDTO
            {
                CheepId = CheepDTO.CheepId,
                AuthorName = CheepDTO.Author.Name,
                Message = CheepDTO.Text,
                Likes = CheepDTO.Likes,
                Timestamp = CheepDTO.TimeStamp
            }
        ).Count();
        return cheepsDTO;
    }

    // Helper method: used by DeleteFromAuthors
    private Author? GetAuthorById(Guid id)
    {
        return _db.Authors.Where(author => author.AuthorId == id).FirstOrDefault();
    }
    
    // Code from the lecture (the Create method to be precise)
    public async Task Create(CheepCreateDTO cheep)
    {
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

    public async Task IncreaseLikeAttributeInCheep(Guid cheepId)
    {
        var cheep = await _db.Cheeps.Where(c => c.CheepId == cheepId).FirstOrDefaultAsync();

        if (cheep != null)
            cheep.Likes++;
        
        await _db.SaveChangesAsync();
    }

    private List<CheepDTO> GetCheepsFromFollowed(string AuthorName)
    {
        List<CheepDTO> cheepsToReturn = new ();
    
        var author = _db.Authors.Where(a => a.Name == AuthorName).Select(authorDTO => new Author
            {
                AuthorId = authorDTO.AuthorId,
                Name = authorDTO.Name,
                Email = authorDTO.Email,
                Followed= authorDTO.Followed
            }).FirstOrDefault();

      
        if (author == null || author.Followed == null)
        {
            throw new NullReferenceException("Author or the Authors List of followers couldn't be found");
        }
        List<AuthorDTO> followed = new List<AuthorDTO>();

        foreach (Author followee in author.Followed) 
        {
            var cheepsFromAuthors = _db.Cheeps.Where(a => a.Author.Name == followee.Name).Select(
                CheepDTO => new CheepDTO
                {
                    CheepId = CheepDTO.CheepId,
                    AuthorName = CheepDTO.Author.Name,
                    Message = CheepDTO.Text,
                    Likes = CheepDTO.Likes,
                    Timestamp = CheepDTO.TimeStamp
                }
            ).ToList();
            cheepsToReturn.AddRange(cheepsFromAuthors);
        }
        return cheepsToReturn;
    } 
    
    private List<CheepDTO> GetAllCheepsFromAuthor(string AuthorName)
    {
        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheepsDTO = _db.Cheeps.ToList()
        .Join(
            _db.Authors,
            cheep => cheep.Author.AuthorId,
            author => author.AuthorId,
            (cheep, author) => new { Cheep = cheep, Author = author }
        )
        .Where(joinResult => joinResult.Author.Name == AuthorName)
        .OrderByDescending(joinResult => joinResult.Cheep.TimeStamp)
        .Select(joinResult => new CheepDTO
        {
            CheepId = joinResult.Cheep.CheepId,
            AuthorName = joinResult.Author.Name,
            Message = joinResult.Cheep.Text,
            Likes = joinResult.Cheep.Likes,
            Timestamp = joinResult.Cheep.TimeStamp
        });
        cheepsToReturn.AddRange(cheepsDTO);

        return cheepsToReturn;
    }

    public List<CheepDTO> CombineCheepsAndFollowerCheeps(string AuthorName, int? pageNum)
    {   
        // Function returns list of cheeps from the logged in user (Author)
        List<CheepDTO> AuthorCheeps = GetAllCheepsFromAuthor(AuthorName);
        // Function returns list of cheeps from all authors the user follows
        List<CheepDTO> FollowedCheeps = GetCheepsFromFollowed(AuthorName);

        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();
        cheepsToReturn.AddRange(AuthorCheeps);
        cheepsToReturn.AddRange(FollowedCheeps);
        
        //OrderByDescending doesn't sort the list but returns a new sequence, therefore we need to assign it to a new list
        var sortedCheeps = cheepsToReturn.OrderByDescending(a => a.Timestamp.Ticks).ToList();
        int? page = (pageNum - 1) * 32;
        
        if (cheepsToReturn.Count < 32)
        {
            return sortedCheeps.GetRange(0, cheepsToReturn.Count);
        } if (page == null)
        {
            return sortedCheeps.GetRange(0, 32);
        } else
        {
            int endIndex = Math.Min((int)page + 32, (int)cheepsToReturn.Count);
            return sortedCheeps.GetRange((int)page, endIndex - (int)(page));
        }
    }

    public int GetCountOfAllCheepsFromCombinedAuthor (string AuthorName)
    {
        List<CheepDTO> AuthorCheeps = GetAllCheepsFromAuthor(AuthorName);
        List<CheepDTO> FollowedCheeps = GetCheepsFromFollowed(AuthorName);
        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();
        cheepsToReturn.AddRange(AuthorCheeps);
        cheepsToReturn.AddRange(FollowedCheeps);
        return cheepsToReturn.Count;
    }
}