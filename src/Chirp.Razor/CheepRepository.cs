using System.Linq;
using CheepDB;
using CheepRecord;
using Initializer;

public class CheepRepository
{
    private ChirpDBContext db;

    public CheepRepository()
    {
        db = new ChirpDBContext();
        DbInitializer.SeedDatabase(db);
        

    }

    public List<CheepViewModel> GetCheeps()
    {
        List<CheepViewModel> cheepsToReturn = new List<CheepViewModel>();

        var cheeps = db.Cheeps.Select(cheep => new CheepViewModel(
            cheep.CheepId,
            cheep.Text,
            cheep.TimeStamp.ToString(),
            cheep.Author.Name
        ));

        cheepsToReturn.AddRange(cheeps);

        return cheepsToReturn;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        List<CheepViewModel> cheepsToReturn = new List<CheepViewModel>();

        var cheeps = db.Cheeps.Select(cheep => new CheepViewModel(
            cheep.CheepId,
            cheep.Text,
            cheep.TimeStamp.ToString(),
            cheep.Author.Name
        )).Where(cheepAuthor => cheepAuthor.Author.Contains(author));

        cheepsToReturn.AddRange(cheeps);

        return cheepsToReturn;
    }
}
