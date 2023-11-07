using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;

namespace Chirp.Razor.Pages;

public class CheepModel : PageModel
{

    CheepRepository cheepRepo = new CheepRepository();
    AuthorRepository authorRepo = new AuthorRepository();


    [BindProperty]
    public string Author { get; set; } = "";

    [BindProperty]
    public string CheepMessage { get; set; } = "";
 
    public async Task<IActionResult> OnPost()
    {
        try
        {
            cheepRepo.AddCheep(authorRepo.GetAuthorByName(Author).AuthorId, CheepMessage);
            return Redirect($"/{Author}");   
        }
        catch (Exception)
        { 
            return Redirect("/");
        }
    }
}
    


   
    


