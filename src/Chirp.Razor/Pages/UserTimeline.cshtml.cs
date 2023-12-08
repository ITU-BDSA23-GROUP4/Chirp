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

    [FromQuery(Name ="follow")]
    public string? follow{ get; set; }

    [FromQuery(Name ="unfollow")]
    public string? unfollow{ get; set; }

    public async Task<ActionResult> OnGet(string author)
    {
        if (pageNum.HasValue)
        {
            Cheeps = _service.GetCheepsFromAuthor(author, pageNum);
        }
        else
        {
            Cheeps = _service.GetCheepsFromAuthor(author, pageNum);
        }

        if (User.Identity?.IsAuthenticated == true  && User.Identity.Name != null) {
            AuthorDTO currentUser = await _service.GetAuthorByName(User.Identity.Name);
            if (follow != null) 
            {
                await _service.AddFollowee(currentUser.Name, follow);
            } 
            else if (unfollow != null) 
            {
                await _service.RemoveFollowee(currentUser.Name, unfollow);
            }
        } 

        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");
        if (User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null && userEmailClaim != null)
        {
            try
            {
                var author = await _service.GetAuthorByEmail(userEmailClaim.Value);
                var cheep = new CheepCreateDTO(author.Name, CheepMessageTimeLine);
                await _service.Create(cheep);
                return Redirect(User.Identity.Name);
            }
            catch
            {
                if(userEmailClaim != null ) {
                await _service.AddAuthor(User.Identity.Name, userEmailClaim.Value);
                await _service.Create(new CheepCreateDTO(User.Identity.Name, CheepMessageTimeLine));
                return Redirect(User.Identity.Name);
                }
            }
        }
        return Redirect("/");
    }

    public async Task<bool> DoesFollow(string CheepAuthorName) 
    {   //The Author who inquires
        if (User.Identity?.IsAuthenticated == true && User.Identity?.Name != null) {
            AuthorDTO? author = await _service.GetAuthorByName(User.Identity.Name);
            if (author != null && author.Followed != null) {
                foreach (AuthorDTO followee in author.Followed) 
                {
                    if (followee.Name == CheepAuthorName)  
                        return true;
                }
            }
        }
        return false;
    }
}