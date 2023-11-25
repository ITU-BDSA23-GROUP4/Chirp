using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class CheepModel : PageModel
{
    private readonly ICheepService _service;

    public CheepModel(ICheepService service){
        _service = service;
    }

    [BindProperty]
    public string Author { get; set; } = "";

    [BindProperty]
    public string CheepMessage { get; set; } = "";
 
    public async Task<IActionResult> OnPost()
    {
        try
        {
            _service.AddCheep(_service.GetAuthorByName(Author).AuthorId, CheepMessage);
            return Redirect($"/{Author}");   
        }
        catch (Exception)
        { 
            return Redirect("/");
        }
    }
}
    


   
    


