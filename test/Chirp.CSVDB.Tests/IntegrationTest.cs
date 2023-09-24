using System.Diagnostics;
using System;
using Xunit;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using CheepNS;
using CsvHelper.Configuration;
using System.Globalization;
using CsvHelper;

public class IntegrationTest
{
    static CSVDatabase<Cheep> DB;

    [Fact]
    public void testReadingAndWritingToDatabase(){

            //Arrange
            string path = "../../../../../src/Chirp/ccirp_cli_Testdb.csv";
            //The file where we store our test-cheeps¨ we need to go back a few times since the wdr is in the bin folder

            SetUpTestDatabase(path); //method that setsup a testdatabase

            //Checks if database was created correctly
            if(File.Exists(path)){
                Console.WriteLine("Path exists: " + path);
            } else {
                Console.WriteLine("No database");
            }

            // Act
            ConstructCheep("***IntegrationTestStatement***"); //Methods creates a cheep and saves it to the database  
            List<Cheep> CheepList = DB.ReadFromFile(); //Reads all the cheeps in the database and returns a list of them
            Console.WriteLine("Last Cheep : " + CheepList[CheepList.Count-1].ToString());
            Console.WriteLine("Size of list: " + CheepList.Count);
            foreach (var item in CheepList)
            {
                Console.WriteLine(item.ToString());
            }

            // Assert
            Assert.Equal(true, CheepList[CheepList.Count-1].ToString().Contains("***IntegrationTestStatement***")); //Compares the two messages  
            
            //Deletes the testDatabase, this step should only be done when using test database
            try{
                File.Delete(path);
            }
            catch (Exception ex){
                Console.WriteLine($"Error deleting the file: {ex.Message}");
            }
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

    //helper method for the test
    private void SetUpTestDatabase(string path){
            DB = CSVDatabase<Cheep>.GetCSVDatabase();    //Initializing the database
            DB.SetPath(path);

            //Create header for the testdatabase, otherwise  DB.ReadFromFile() will make the program crash
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = false, // Disable automatic header generation
            };

            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                // Write the custom header line
                writer.WriteLine("Author,Message,Timestamp");
            }
    }
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