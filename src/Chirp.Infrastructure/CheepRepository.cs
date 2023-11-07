using Chirp.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;


namespace Chirp.Infrastructure;

public class CheepRepository
{
    private readonly ChirpDBContext db; //Needed to get our CheepDTO
    private readonly CheepCreateValidator?  _validator; //Needed to validate our CheepDTO
    private AuthorRepository AuthorRepository; //Needed to get our AuthorDTO


    public CheepRepository() //Initializes our model
    {
        db = new ChirpDBContext(); 
        AuthorRepository = new AuthorRepository(db);
    }

    public CheepRepository(string dbName) //If creating a new db is needed
    {
        db = new ChirpDBContext(dbName);
        AuthorRepository = new AuthorRepository(db);
    }

    public CheepRepository(ChirpDBContext context, CheepCreateValidator validator) //If we want to use an existing db
    {
        db = context;
        try
        {   if( validator==null){
            throw new NullReferenceException();
        } else {
            _validator = validator;
        }
        }
        catch (System.Exception)
        {
            
            throw;
        }
        AuthorRepository = new AuthorRepository(db);
    }

    public void AddCheep(int authorId, string text)
    {
        try
        {
            int TLength = text.Length; //Sets a scalable length that we can use for if statement
            var author = GetAuthorById(authorId);
            if(TLength <= 160 && TLength > 0 && author != null){
                db.Add(new Cheep
                {
                    Author = author,
                    Text = text,
                    TimeStamp = DateTime.Now
                });
            }else
            {
                throw new ArgumentException("Message is above 160 characters or empty");
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

        var cheepsDTO = db.Cheeps.OrderByDescending(c => c.TimeStamp.Ticks).Select(CheepDTO => new CheepDTO
        {
            //Sets the properties of the Cheep
            AuthorId = CheepDTO.Author.AuthorId,
            Author = CheepDTO.Author.Name,
            Message = CheepDTO.Text,
            Timestamp = CheepDTO.TimeStamp
        }
        );

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

        var cheepsDTO = db.Cheeps.OrderByDescending(c => c.TimeStamp.Ticks)
            .Where(cheep => cheep.Author != null && cheep.Author.Name != null && cheep.Author.Name.Equals(author))
            .Select(CheepDTO => new CheepDTO
            {
                //Sets the properties of the Cheep
                AuthorId = CheepDTO.Author.AuthorId,
                Author = CheepDTO.Author.Name,
                Message = CheepDTO.Text,
                Timestamp = CheepDTO.TimeStamp
            }
        );
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
            return cheepsToReturn.GetRange((int)page, endIndex-(int)(page));
        }
    }

    //This method is needed for the dynamic buttons as we only have methods for returning 32 cheeps at a time
    public int GetCountOfAllCheeps()
    {
        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheepsDTO = db.Cheeps.Select(CheepDTO => new CheepDTO
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

        var cheepsDTO = db.Cheeps
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
        return db.Authors.Where(author => author.AuthorId == id).FirstOrDefault();
    }
    
    // Code directly from lecture
    public async Task Create(CheepCreateDTO cheep)
    {
        //NullReferenceException is handled in the constructor - CheepRepository()
        var validationResult = await _validator.ValidateAsync(cheep);

        if (!validationResult.IsValid)
        {
            throw new ValidationException();
        }

        var user = await db.Authors.SingleAsync(u => u.Name == cheep.Author);

        var entity = new Cheep{
            Text = cheep.Text,
            Author = user,
            TimeStamp = DateTime.Now
        };
    }
}
