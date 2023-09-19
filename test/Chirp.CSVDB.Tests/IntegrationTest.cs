using System.Diagnostics;
using System;
using Xunit;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

public class IntegrationTest
{
    static string path = "ccirp_cli_db.csv";   //The file where we store our cheeps¨
    static CSVDatabase<Cheep> DB;

    [Fact]
    public void testReadingAndWritingToDatabase(){

            //Arrange
            DB = CSVDatabase<Cheep>.GetCSVDatabase();    //Initializing the database
            DB.SetPath(path);

            //ACT
            ConstructCheep("*** IntegrationTestStatement ***"); //Construct a cheep with the message which also tries to save to
            Console.WriteLine("tester");

            string lastLine = Console.ReadLine();
            Console.WriteLine("This was the last line: " + lastLine);

            Userinterface<Cheep>.PrintCheeps(DB.ReadFromFile());

            //GET LAST NOT FIRST CHEEP
            

            // Assert
            //Assert.StartsWith("nielsfaber", IntegrationCheep);
            //Assert.EndsWith("*** IntegrationTestStatement ***", lastLine);
    }

    //This method should be removed and simpy called from it original source, but since CLI is
    //Protected this is used for the time being.
    private void ConstructCheep(string message)
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