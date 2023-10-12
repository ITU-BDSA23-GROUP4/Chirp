using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;

namespace Chirp.Razor.Pages;

public class CheepModel : PageModel
{

    CheepRepository cheepRepo = new CheepRepository();

    [BindProperty]
    public CheepMessage cheepMessage { get; set; }
    public ActionResult OnPostSendCheep()
    {
        var cheep = cheepMessage;
        Console.WriteLine($"Author: {cheepMessage.Author}, Message: {cheepMessage.Message}");
        Console.WriteLine(cheep);
        cheepRepo.AddCheep(1111, cheepMessage.Message);
        return Page();
    }
    public class CheepMessage
    {
        public string Author { get; set; }
        public string Message { get; set; }
    }

}
    
    


