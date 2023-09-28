using CheepNS;

public class WebS
{
    //static string path = "../Chirp/chirp_cli_db.csv";
    //static CSVDatabase<Cheep>? DB;

    static void Main(String[] args)
    {
        //DB = CSVDatabase<Cheep>.GetCSVDatabase();
        //DB.SetPath(path);

        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/cheeps", async () => {
            return "Cheep";
            //return await DB.ReadFromFileAsync();
        });

        app.MapPost("/cheep", async (Cheep cheep) => { 
            return "Cheeps";
            //await DB.SaveToFileAsync(cheep);
        });

        app.Run();
    }
}
N