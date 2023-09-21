using CheepNS;
using CLINS;

public class WebS
{
    static CSVDatabase<Cheep> DB;

    static void main(String[] args)
    {
        DB = CSVDatabase<Cheep>.GetCSVDatabase();
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/cheeps", () => { Cheeps(); });  //readFromFile
        app.MapPost("/cheep", () => { Cheep(); });    //SaveFromFile

        app.Run();
    }

    static void Cheep()
    { //Post
        try
        {
            DB.SaveToFile(Cheep.ConstructCheep());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    static void Cheeps()
    { //get

        Userinterface<Cheep>.PrintCheeps(DB.ReadFromFile());
    }
}
