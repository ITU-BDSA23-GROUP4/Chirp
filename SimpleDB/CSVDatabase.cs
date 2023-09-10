namespace SimpleDB;
﻿using System;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    static string path = "ccirp_cli_db.csv";
    public void SaveToFile(T item)
    {
         List<T>  AllItems = new List<T>();
        AllItems.Add(item);
        try {
            //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record
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
                csv.WriteRecords(AllItems);;
            }
        }
        catch (Exception e){
            Console.WriteLine(e.Message);
            Console.WriteLine("Can't write to file");
        }
    }

    public List<T> ReadFromFile(T item)
    {
        List<T> AllItems = new List<T>();
        try
        {
            //https://joshclose.github.io/CsvHelper/examples/reading/enumerate-class-records/
            using (StreamReader sr = File.OpenText(path))
            using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
            {
                csv.Read();
                // reading The header and not saving it 
                csv.ReadHeader();
                //Reading whole document. 
                while (csv.Read())
                {
                    var newItem = csv.GetRecord<T>();
                    AllItems.Add(newItem);
                }
                return AllItems;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Can't read from file");
            return null;
        }
    }
}
