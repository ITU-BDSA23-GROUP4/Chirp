using System;

string path = "ccirp_cli_db.csv";

void saveToFile(string line) {
    try {
        var sw = new StreamWriter(path);
        sw.WriteLine(line);
    } catch {
        Console.WriteLine("Can't write to file");
    }
}

void readFromFile() {
    try {
        var sr = new StreamReader(path);
        while(sr.Peek() >= 0) {
            string line = sr.ReadLine();
            // First line contains headers and not data. Therfore we skip it
            if (line.StartsWith("Author,Message,Timestamp"))
                continue;
            printToCLI(line.Split(","));
        }

    } catch {
        Console.WriteLine("Can't read from file");
    }
}

void printToCLI(String[] line) {
    Console.WriteLine("User" + line[0]);
    var message = line[1].Replace("/comma/", ",");
    Console.WriteLine(message);
    Console.WriteLine("At time: " + line[2]);
}

void readFromCLI() {
    Console.WriteLine("Please enter your message");
    string message = Console.ReadLine().Replace(",", "/comma/");
    string author = Environment.UserName;
    // https://www.delftstack.com/howto/csharp/how-to-get-the-unix-timestamp-in-csharp/
    TimeSpan epochTicks = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
    TimeSpan unixTicks = new TimeSpan(DateTime.Now.Ticks) - epochTicks;
    Int32 time = (Int32) unixTicks.TotalSeconds;
    //saveToFile(author + "," + message + "," + time);
    saveToFile("TEst");
}

Console.WriteLine("Possible commands: post, read, quit");

string cmd = Console.ReadLine();
while(!cmd.Equals("quit")) {
    if (cmd.Equals("post")) {
        readFromCLI(); 
    } else if (cmd.Equals("read")) {
        readFromFile();
    } else if (cmd.Equals("quit")) {
        break;
    } else {
        Console.WriteLine("Possible commands: post, read, quit");
    }
    cmd = Console.ReadLine();
}