using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CheepRecord;
using System.Collections.Specialized;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    public int Count { get; set; }
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }
    public int getPage(){
        return  _service.GetPage();
    }

    public ActionResult OnGet(int? page)
    {  
        // string param = Request.FromQuery("Page");
        Cheeps = _service.GetCheeps(1);
        return Page();
    } 

}
