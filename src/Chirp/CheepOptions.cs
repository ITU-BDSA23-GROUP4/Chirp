using CommandLine;

namespace CheepOptionsNS 
{
    [Verb("cheep", HelpText = "Post a cheep.")]
    public class CheepOptions
    {
        [Value(0, HelpText = "Cheep message.", Required = true)]
        public string MessageValue { get; set; }
    }
}