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

            string[] output; //Whole array of the database split by line
            string[] line; //Array that contains everything in the line we are adding to the database
            string fullMessage = ""; //The message that we will later compare

            // Act
            ConstructCheep("***IntegrationTestStatement***");
            output = ExecuteCLICommand("read").Replace("\r", "").Split("\n");
            line = output[output.Length - 2].Split(" "); //Splits the line we are interested in by spaces

            
            for (int i = 4; i < line.Length; i++){  //starts at 4, since that is the array index where the message starts
                Console.WriteLine(line[i]);
                fullMessage += " " + line[i]; //Will keep adding the rest of the message
            }
        
            // Assert
            Assert.Equal("***IntegrationTestStatement***", fullMessage); //Compares the two messages  

    
            // Assert
            //Console.WriteLine(lastCheep);
            //string lastLine = lastCheep[lastCheep.Length-]; // Get the last line from the array
            //Console.WriteLine("THIS IS THE LASTLINE: " + lastLine);
            //Assert.Equal(lastLine.Contains("*** IntegrationTestStatement ***"),true);

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
    static string ExecuteCLICommand(string command) //Takes the command we are using in tests
    {
        using (Process process = new Process())
        {
            //This section specifies all the information needed to run the file
            process.StartInfo.FileName = "dotnet"; // e.g., "dotnet" for .NET CLI
            process.StartInfo.Arguments = "./src/Chirp/bin/Debug/net7.0/chirp.dll " + command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = "../../../../../"; //sets the working directory for the process.

            //Starts the program
            process.Start();

            //Copies the entire output received when running the given command.
            string output = process.StandardOutput.ReadToEnd();

            //Stops the program
            process.WaitForExit();

            return output;
        }
    }
}