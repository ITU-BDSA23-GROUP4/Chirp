using System;

public static class Userinterface<T> {
 
    public static void PrintCheeps(IEnumerable<Cheep> cheeps ){
        foreach (var Cheep in cheeps)    //Setting the layour for all cheeps
        {
        Console.WriteLine("User: " + Cheep.Author);
        var message = Cheep.Message.Replace("/comma/", ","); //Replaces what we did earlier, for a cleaner output
        Console.WriteLine(message);

        // Creates time obejct from unix time stamp and prints it in local time zone
        DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(Cheep.Timestamp);
        Console.WriteLine("At time: " + time.LocalDateTime +"\n");
        }
       
    } 
    
}