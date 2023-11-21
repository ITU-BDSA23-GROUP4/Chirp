using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.AspNetCore.Authorization;

namespace Chirp.Razor.Pages;

[AllowAnonymous]
public class PublicModel : PageModel
{

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    public int Count { get; set; }
    public readonly ICheepService _service;
    
    public PublicModel(ICheepService service)
    {
        _service = service;
    }
    // private readonly ICheepService _service;
    [BindProperty]
    public string CheepMessageTimeLine { get; set; } = "";
    public List<CheepDTO>? Cheeps { get; set; }

    

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

    public IActionResult OnPost()
    {
        try
        {
            Console.WriteLine("I am the message" + CheepMessageTimeLine);
            var cheep = new CheepCreateDTO(_service.GetAuthorByName(User.Identity.Name).Name, CheepMessageTimeLine);
            _service.Create(cheep);
            return Redirect(User.Identity.Name);
        }
        catch (Exception)
        {
            return Redirect("/");
        }
    }
}
