using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;

namespace Chirp.Razor.Pages;

public class CheepModel : PageModel
{

    CheepRepository cheepRepo = new CheepRepository();

    [BindProperty]
    public string Author { get; set; }

    [BindProperty]
    public string CheepMessage { get; set; }
 
    public ActionResult OnPostSubmit()
    {
        Console.WriteLine($"Author: {Author}, Message: {CheepMessage}");

        //Cheep ID is just 1111 for testing, will add real identifaction later
        //cheepRepo.AddCheep(1111, Message);
        return Page();
       
    }
}
    
    


