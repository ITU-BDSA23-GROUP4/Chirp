using Chirp.Core;


namespace Chirp.Infrastructure;

public class CheepRepository
{
    private readonly ChirpDBContext db; //Needed to get our CheepDTO
    private AuthorRepository AuthorRepository = new AuthorRepository(); //Needed to get our AuthorDTO


    public CheepRepository() //Initializes our model
    {
        db = new ChirpDBContext();
    }

    public CheepRepository(string dbName) //If creating a new db is needed
    {
        db = new ChirpDBContext(dbName);
    }

    public void AddCheep(int authorId, string text)
    {
        try
        {
            var author = AuthorRepository.GetAuthorByID(authorId);
            db.Add(new Cheep
            {
                Author = new Author { AuthorId = author.AuthorId, Name = author.Name, Email = author.Email, Cheeps = new List<Cheep>() },
                Text = text,
                TimeStamp = DateTime.Now
            });
        }
        catch (System.Exception)
        {
            throw;
        }

    }

    public List<CheepDTO> GetCheeps(int? pageNum)
    {
        //Creates a list of max 32 CheepDTO sorted by recent cheep

        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

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

    
}
