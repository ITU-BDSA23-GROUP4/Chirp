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

    public static List<CheepViewModel> GetCheeps()
    {
        List<CheepViewModel> cheepsToReturn = new List<CheepViewModel>();

        var cheeps = db.Cheeps.Select(cheep => new CheepViewModel(
            cheep.CheepId,
            cheep.Author.Name,
            cheep.Text,
            cheep.TimeStamp.ToString()
        ));

        cheepsToReturn.AddRange(cheeps);

        return cheepsToReturn;
    }

    public static List<CheepViewModel> GetCheepsFromAuthor(string author)
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

        return cheepsToReturn;
    }
}
