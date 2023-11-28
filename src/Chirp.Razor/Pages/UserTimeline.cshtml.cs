using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;
using Microsoft.AspNetCore.Authorization;
namespace Chirp.Razor.Pages;

[AllowAnonymous]
public class UserTimelineModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    public int Count { get; set; }
    public readonly ICheepService _service;

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    // private readonly ICheepService _service;
    [BindProperty]
    public string CheepMessageTimeLine { get; set; } = "";
    public List<CheepDTO>? Cheeps { get; set; }

    [FromQuery(Name = "page")]
    public int? pageNum { get; set; }

    public ActionResult OnGet(string author)
    {
        if (pageNum.HasValue)
        {
            Cheeps = _service.GetCheepsFromAuthor(author, pageNum);
        }
        else
        {
            Cheeps = _service.GetCheepsFromAuthor(author, pageNum);
        }
        return Page();
    }
    public IActionResult OnPost()
    {
        if (User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null)
        {
            var userName = User.Identity.Name;
            var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");

            try
            {
                var author = _service.GetAuthorByName(userName);
                var cheep = new CheepCreateDTO(author.Name, CheepMessageTimeLine);
                _service.Create(cheep);
                return Redirect(userName);
            }
            catch
            {
                if(userEmailClaim != null ) {
                _service.AddAuthor(userName, userEmailClaim.Value);
                _service.Create(new CheepCreateDTO(userName, CheepMessageTimeLine));
                return Redirect(userName);
                }
            }
        }
        return Redirect("/");
    }
}


