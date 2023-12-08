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
    
    [FromQuery(Name ="follow")]
    public string? follow{ get; set; }

    [FromQuery(Name ="unfollow")]
    public string? unfollow{ get; set; }
    
    public async Task<ActionResult> OnGet()
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
            bool authorExists = (bool)await _service.DoesAuthorExist(userEmailClaim.Value);
            if(!authorExists)
            {
                try{
                    await _service.AddAuthor(User.Identity.Name, userEmailClaim.Value);
                } catch (Exception) {
                    //Do nothing as the author already exists
                }
            }
        }
        
        if (User?.Identity?.IsAuthenticated == true  && User.Identity.Name != null) {
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

    //This is the method that adds a cheep from the user, if a user isn't in the DB they will be added to the db
    public async Task<IActionResult> OnPost()
    {
        var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");
        if(User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null && userEmailClaim != null)
        {
            try
            {
                var author = await _service.GetAuthorByEmail(userEmailClaim.Value);
                if (author != null)
                {
                    var cheep = new CheepCreateDTO(author.Name, CheepMessageTimeLine);
                    await _service.Create(cheep);
                    return Redirect(User.Identity.Name);
                }
            }
            catch (Exception)
            {
                await _service.AddAuthor(User.Identity.Name, userEmailClaim.Value);
                await _service.Create(new CheepCreateDTO(User.Identity.Name, CheepMessageTimeLine));
                return Redirect(User.Identity.Name);
            }
        }
        return Redirect("/");
    }

    public async Task<bool> DoesFollow(string AuthorName) 
    {
        // AuthorDTO? author = null;
        // // Needs to be refactored into the get method so we does not call it 32 times per page load
        // if (User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null) {
        // List<AuthorDTO> followedMyAuthor = _service.GetAllFollowedAuthors(())
        //     if (User.Identity.Name != null) {
        //         author = _service.GetAuthorByName(User.Identity.Name);


        //     }
            
        //     if (author != null && author.Followed != null) {
        //         foreach (AuthorDTO followingAuthor in author.Followed) {
        //             if (followingAuthor.AuthorId == AuthorId) {
        //                 return true;
        //             }
        //         }
        //     }
        // }
        return false;
    }
}
