using CheepNS;

public class WebS
{
    static string path = "../Chirp/ccirp_cli_db.csv";
    static CSVDatabase<Cheep> DB;

    static void Main(String[] args)
    {
        DB = CSVDatabase<Cheep>.GetCSVDatabase();
        DB.SetPath(path);
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();
        app.MapGet("/cheeps",  () => { GetCheeps();});
        app.MapPost("/cheep",  (Cheep cheep) => {postCheep(cheep);});

        app.Run();
    }

    /// <summary>
    /// From the database at the directory "../Chirp/ccirp_cli_db.csv"
    /// It gets a list of all priviously written cheeps
    /// </summary>
    /// <returns>List<Cheep></returns>
    static IEnumerable<Cheep> GetCheeps()
    {
        return DB.ReadFromFile();
    }

    static void postCheep(Cheep input)
    {
        try
        {
            DB.SaveToFile(input);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
