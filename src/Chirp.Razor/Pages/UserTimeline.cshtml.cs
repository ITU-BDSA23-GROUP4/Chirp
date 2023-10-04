using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CheepRecord;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(int author, int ? change)
    {   
        Cheeps = _service.GetCheepsFromAuthor(author);
        if (change.HasValue){
           _service.AlterPage(Cheeps.Count(), change.Value );
        }
        return Page();
    }
      public int getPage(){
        return  _service.GetPage();
    }
}
