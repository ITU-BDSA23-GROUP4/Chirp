using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.AspNetCore.Authorization;
namespace Chirp.Razor.Pages;

[AllowAnonymous]
public class UserTimelineModel : PageModel
{
    // private readonly ICheepService _service;

    public List<CheepDTO>? Cheeps { get; set; }

    public CheepRepository cheepRepo = new CheepRepository();

    [FromQuery(Name = "page")]
    public int? pageNum { get; set; }

    public ActionResult OnGet(string author)
    {
        if (pageNum.HasValue)
        {
            Cheeps = cheepRepo.GetCheepsFromAuthor(author, pageNum);
        }
        else
        {
            Cheeps = cheepRepo.GetCheepsFromAuthor(author, pageNum);
        }
        return Page();
    }
}
