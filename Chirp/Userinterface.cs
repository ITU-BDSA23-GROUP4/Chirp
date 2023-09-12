using System;
using System.ComponentModel.Design;

public static class Userinterface<T> {

    public static void PrintCheeps(List<Cheep> cheeps){
        Console.WriteLine("User: " + cheeps.Author);
        var message = cheeps.Message.Replace("/comma/", ",");
        Console.WriteLine(message);
        // Creates time obejct from unix time stamp and prints it in local time zone
        DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(cheeps.Timestamp);
        Console.WriteLine("At time: " + time.LocalDateTime);
    } 
    
}