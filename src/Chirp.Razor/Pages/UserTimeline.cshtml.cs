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
    // private readonly ICheepService _service;
    [BindProperty]
    public string CheepMessageTimeLine { get; set; } = "";
    public List<CheepDTO>? Cheeps { get; set; }

    public CheepRepository cheepRepo = new CheepRepository();
    public AuthorRepository authorRepo = new AuthorRepository();

    [FromQuery(Name = "page")]
    public int? pageNum { get; set; }

    public ActionResult OnGet(string author)
    {
        if (pageNum.HasValue)
        {
            Cheeps = cheepRepo.GetCheepsFromAuthor(author, pageNum);
        }
        else
        {
            Cheeps = cheepRepo.GetCheepsFromAuthor(author, pageNum);
        }
        return Page();
    }
    public IActionResult OnPost()
    {
        try
        {
            if (User?.Identity?.Name != null)
            {
                var author = authorRepo.GetAuthorByName(User.Identity.Name);
                if (author != null)
                {
                    var cheep = new CheepCreateDTO(author.Name, CheepMessageTimeLine);
                    cheepRepo.Create(cheep);
                }
            }

            return Redirect(User?.Identity?.Name ?? "/");
        }
        catch (Exception)
        {
            return Redirect("/");
        }
    }
}
