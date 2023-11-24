using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        
        return Page();
    }

    public IActionResult OnPost()
    {
        // This can be deleted, if refactored into get, and reduce repitative code
        if(User.Identity?.IsAuthenticated == true && User.Identity?.Name != null)
        {
            var userName = User.Identity.Name;
            var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");

            if (userName != null && userEmailClaim != null)
            {
                try
                {
                    var author = _service.GetAuthorByName(userName);
                    if (author != null)
                    {
                        var cheep = new CheepCreateDTO(author.Name, CheepMessageTimeLine);
                        _service.Create(cheep);
                        return Redirect(userName);
                    }
                }
                catch (Exception)
                {
                    _service.AddAuthor(userName, userEmailClaim.Value);
                    _service.Create(new CheepCreateDTO(userName, CheepMessageTimeLine));
                    return Redirect(userName);
                }
            }
        }
        return Redirect("/");
    }

    public bool DoesFollow(int AuthorId) 
    {
        AuthorDTO? author = null;
        // Needs to be refactored into the get method so we does not call it 32 times per page load
        if (User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null) {
            
            if (User.Identity.Name != null) {
                author = _service.GetAuthorByName(User.Identity.Name);
            }
            
            if (author != null && author.Followed != null) {
                foreach (AuthorDTO followingAuthor in author.Followed) {
                    if (followingAuthor.AuthorId == AuthorId) {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
