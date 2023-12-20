using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Microsoft.AspNetCore.Authorization;
namespace Chirp.Razor.Pages;

/*
<Summary>
This is the model for the UsersTimeline page.

This is where the user can see their own timeline of cheeps and those they follow.
</Summary>
*/

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

    [BindProperty]
    public string CheepMessageTimeLine { get; set; } = "";
    public List<CheepDTO>? Cheeps { get; set; }

    [FromQuery(Name = "page")]
    public int? pageNum { get; set; }

    [FromQuery(Name ="follow")]
    public string? follow{ get; set; }

    [FromQuery(Name ="unfollow")]
    public string? unfollow{ get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? Author { get; set; }

    public async Task<ActionResult> OnGet(string author)
    {
        if (pageNum.HasValue)
        {   
            if(User.Identity?.Name==author)
            {
                Cheeps = _service.CombineCheepsAndFollowerCheeps(author ,pageNum);
                
            } else {
                Cheeps = _service.CombineCheepsAndFollowerCheeps(author, pageNum);
            }
        }
        // the else statement with the same code ensures page 0 and page 1 shows the same cheeps
        else
       {   
            if(User.Identity?.Name==author)
            {
                Cheeps =  _service.CombineCheepsAndFollowerCheeps(author ,pageNum);
                
            } else {
                Cheeps = _service.CombineCheepsAndFollowerCheeps(author, pageNum);
            }
        }

        if (User.Identity?.IsAuthenticated == true  && User.Identity.Name != null) {
            if (follow != null) 
            {
                await _service.AddFollowee(User.Identity.Name, follow);
            } 
            else if (unfollow != null) 
            {
                await _service.RemoveFollowee(User.Identity.Name, unfollow);
                return Redirect(User.Identity.Name);
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
                // Theese lines executes if the user already exists in the database as an author
                var cheep = new CheepCreateDTO(author.Name, CheepMessageTimeLine);
                await _service.Create(cheep);
                return Redirect(User.Identity.Name);
            }
            catch
            {   
                // Theese lines are executed if the user is not yet added to the database as an author
                if(userEmailClaim != null ) 
                {
                    await _service.AddAuthor(User.Identity.Name, userEmailClaim.Value);
                    await _service.Create(new CheepCreateDTO(User.Identity.Name, CheepMessageTimeLine));
                    return Redirect(User.Identity.Name);
                }
            }
        }
        return Redirect("/");
    }

    public async Task<bool> DoesFollow(string CheepAuthorName) 
    {
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
    public async Task<IActionResult> OnPostLike(Guid cheepId)
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
            await _service.IncreaseLikeAttributeInCheep(cheepId);
        return Redirect("/");
    }
}