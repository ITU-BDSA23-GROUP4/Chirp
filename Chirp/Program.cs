using System;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

class CLI {
    static string path = "ccirp_cli_db.csv";

    static void Main(string[] args) {
        switch (args[0].ToLower()) {
            case "help":
                Console.WriteLine("Possible commands: cheep, read, help");
                break;
            case "cheep":
                ReadFromCLI(args[1]);
                break;
            case "read":
                PrintCheeps();
                break;
            default:
                break;
        }
    }
    
    static void SaveToFile(Cheep newCheep) {
        List<Cheep>  Allcheeps = new List<Cheep>();
        Allcheeps.Add(newCheep);
        try {
            //https://joshclose.github.io/CsvHelper/examples/writing/appending-to-an-existing-file/
             var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
            // Don't write the header again.
            HasHeaderRecord = false,
            };
            using (var stream = File.Open(path,FileMode.Append)) 
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(Allcheeps);;
            }
        }
        catch (Exception e){
            Console.WriteLine(e.Message);
            Console.WriteLine("Can't write to file");
        }
    }
    public static void PrintCheeps(){PrintToCLI(ReadFromFile());}
    static List<Cheep> ReadFromFile(){
        List<Cheep>  Allcheeps = new List<Cheep>();
        try {
            //https://joshclose.github.io/CsvHelper/examples/reading/enumerate-class-records/
            using (StreamReader sr = File.OpenText(path)) 
            using ( var csv = new CsvReader(sr,CultureInfo.InvariantCulture)){
            IEnumerable<Cheep> cheeps = csv.EnumerateRecords(new Cheep());
                foreach (Cheep cheep in cheeps){
                    Allcheeps.Add(cheep);
                }
                return Allcheeps;
            }
            
        } catch (Exception e){
            Console.WriteLine(e.Message);
            Console.WriteLine("Can't read from file");
            return null;
        }
    }

    static void PrintToCLI(IEnumerable<Cheep> cheeps) {
        foreach (Cheep cheep in cheeps) {
            Console.WriteLine("User: " +cheep.Author);
            var message = cheep.Message.Replace("/comma/", ",");
            Console.WriteLine(message);
           // Creates time obejct from unix time stamp and prints it in local time zone
            DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp);
            Console.WriteLine("At time: " + time.LocalDateTime);
        }
    }

    static void ReadFromCLI(string message) {
        message = message.Replace(",", "/comma/");
        string author = Environment.UserName;
        // Creates time object with current time in UTC 00. Saves as unix time stamp
        DateTimeOffset time = DateTimeOffset.Now;    
        SaveToFile(new Cheep {Author = author,Message=message, Timestamp = (int)time.ToUnixTimeSeconds()});
    }
}
//Author,Message,Timestamp
public record Cheep{
     [Index( 0)]
    public string Author { get; set;}
    [Index(1)]
    public string Message { get; set;}
    [Index( 2)]
    public int Timestamp {get;set;}
}