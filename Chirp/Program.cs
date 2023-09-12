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
    static CSVDatabase<Cheep> DB = new CSVDatabase<Cheep>(path);


    static void Main(string[] args) {
        switch (args[0].ToLower()) {
            case "help":
                Console.WriteLine("Possible commands: cheep, read, help");
                break;
            case "cheep":
                ReadFromCLI(args[1]);
                break;
            case "read":
            try{
                Userinterface<Cheep>.PrintCheeps(DB.ReadFromFile());
            }
            catch (Exception e){
            Console.WriteLine(e.Message);
            }
                break;
            default:
                break;
        }
    }
    
    static void SaveToFile(Cheep newCheep) {
        List<Cheep>  Allcheeps = new List<Cheep>();
        Allcheeps.Add(newCheep);
        try {
            using (StreamWriter sw = File.AppendText(path)) {
                sw.WriteLine(newCheep);
            }
        }
        catch (Exception e){
            Console.WriteLine(e.Message);
            Console.WriteLine("Can't write to file");
        }
    }
    static List<Cheep> ReadFromFile(){
        List<Cheep>  Allcheeps = new List<Cheep>();
        try {
            //https://joshclose.github.io/CsvHelper/examples/reading/enumerate-class-records/
            using (StreamReader sr = File.OpenText(path)) 
            using ( var csv = new CsvReader(sr,CultureInfo.InvariantCulture)){
            csv.Read();
            // reading The header and not saving it 
            csv.ReadHeader();
            //Reading whole document. 
            while (csv.Read()){
                var cheep = csv.GetRecord<Cheep>();
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

    static void ReadFromCLI(string message) {
        message = message.Replace(",", "/comma/");
        string author = Environment.UserName;
        // Creates time object with current time in UTC 00. Saves as unix time stamp
        DateTimeOffset time = DateTimeOffset.Now;    
        DB.SaveToFile(new Cheep {Author = author, Message=message, Timestamp = (int)time.ToUnixTimeSeconds()});
    }
}
//Author,Message,Timestamp
public record Cheep{
    [Index( 0)]
    public string Author { get; set;}
    [Index(1)]
    public string Message { get; set;}
    [Index( 2)]
    public long Timestamp {get;set;}
}