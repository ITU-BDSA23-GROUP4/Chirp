using System.Linq;
using CheepDB;
using CheepRecord;
using Initializer;


namespace Repository;

public static class CheepRepository
{
    private static readonly ChirpDBContext db;

    static CheepRepository()
    {
        db = new ChirpDBContext();



    }

    public static void InitDB()
    {
        DbInitializer.SeedDatabase(db);
    }

    public static List<CheepViewModel> GetCheeps(int? pageNum)
    {
        List<CheepViewModel> cheepsToReturn = new List<CheepViewModel>();

        var cheeps = db.Cheeps.Select(cheep => new CheepViewModel(
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

    public static List<CheepViewModel> GetCheepsFromAuthor(string author, int? pageNum)
    {
        List<CheepViewModel> cheepsToReturn = new List<CheepViewModel>();

        var cheeps = db.Cheeps
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
