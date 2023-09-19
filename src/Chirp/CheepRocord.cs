//Author,Message,Timestamp
using System;
using System.Globalization;

namespace CheepNS
{
    public record Cheep
    {
        //[Index(0)]
        public string Author { get; set; }
        //[Index(1)]
        public string Message { get; set; }
        //[Index(2)]
        public long Timestamp { get; set; }
        public override string ToString()
        {
            var printMessage = Message.Replace("/comma/", ","); //Replaces what we did earlier, for a cleaner output
                                                                // Changing cutlture date and time format from this source: https://code-maze.com/csharp-datetime-format/
            return $"{Author} @ {CreateTimeString(Timestamp)}: {printMessage}";
        }

        private string CreateTimeString(long TimeStamp) {
            DateTimeOffset utcTime = DateTimeOffset.FromUnixTimeSeconds(Timestamp);
            return utcTime.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("sw-SW"));
        }
    }
}