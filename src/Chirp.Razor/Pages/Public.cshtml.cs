using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;

namespace Chirp.Razor.Pages;

/*
<Summary>
This is the model for the Public page.

This is where we react to all of the users behaviour. 
That is, getting all the cheeps for the relevant page and showing the cheeps with the correct information.
</Summary>
*/

public class PublicModel : PageModel
{
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
    
    [FromQuery(Name ="follow")]
    public string? follow { get; set; }

    [FromQuery(Name ="unfollow")]
    public string? unfollow { get; set; }

    public async Task<ActionResult> OnGet()
    {
        if (pageNum.HasValue)
        {
            Cheeps = _service.GetCheeps(pageNum);
        }
        // the else statement with the same code ensures page 0 and page 1 shows the same cheeps
        else
        {
            Cheeps = _service.GetCheeps(pageNum);
        }

        var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");
        if(User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null && userEmailClaim != null)
        { 
            bool authorExists = await _service.DoesAuthorExist(userEmailClaim.Value);
            if(!authorExists)
            {
                await _service.AddAuthor(User.Identity.Name, userEmailClaim.Value);
            }
        }
        
        if (User?.Identity?.IsAuthenticated == true  && User.Identity.Name != null) 
        {
            if (follow != null) 
            {
                await _service.AddFollowee(User.Identity.Name, follow);
            } 
            else if (unfollow != null) 
            {
                await _service.RemoveFollowee(User.Identity.Name, unfollow);
            }
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");
        if(User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null && userEmailClaim != null)
        {
            try
            {
                var author = await _service.GetAuthorByEmail(userEmailClaim.Value);
                // Theese lines executes if the user already exists in the database as an author
                var cheep = new CheepCreateDTO(author.Name, CheepMessageTimeLine);
                await _service.Create(cheep);
                return Redirect(User.Identity.Name);
            }
            catch (Exception)
            {
                // Theese lines are executed if the user is not yet added to the database as an author
                await _service.AddAuthor(User.Identity.Name, userEmailClaim.Value);
                await _service.Create(new CheepCreateDTO(User.Identity.Name, CheepMessageTimeLine));
                return Redirect(User.Identity.Name);
            }
        }
        return Redirect("/");
    }

    public async Task<bool> DoesFollow(string CheepAuthorName) 
    {
        if(User.Identity?.IsAuthenticated == true && User.Identity?.Name != null )
        {
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
