using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Chirp.Infrastructure;
using Chirp.Core;


namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    public int Count { get; set; }
    public readonly ICheepService _service;
    public List<CheepDTO>? Cheeps { get; set; }

    [BindProperty]
    public string Author { get; set; } = "Rasmus";

    [BindProperty, Required, StringLength(160)]
    public string CheepMessage { get; set; } = string.Empty; //= string.Empty; fixes null error

    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    [FromQuery(Name = "page")]
    public int? pageNum { get; set; }
    public ActionResult OnGet()
    {
        if (pageNum.HasValue)
        {
            Cheeps = _service.GetCheeps(pageNum);
        }
        else
        {
            Cheeps = _service.GetCheeps(pageNum);
        }

        return Page();
    }
}
