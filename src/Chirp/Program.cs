using System;
using System.CommandLine;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

class CLI
{
    static string path = "ccirp_cli_db.csv";
    static CSVDatabase<Cheep> DB = new CSVDatabase<Cheep>(path);


    static void Main(string [] args) {
        
        Console.WriteLine("works");

        // Create commands
        var rootCommand = new RootCommand("chirp");
        var readCommand = new Command("read", "Display all cheeps");
        var cheepCommand = new Command("cheep", "post a cheep");
        //var helpCommand = new Command("help", "Display help");
        rootCommand.AddCommand(readCommand);
        rootCommand.AddCommand(cheepCommand);
        //rootCommand.AddCommand(helpCommand);

        // Create arguments
        var cheepArgument = new Argument<string>("m", "cheep message");
        cheepCommand.AddArgument(cheepArgument);
    
        // Handle commands and arguments
        readCommand.SetHandler(() => {
            Console.WriteLine("test");
            Userinterface<Cheep>.PrintCheeps(DB.ReadFromFile());
        });

    }

/*
    static void Main(string[] args)
    {
        switch (args[0].ToLower())
        {
            case "help":
                Console.WriteLine("Possible commands: cheep, read, help");
                break;
            case "cheep":
                ReadFromCLI(args[1]);
                break;
            case "read":
                try
                {
                    Userinterface<Cheep>.PrintCheeps(DB.ReadFromFile());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                break;
            default:
                break;
        }
    }
*/
    static void ReadFromCLI(string message)
    {
        message = message.Replace(",", "/comma/");
        string author = Environment.UserName;
        // Creates time object with current time in UTC 00. Saves as unix time stamp
        DateTimeOffset time = DateTimeOffset.Now;
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
//Author,Message,Timestamp
public record Cheep
{
    [Index(0)]
    public string Author { get; set; }
    [Index(1)]
    public string Message { get; set; }
    [Index(2)]
    public long Timestamp { get; set; }
}