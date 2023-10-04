using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CheepRecord;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }
    public int getPage(){
        return  _service.GetPage();
    }

    public ActionResult OnGet(int ? change )
    {
        Cheeps = _service.GetCheeps();
        if (change.HasValue){
               _service.AlterPage(Cheeps.Count(), change.Value );
        }
        return Page();
    } 
    

    
}
