using CheepNS;
public static class Userinterface<T> {
 
    public static void PrintCheeps(IEnumerable<Cheep> cheeps ){
        foreach (var cheep in cheeps)    //Setting the layour for all cheeps
        {
        //var message = Cheep.Message.Replace("/comma/", ","); //Replaces what we did earlier, for a cleaner output
        Console.WriteLine(cheep.ToString());
        }
    }
}