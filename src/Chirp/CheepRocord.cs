//Author,Message,Timestamp
using System;
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
        public override string ToString(){
        DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(Timestamp);
        
        return $" {Author} @ {time.LocalDateTime}: {Message} ";
    }
}
}