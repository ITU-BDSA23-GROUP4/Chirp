using System;
using System.IO;
public class LogFile
{
    private readonly string filePath;

    public LogFile(string fileName)
    {
        // You can specify the directory path if needed
        string directoryPath = "logs";
        Directory.CreateDirectory(directoryPath); // Create the directory if it doesn't exist

        filePath = Path.Combine(directoryPath, fileName);
    }

    public void Log(string message)
    {
        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

        // Append the log entry to the file
        File.AppendAllText(filePath, logEntry + Environment.NewLine);
    }
}