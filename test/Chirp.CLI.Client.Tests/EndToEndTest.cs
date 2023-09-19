using System.Diagnostics;
using Xunit;
using System;
using System.IO;

namespace Chirp.CLI.Client.Tests;

public class EndToEndTest
{

    //Testing that the output layout is correct
    [Fact]
    public void TestReadCheep()
    {
        //Arrange
        string[] fstCheep; //Contains every line that the program prints

        //Act
        //runs the read command, deletes whitespaces and splits on each line
        fstCheep = ExecuteCLICommand("read 10").Replace("\r", "").Split("\n");

        //Assert
        //First the message we expect and then compared to the one we are reading
        Assert.Equal("ropf @ 01/08/2023 14.09.20: Hello, BDSA students!", fstCheep[0]);
        Assert.Equal("rnie @ 02/08/2023 14.19.38: Welcome to the course!", fstCheep[1]);
        Assert.Equal("rnie @ 02/08/2023 14.37.38: I hope you had a good summer.", fstCheep[2]);
        Assert.Equal("ropf @ 02/08/2023 15.04.47: Cheeping cheeps on Chirp :)", fstCheep[3]);
    }


    //Testing that the program stores a cheep in the database
    [Fact]
    public void TestStoreCheep()
    {
        // Arrange
        string[] output; //Whole array of the database split by line
        string[] line; //Array that contains everything in the line we are adding to the database
        string fullMessage = ""; //The message that we will later compare

        // Act
        ExecuteCLICommand("cheep Hello!!!"); //Adds the message to the database
        output = ExecuteCLICommand("read").Replace("\r", "").Split("\n");
        line = output[output.Length - 2].Split(" "); //Splits the line we are interested in by spaces

        for (int i = 4; i < line.Length; i++){  //starts at 4, since that is the array index where the message starts
            fullMessage += " " + line[i]; //Will keep adding the rest of the message
        }
    
        // Assert
        Assert.Equal(" Hello!!!", fullMessage); //Compares the two messages
    }


    //Executes the program with the given command
    private string ExecuteCLICommand(string command) //Takes the command we are using in tests
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