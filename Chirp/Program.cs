using System;

class CLI {
    static string path = "ccirp_cli_db.csv";

    static void Main(string[] args) {
        switch (args[0].ToLower()) {
            case "help":
                Console.WriteLine("Possible commands: cheep, read, help");
                break;
            case "cheep":
                ReadFromCLI(args[1]);
                break;
            case "read":
                ReadFromFile();
                break;
            default:
                break;
        }
    }
    
    static void SaveToFile(string line) {
        try {
            using (StreamWriter sw = File.AppendText(path)) {
                sw.WriteLine(line);
            }
        }
        catch {
            Console.WriteLine("Can't write to file");
        }
    }

    static void ReadFromFile() {
        try {
            using (StreamReader sr = File.OpenText(path)) {
                while (sr.Peek() >= 0) {
                    string line = sr.ReadLine();
                    // First line contains headers and not data. Therfore we skip it
                    if (line.StartsWith("Author,Message,Timestamp"))
                        continue;
                    Userinterface.PrintCheeps(line.Split(","));
                    //PrintToCLI(line.Split(","));
                }
            }
        }
        catch {
            Console.WriteLine("Can't read from file");
        }
    }

    static void ReadFromCLI(string message) {
        message = message.Replace(",", "/comma/");
        string author = Environment.UserName;
        // Creates time object with current time in UTC 00. Saves as unix time stamp
        DateTimeOffset time = DateTimeOffset.Now;    
        SaveToFile(author + "," + message + "," + time.ToUnixTimeSeconds());
    }
}