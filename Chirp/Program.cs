using System.IO;
using System.Text.RegularExpressions;

string path = "";

void saveToFile() {

}

void readFromFile() {
    try {
        var sr = new StreamReader(path);
        while(sr.Peek() >= 0) {
            string line = sr.ReadLine();
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

void readFromCLI() {

}