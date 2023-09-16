using System.Diagnostics;
using Xunit;
using System;
using System.IO;

namespace Chirp.CLI.Client.Tests;

public class EndToEndTest
{
    [Fact]
    public void TestReadCheep()
    {
        // Arrange
        string expectedOutput = "expected-output-here";

        // Act
        var fstCheep = ExecuteCLICommand().Replace("\r", "").Split("\n");

        // Assert
        Assert.Equal("ropf @ 01-08-2023 14:09:20: Hello, BDSA students!", fstCheep[0]);
        Assert.Equal("rnie @ 02-08-2023 14:19:38: Welcome to the course!", fstCheep[1]);
        Assert.Equal("rnie @ 02-08-2023 14:37:38: I hope you had a good summer.", fstCheep[2]);
        Assert.Equal("ropf @ 02-08-2023 15:04:47: Cheeping cheeps on Chirp :)", fstCheep[3]);
    }

    private string ExecuteCLICommand()
    {
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "dotnet"; // e.g., "dotnet" for .NET CLI
            process.StartInfo.Arguments = "./src/Chirp/bin/Debug/net7.0/chirp.dll read 10";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;

            process.StartInfo.WorkingDirectory = "../../../../../"; //sets the working directory for the process.

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }
    }
}