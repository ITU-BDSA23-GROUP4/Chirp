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
        var author = AuthorRepository.GetAuthorByID(authorId);
        db.Add(new Cheep { Author = new Author {AuthorId = author.AuthorId, Name = author.Name, Email = author.Email, Cheeps = new List<Cheep>() },
                Text = text,
                TimeStamp = DateTime.Now });
        
    }

    public List<CheepDTO> GetCheeps()
    {
        //Creates a list of max 32 CheepDTO sorted by recent cheep

        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheeps = db.Cheeps.OrderByDescending(c => c.TimeStamp.Ticks);
        var cheepDTOList = cheeps.Select(cheep => new CheepDTO
        {
            //Sets the properties of the Cheep
            Author = cheep.Author.Name,
            Message = cheep.Text,
            Timestamp = cheep.TimeStamp
        });
        cheepsToReturn.AddRange(cheepDTOList);

        return cheepsToReturn;


    }
    public List<CheepDTO> GetCheepsFromAuthor(string author)
    {
        //Creates a list of max 32 CheepDTO sorted by recent cheep and only for the given author
        
        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheepsDTO = db.Cheeps.OrderByDescending(c => c.TimeStamp.Ticks)
            .Where(cheep => cheep.Author != null && cheep.Author.Name != null && cheep.Author.Name.Equals(author))
            .Select(CheepDTO => new CheepDTO{
                //Sets the properties of the Cheep
                Author = CheepDTO.Author.Name,
                Message = CheepDTO.Text,
                Timestamp = CheepDTO.TimeStamp
            }
        );
        cheepsToReturn.AddRange(cheepsDTO);

        return cheepsToReturn;
    }
    
    public List<CheepDTO> Pagination(int? pageNum)
    {
        int? page = (pageNum - 1) * 32;


        if(GetCheeps().Count < 32){
            return GetCheeps().GetRange(0, GetCheeps().Count);
        }
        if (page == null)
        {
            return GetCheeps().GetRange(0, 32);
        }
        else
        {
            return GetCheeps().GetRange((int)page, (int)(page + 32));
        }
    }

    public List<CheepDTO> Pagination(string author,int? pageNum)
    {
        int? page = (pageNum - 1) * 32;


        if(GetCheepsFromAuthor(author).Count < 32){
            return GetCheepsFromAuthor(author).GetRange(0, GetCheepsFromAuthor(author).Count);
        }
        if (page == null)
        {
            return GetCheepsFromAuthor(author).GetRange(0, 32);
        }
        else
        {
            return GetCheepsFromAuthor(author).GetRange((int)page, (int)(page + 32));
        }
    }
}
