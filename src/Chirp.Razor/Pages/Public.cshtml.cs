using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.AspNetCore.Authorization;

namespace Chirp.Razor.Pages;


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
            Console.WriteLine("I am the message" + CheepMessageTimeLine);
            var cheep = new CheepCreateDTO(authorRepo.GetAuthorByName(User.Identity.Name).Name, CheepMessageTimeLine);
            cheepRepo.Create(cheep);
            return Redirect(User.Identity.Name);
        }
        catch (Exception)
        {
            return Redirect("/");
        }
    }
}
