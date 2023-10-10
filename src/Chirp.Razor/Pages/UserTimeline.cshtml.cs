using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CheepRecord;
using Repository;
using CheepDB;

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
            Cheeps = cheepRepo.GetCheepsFromAuthor(author, pageNum);
        }
        else
        {
            Cheeps = cheepRepo.GetCheepsFromAuthor(author, pageNum);
        }
        return Page();
    }
}
