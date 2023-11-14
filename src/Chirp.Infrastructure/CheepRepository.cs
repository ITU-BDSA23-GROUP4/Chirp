using Chirp.Core;
using Microsoft.EntityFrameworkCore;


namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _db; //Needed to get our CheepDTO
    private AuthorRepository AuthorRepository; //Needed to get our AuthorDTO

    public CheepRepository(ChirpDBContext db) //Initializes our model
    {
        _db = db;
        AuthorRepository = new AuthorRepository(db);
    }

    public void AddCheep(int authorId, string text)
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
            _db.SaveChanges();
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

        // Print out the number of cheeps in the database
        Console.WriteLine("Checking number of cheeps in the database");
        Console.WriteLine("Number of cheeps in the database: " + cheepsDTO.Count());

        cheepsToReturn.AddRange(cheepsDTO);

        int? page = (pageNum - 1) * 32;

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

        Console.WriteLine("The nummber of cheeps from " + author + " is: " + cheepsDTO.Count());


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
        Console.WriteLine("The nummber of cheeps from " + author + " is: " + cheepsDTO);
        return cheepsDTO;
    }
    // A method to get an Author class representation with an id
    private Author? GetAuthorById(int id)
    {
        return _db.Authors.Where(author => author.AuthorId == id).FirstOrDefault();
    }

}
