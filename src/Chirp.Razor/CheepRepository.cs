using CheepDB;
using CheepRecord;
using Initializer;


namespace Repository;

public class CheepRepository
{
    private readonly ChirpDBContext db;



    public CheepRepository()
    {
        db = new ChirpDBContext();
    }

    public CheepRepository(string dbName)
    {
        db = new ChirpDBContext(dbName);
    }

    public void InitDB()
    {
        DbInitializer.SeedDatabase(db);
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
        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheepsDTO = db.Cheeps.OrderByDescending(c => c.TimeStamp.Ticks).Select(CheepDTO => new CheepDTO{
            //cheep.CheepId,
            Author = CheepDTO.Author.Name,
            Message = CheepDTO.Text,
            Timestamp = CheepDTO.TimeStamp.ToString()
        });

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
        List<CheepDTO> cheepsToReturn = new List<CheepDTO>();

        var cheepsDTO = db.Cheeps.OrderByDescending(c => c.TimeStamp.Ticks)
            .Where(cheep => cheep.Author != null && cheep.Author.Name != null && cheep.Author.Name.Equals(author))
            .Select(CheepDTO => new CheepDTO{
            Author = CheepDTO.Author.Name,
            Message = CheepDTO.Text,
            Timestamp = CheepDTO.TimeStamp.ToString()
        });
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
