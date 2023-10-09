using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CheepRecord;
using Repository;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    // private readonly ICheepService _service;

    public List<CheepViewModel>? Cheeps { get; set; }

    // public UserTimelineModel(ICheepService service)
    // {
    //     _service = service;
    // }

    [FromQuery(Name = "page")]
    public int? pageNum { get; set; }

    public ActionResult OnGet(string author)
    {   
        if (pageNum.HasValue) {
            Cheeps = CheepRepository.GetCheepsFromAuthor(author, pageNum);
        } else {
            Cheeps = CheepRepository.GetCheepsFromAuthor(author, pageNum);
        }
        return Page();
    }
}
