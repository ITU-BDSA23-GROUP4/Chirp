using System.Linq;
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

    public List<CheepViewModel> GetCheeps(int? pageNum)
    {
        List<CheepViewModel> cheepsToReturn = new List<CheepViewModel>();

        var cheeps = db.Cheeps.OrderByDescending(c=>c.TimeStamp.Ticks).Select(cheep => new CheepViewModel(
            cheep.CheepId,
            cheep.Author.Name,
            cheep.Text,
            cheep.TimeStamp.ToString()
        ));

        cheepsToReturn.AddRange(cheeps);

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

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int? pageNum)
    {
        List<CheepViewModel> cheepsToReturn = new List<CheepViewModel>();

        var cheeps = db.Cheeps.OrderByDescending(c=>c.TimeStamp.Ticks)
            .Where(cheep => cheep.Author != null && cheep.Author.Name != null && cheep.Author.Name.Equals(author))
            .Select(cheep => new CheepViewModel(
                cheep.CheepId,
                cheep.Author.Name,
                cheep.Text,
                cheep.TimeStamp.ToString()
            ));

        cheepsToReturn.AddRange(cheeps);

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
}
