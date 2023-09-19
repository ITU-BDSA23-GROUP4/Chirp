using System;

public static class Userinterface<T>
{

    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (var Cheep in cheeps)    //Setting the layour for all cheeps
        {
            var message = Cheep.Message.Replace("/comma/", ","); //Replaces what we did earlier, for a cleaner output
            DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(Cheep.Timestamp);
            System.Console.WriteLine(Cheep.Author + " @ " + time.LocalDateTime + ": " + message);
        }
    }
}