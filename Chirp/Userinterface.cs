using System;
using System.ComponentModel.Design;

public static class Userinterface {

    /* public static void PrintCheeps(IEnumerable<Cheep> cheeps){
        Cheep not yet defined
    } */
    public static void PrintCheeps(String[] line){
        Console.WriteLine("User: " + line[0]);
        var message = line[1].Replace("/comma/", ",");
        Console.WriteLine(message);
        // Creates time obejct from unix time stamp and prints it in local time zone
        DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(long.Parse(line[2]));
        Console.WriteLine("At time: " + time.LocalDateTime);
    }
}