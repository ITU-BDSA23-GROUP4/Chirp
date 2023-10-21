using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    // private readonly ICheepService _service;

    public List<CheepDTO>? Cheeps { get; set; }

    CheepRepository cheepRepo = new CheepRepository();

    // public UserTimelineModel(ICheepService service)
    // {
    //     _service = service;
    // }


    [FromQuery(Name = "page")]
    public int? pageNum { get; set; }

    public ActionResult OnGet(string author)
    {
        if (pageNum.HasValue)
        {
            Cheeps = cheepRepo.Pagination(author, pageNum);
        }
        else
        {
            Cheeps = cheepRepo.Pagination(author, pageNum);
        }
        return Page();
    }
}
