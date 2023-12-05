public class LogFile
{
    private readonly string filePath;

    public LogFile(string fileName)
    {


        filePath = Path.Combine(fileName);
    }

    public void Log(string message)
    {
        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

        // Append the log entry to the file
        File.AppendAllText(filePath, logEntry + Environment.NewLine);
    }
}