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
            
            //temporary code
            if (File.Exists("src/Chirp/chirp_cli_db.csv")) {
            DB.SetPath("src/Chirp/chirp_cli_db.csv");
            }
            else {
                DB.SetPath("chirp_cli_db.csv");
            }
           
            //DB.SetPath(path);

            Parser.Default.ParseArguments<CheepOptions, ReadOptions>(args)
            .WithParsed<CheepOptions>(result =>
            {
                if (result.MessageOption != null)    //Will see which commands the user are requesting, and then create a cheep from that
                    try
                    {
                        DB.SaveToFile(ConstructCheep(result.MessageOption));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                if (result.MessageValue != null)
                    try
                    {
                        DB.SaveToFile(ConstructCheep(result.MessageValue));
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
        public static Cheep ConstructCheep(string message)
        {
            //Replaces the comma to a more readable format in our datafiles
            message = message.Replace(",", "/comma/");
            string author = Environment.UserName;
            // Creates time object with current time in UTC 00. Saves as unix time stamp
            DateTimeOffset time = DateTimeOffset.Now;
            //Returns the cheep
            return new Cheep { Author = author, Message = message, Timestamp = time.ToUnixTimeSeconds() };
        }
    }
    //End of class CLI


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