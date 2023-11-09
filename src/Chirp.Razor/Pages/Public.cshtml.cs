using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.AspNetCore.Authorization;

namespace Chirp.Razor.Pages;

[AllowAnonymous]
public class PublicModel : PageModel
{

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    public int Count { get; set; }
    // private readonly ICheepService _service;
    [BindProperty]
    public string CheepMessageTimeLine { get; set; } = "";
    public List<CheepDTO>? Cheeps { get; set; }

    public CheepRepository cheepRepo = new CheepRepository();
    public AuthorRepository authorRepo = new AuthorRepository();

    // public PublicModel(ICheepService service)
    // {
    //     _service = service;
    // }

    [FromQuery(Name = "page")]
    public int? pageNum { get; set; }
    public ActionResult OnGet()
    {  
        if (pageNum.HasValue){
            Cheeps = cheepRepo.GetCheeps(pageNum);
        } else {
            Cheeps = cheepRepo.GetCheeps(pageNum);
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        try
        {
            //AUTHOR IS CURRENTLY HARDCODED IN, CHANGE WHEN USERAUTHENTICATION IS IMPLEMENTED
            Console.WriteLine("I am the message" + CheepMessageTimeLine);
            cheepRepo.AddCheep(authorRepo.GetAuthorByName("Rasmus").AuthorId, CheepMessageTimeLine);
            return Redirect($"/{"Rasmus"}");
        }
        catch (Exception)
        {
            return Redirect("/");
        }
    }
}
