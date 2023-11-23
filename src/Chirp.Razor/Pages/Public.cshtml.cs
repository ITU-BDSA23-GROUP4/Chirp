using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
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
    public ActionResult OnGet()
    {  
        if (pageNum.HasValue){
            Cheeps = cheepRepo.GetCheeps(pageNum);
        } else {
            Cheeps = cheepRepo.GetCheeps(pageNum);
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
                    var author = authorRepo.GetAuthorByName(userName);
                    if (author != null)
                    {
                        var cheep = new CheepCreateDTO(author.Name, CheepMessageTimeLine);
                        cheepRepo.Create(cheep);
                        return Redirect(userName);
                    }
                }
                catch (Exception)
                {
                    authorRepo.AddAuthor(userName, userEmailClaim.Value);
                    cheepRepo.Create(new CheepCreateDTO(userName, CheepMessageTimeLine));
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
                author = authorRepo.GetAuthorByName(User.Identity.Name);
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
