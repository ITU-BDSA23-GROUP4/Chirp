using System;
using CommandLine;
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

    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CheepOptions, ReadOptions>(args)
        .WithParsed<CheepOptions>(result => {
            if (result.MessageOption != null)
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
            {
                Console.WriteLine(e.Message);
            }
        });
    }

    static void ConstructCheep(string message)
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