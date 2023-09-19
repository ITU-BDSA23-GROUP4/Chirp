using System;
using CommandLine;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CheepNS;
class CLI
{
    static string path = "ccirp_cli_db.csv";   //The file where we store our cheeps¨
    static CSVDatabase<Cheep> DB;

    static void Main(string[] args)
    {

        DB = CSVDatabase<Cheep>.GetCSVDatabase();    //Initializing the database
        DB.SetPath(path);

        Parser.Default.ParseArguments<CheepOptions, ReadOptions>(args)
        .WithParsed<CheepOptions>(result => {
            if (result.MessageOption != null)    //Will see which commands the user are requesting, and then create a cheep from that
                ConstructCheep(result.MessageOption);
            if (result.MessageValue != null)
                ConstructCheep(result.MessageValue);
            })
        .WithParsed<ReadOptions>(result => {
            try
            {
                Userinterface<Cheep>.PrintCheeps(DB.ReadFromFile());
            }
            catch (Exception e)
            {;
                Console.WriteLine(e.Message);
            }
        });
    }
    static void ConstructCheep(string message)
    {
        message = message.Replace(",", "/comma/"); //Replaces the comma to a more readable format in our datafiles
        string author = Environment.UserName;

        DateTimeOffset time = DateTimeOffset.Now;   // Creates time object with current time in UTC 00. Saves as unix time stamp

        //Tries to add the cheep to the databse, which requires all variabels to be filled. Otherwise it will print error message
        try
        {
            DB.SaveToFile(new Cheep { Author = author, Message = message, Timestamp = time.ToUnixTimeSeconds() });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
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
public class ReadOptions {}


