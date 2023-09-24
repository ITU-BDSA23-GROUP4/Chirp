﻿using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

public sealed class CSVDatabase<T> : IDatabaseRepository<T> //Inherits method from IDatabaseRepository
{
    string path;

    private static readonly CSVDatabase<T> instance = new CSVDatabase<T>();
    private CSVDatabase()    //The class constructor
    {
        this.path = "";
    }

    public static CSVDatabase<T> GetCSVDatabase()
    {
        return instance;
    }

    public void SetPath(string path)
    {
        if (File.Exists(path))
        {
            this.path = path;
        }
        else
        {
            this.path = path;
            try
            {
                // Create the file, or overwrite if the file exists.
                using (StreamWriter sw = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write)))
                {   
                    // Writing header
                    sw.WriteLine("Author,Message,Timestamp");
                }
            } catch (Exception e){
                Console.WriteLine(e.Message);
            }
        }

    }

    public void SaveToFile(T item)    //Method used to save our Cheep in our csv file
    {
        List<T> AllItems = new List<T>();    //Has a list of items (cheeps)
        AllItems.Add(item);     //Adds the item that the method is called with
        try
        {    //Try saving the item (in our case a cheep), to the file that we are using to store. If fail, then catch the exception
             //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record  &&  https://joshclose.github.io/CsvHelper/examples/writing/appending-to-an-existing-file/
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) //Creates the config variable, to later make the use more readable
            {
                HasHeaderRecord = false,   // Don't write the header again.
            };
            using (var stream = File.Open(path, FileMode.Append))   //The stream is used to open our file, so that we can change it
            using (var writer = new StreamWriter(stream))    //The streamWriter will find the path to our file
            using (var csv = new CsvWriter(writer, config))    //Creates a cvs writer, making us able to edit the file itself
            {
                csv.WriteRecords(AllItems);    //Will write our cheeps into the file
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Can't write to file");
        }
    }

    public IEnumerable<T> ReadFromFile()  //Method used to read all Cheeps in our csv file
    {
        List<T> AllItems = new List<T>();    //List of all cheeps, that will handle cheeps in the database as items
        try
        {    //Try reading the file by opening it, and then using a cvs reader - https://joshclose.github.io/CsvHelper/examples/reading/enumerate-class-records/

            using (StreamReader sr = File.OpenText(path))    //Makes a streamreader that takes the path to our cheep records file
            using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))    //Makes a reader, so that the file can be read
            {
                csv.Read();
                csv.ReadHeader();    //Reading The header and not saving it 
                while (csv.Read())   //Reading whole document and adding all cheeps to the list. 
                {
                    var newItem = csv.GetRecord<T>();    //Creates temporary item from the records 
                    AllItems.Add(newItem);    //Adds that item to our list
                }
                return AllItems;    //Will return everything in the csv file
            }

        }
        catch (Exception e)     //If it cant read the document, we wukk give the error message
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Can't read from file");
            return null;
        }
    }

    /* Following two methods is constructed from the answer to this question on stackoverflow:
     https://stackoverflow.com/questions/21700846/how-to-write-an-awaitable-method 
     
     Returning a task allows the caller method to be asynchronous and wait for the returned task to finish
     */
    public Task SaveToFileAsync(T item) {
        return Task.Run(() => SaveToFile(item));
    }

    public Task<IEnumerable<T>> ReadFromFileAsync() {
        return Task.Run(() => ReadFromFile());
    }
}
