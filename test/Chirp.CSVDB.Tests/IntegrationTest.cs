using System.Diagnostics;
using System;
using Xunit;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using CheepNS;

public class IntegrationTest
{
    static string path = "/Users/nielsfaber/Documents/GitHub/Chirp/src/Chirp/ccirp_cli_db.csv";
       //The file where we store our cheeps¨
    static CSVDatabase<Cheep> DB;

    [Fact]
    public void testReadingAndWritingToDatabase(){

            //Arrange
            DB = CSVDatabase<Cheep>.GetCSVDatabase();    //Initializing the database
            DB.SetPath(path);

            // Act
            ConstructCheep("***IntegrationTestStatement***"); //Methods creates a cheep and saves it to the database  
            List<Cheep> CheepList = DB.ReadFromFile(); //Read From 
            
            // Assert
            Assert.Equal(true, CheepList[CheepList.Count-2].ToString().Contains("***IntegrationTestStatement***")); //Compares the two messages  

    }

    /* [Fact]
    public void testThatCheepWithNoMessageDoesntGetSaves(){
            //Arrange
            DB = CSVDatabase<Cheep>.GetCSVDatabase();    //Initializing the database
            DB.SetPath(path);
            List<Cheep> CheepList = DB.ReadFromFile(); //Read From

            //ACT
            string CheepBeforeNewesTest = CheepList[CheepList.Count-2].ToString(); //saves the last Cheep and ocnverts to string
            ConstructCheep(""); //Methods creates an empty cheep and saves it to the database  
            List<Cheep> CheepListAfterNewCheep = DB.ReadFromFile(); //Read From 
            Console.WriteLine(CheepBeforeNewesTest);


            // Assert
            Assert.Equal(true, CheepListAfterNewCheep[CheepListAfterNewCheep.Count-2].ToString().Contains(CheepBeforeNewesTest)); //Compares the two messages  
 //Compares the two messages  

    } */

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