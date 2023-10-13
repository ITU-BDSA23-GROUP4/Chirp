<<<<<<< Updated upstream
using Chirp.Core;


namespace Chirp.Infrastructure;

public class CheepRepository
{
    private readonly ChirpDBContext db; //Needed to get our CheepDTO



    public CheepRepository() //Initializes our model
    {
        db = new ChirpDBContext();
    }

    public CheepRepository(string dbName) //If creating a new db is needed
    {
        db = new ChirpDBContext(dbName);
    }

    public void AddAuthor(string name, string email)
    {
        db.Add(new Author {Name = name, Cheeps = new List<Cheep>(), Email = email});
    }

    public void AddCheep(int authorId, string text) 
    {
        db.Add(new Cheep { Author = db.Authors.
            Where(author => author.AuthorId == authorId).First(),
                Text = text,
                TimeStamp = DateTime.Now });
    }

    public List<CheepDTO> GetCheeps(int? pageNum)
    {
        //Creates a list of max 32 CheepDTO sorted by recent cheep

        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheepsDTO = db.Cheeps.OrderByDescending(c => c.TimeStamp.Ticks).Select(CheepDTO => new CheepDTO{
            //Sets the properties of the Cheep
            AuthorId = CheepDTO.Author.AuthorId,
            Author = CheepDTO.Author.Name,
            Message = CheepDTO.Text,
            Timestamp = CheepDTO.TimeStamp.ToString()
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
            return cheepsToReturn.GetRange((int)page, (int)(page + 32));
        }


    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int? pageNum)
    {
        //Creates a list of max 32 CheepDTO sorted by recent cheep and only for the given author
        
        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheepsDTO = db.Cheeps.OrderByDescending(c => c.TimeStamp.Ticks)
            .Where(cheep => cheep.Author != null && cheep.Author.Name != null && cheep.Author.Name.Equals(author))
            .Select(CheepDTO => new CheepDTO{
                //Sets the properties of the Cheep
                AuthorId = CheepDTO.Author.AuthorId,
                Author = CheepDTO.Author.Name,
                Message = CheepDTO.Text,
                Timestamp = CheepDTO.TimeStamp.ToString()
            }
        );
        cheepsToReturn.AddRange(cheepsDTO);

        int? page = (pageNum - 1) * 32;


        if(cheepsToReturn.Count < 32){
            return cheepsToReturn.GetRange(0, cheepsToReturn.Count);
        }
        if (page == null)
        {
            return cheepsToReturn.GetRange(0, 32);
        }
        else
        {
            return cheepsToReturn.GetRange((int)page, (int)(page + 32));
        }
    }
}
=======
using Chirp.Core;


namespace Chirp.Infrastructure;

public class CheepRepository
{
    private readonly ChirpDBContext db; //Needed to get our CheepDTO



    public CheepRepository() //Initializes our model
    {
        db = new ChirpDBContext();
    }

    public CheepRepository(string dbName) //If creating a new db is needed
    {
        db = new ChirpDBContext(dbName);
    }

    public void AddAuthor(string name, string email)
    {
        db.Add(new Author {Name = name, Cheeps = new List<Cheep>(), Email = email});
    }

    public void AddCheep(int authorId, string text) 
    {
        db.Add(new Cheep { Author = db.Authors.
            Where(author => author.AuthorId == authorId).First(),
                Text = text,
                TimeStamp = DateTime.Now });
    }

    public List<CheepDTO> GetCheeps(int? pageNum)
    {
        //Creates a list of max 32 CheepDTO sorted by recent cheep

        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheepsDTO = db.Cheeps.OrderByDescending(c => c.TimeStamp.Ticks).Select(CheepDTO => new CheepDTO{
            //Sets the properties of the Cheep
            AuthorId = CheepDTO.Author.AuthorId,
            Author = CheepDTO.Author.Name,
            Message = CheepDTO.Text,
            Timestamp = CheepDTO.TimeStamp.ToString()
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
            Console.WriteLine("\n \n \n \n");
            Console.WriteLine(" Page " + page.Value + " page max " + (page + 32));
            Console.WriteLine("\n \n \n \n");
            return cheepsToReturn.GetRange((int)page, (int)(page + 32));
        }


    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int? pageNum)
    {
        //Creates a list of max 32 CheepDTO sorted by recent cheep and only for the given author
        
        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheepsDTO = db.Cheeps.OrderByDescending(c => c.TimeStamp.Ticks)
            .Where(cheep => cheep.Author != null && cheep.Author.Name != null && cheep.Author.Name.Equals(author))
            .Select(CheepDTO => new CheepDTO{
                //Sets the properties of the Cheep
                AuthorId = CheepDTO.Author.AuthorId,
                Author = CheepDTO.Author.Name,
                Message = CheepDTO.Text,
                Timestamp = CheepDTO.TimeStamp.ToString()
            }
        );
        cheepsToReturn.AddRange(cheepsDTO);

        int? page = (pageNum - 1) * 32;

        if(cheepsToReturn.Count < 32){
            return cheepsToReturn.GetRange(0, cheepsToReturn.Count);
        }
        if (page == null)
        {
            return cheepsToReturn.GetRange(0, 32);
        }
        else
        {   
            return cheepsToReturn.GetRange((int)page, (int)(page + 32));
        }
    }
}
>>>>>>> Stashed changes
