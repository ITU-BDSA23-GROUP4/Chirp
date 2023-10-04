using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

    public void AlterPage(int change){
        _service.AlterP(change);
    }

    public ActionResult OnGet(int ? change )
    {
        if (change.HasValue){
               _service.AlterP(change.Value );
        }
        Cheeps = _service.GetCheeps();
        return Page();
    } 
    

    
}
