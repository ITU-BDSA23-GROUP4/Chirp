using CommandLine;
using CheepNS;

namespace CLINS
{
    public class CLI
    {
        static string path = "ccirp_cli_db.csv";   //The file where we store our cheeps¨
        static CSVDatabase<Cheep> DB;

        static void Main(string[] args)
        {

            DB = CSVDatabase<Cheep>.GetCSVDatabase();    //Initializing the database
            DB.SetPath(path);

            Parser.Default.ParseArguments<CheepOptions, ReadOptions>(args)
            .WithParsed<CheepOptions>(result =>
            {
                if (result.MessageOption != null)    //Will see which commands the user are requesting, and then create a cheep from that
                    try
                    {
                        DB.SaveToFile(Cheep.ConstructCheep(result.MessageOption));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                if (result.MessageValue != null)
                    try
                    {
                        DB.SaveToFile(Cheep.ConstructCheep(result.MessageValue));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
            })
            .WithParsed<ReadOptions>(result =>
            {
                try
                {
                    Userinterface<Cheep>.PrintCheeps(DB.ReadFromFile());
                }
                catch (Exception e)
                {
                    ;
                    Console.WriteLine(e.Message);
                }
            });
        }
    }

    [Verb("cheep", HelpText = "Post a cheep.")]
    public class CheepOptions
    {
        [Option('m', "message", HelpText = "Cheep message.")]
        public string MessageOption { get; set; }

        [Value(0, HelpText = "Cheep message.")]
        public string MessageValue { get; set; }
    }


    [Verb("read", HelpText = "Read all cheeps.")]
    public class ReadOptions { }

}