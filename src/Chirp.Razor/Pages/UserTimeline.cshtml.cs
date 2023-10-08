using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CheepRecord;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel>? Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    [FromQuery(Name = "page")]
    public int? pageNum { get; set; }

    public ActionResult OnGet(string author)
    {   
        if (pageNum.HasValue) {
            Cheeps = _service.GetCheepsFromAuthor(pageNum.Value, author);
        } else {
            Cheeps = _service.GetCheepsFromAuthor(1, author);
        }
        return Page();
    }
}
