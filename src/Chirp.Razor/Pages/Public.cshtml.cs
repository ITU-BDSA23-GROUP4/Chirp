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
  

    [FromQuery(Name = "page")]
    public int? pageNum { get; set; }
    public ActionResult OnGet()
    {  
        if (pageNum.HasValue){
            Cheeps = _service.GetCheeps(pageNum.Value);
        } else {
            Cheeps = _service.GetCheeps(1);
        }

        return Page();
    } 

}
