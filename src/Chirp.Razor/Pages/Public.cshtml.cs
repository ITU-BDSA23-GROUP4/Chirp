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
        var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");
        if(User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null && userEmailClaim != null)
        {
            try{
                _service.AddAuthor(User.Identity.Name, userEmailClaim.Value);
            } catch (Exception) {
                //Do nothing as the author already exists
            }
        }

        return Page();
    }
    public IActionResult OnPost()
    {
        var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");
        if(User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null && userEmailClaim != null)
        {
            try
            {
                var author = _service.GetAuthorByEmail(userEmailClaim.Value);
                if (author != null)
                {
                    var cheep = new CheepCreateDTO(author.Name, CheepMessageTimeLine);
                    _service.Create(cheep);
                    return Redirect(User.Identity.Name);
                }
            }
            catch (Exception)
            {
                _service.AddAuthor(User.Identity.Name, userEmailClaim.Value);
                _service.Create(new CheepCreateDTO(User.Identity.Name, CheepMessageTimeLine));
                return Redirect(User.Identity.Name);
            }
        }
        return Redirect("/");
    }
}
