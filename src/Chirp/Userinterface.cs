using System;
using CheepNS;
public static class Userinterface<T> {
 
    public static void PrintCheeps(IEnumerable<Cheep> cheeps ){
        foreach (var cheep in cheeps)    //Setting the layour for all cheeps
        {
        Console.WriteLine(cheep.ToString());
        // Console.WriteLine("User: " + Cheep.Author);
        // var message = Cheep.Message.Replace("/comma/", ","); //Replaces what we did earlier, for a cleaner output
        // Console.WriteLine(message);

        // // Creates time obejct from unix time stamp and prints it in local time zone
        // DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(Cheep.Timestamp);
        // Console.WriteLine("At time: " + time.LocalDateTime +"\n");
        }
       
    } 
    
}