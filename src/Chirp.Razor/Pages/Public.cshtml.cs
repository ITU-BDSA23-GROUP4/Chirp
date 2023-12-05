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

    [FromQuery(Name = "{page}")]
    public int? pageNum { get; set; }
    
   /*  [FromQuery(Name ="follow")]
    public int? follow{ get; set; }

    [FromQuery(Name ="unfollow")]
    public int? unfollow{ get; set; } */
    LogFile logger = new LogFile("mylog.txt");
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

        /* var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");
        if(User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null && userEmailClaim != null)
        { 
            try{
                _service.AddAuthor(User.Identity.Name, userEmailClaim.Value);
            } catch (Exception) {
                //Do nothing as the author already exists
            }
        } */
        /*
        if (User?.Identity?.IsAuthenticated == true  && User.Identity.Name != null) {
            AuthorDTO currentUser = _service.GetAuthorByName(User.Identity.Name);
            if (follow.HasValue && follow != null) 
            {
                _service.AddFollowee(currentUser.AuthorId, (int)follow);
            } 
            else if (unfollow.HasValue && unfollow != null) 
            {
                _service.RemoveFollowee(currentUser.AuthorId, (int)unfollow);
            }
        } */

        return Page();
    }

    //This is the method that adds a cheep from the user, if a user isn't in the DB they will be added to the db
    public async Task<IActionResult> OnPost()
    {
        logger.Log("User clicked share on the cheep button");
        var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");
        if(User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null && userEmailClaim != null)
        {
            logger.Log("The user is authenticated");
            try
            {
                logger.Log("Trying to add a cheep");
                var author = await _service.GetAuthorByEmail(userEmailClaim.Value);
                logger.Log("_service.GetAuthorByEmail has completed, author is: " + author.Name);
                if (author != null)
                {
                    logger.Log("Author is not null");
                    var cheep = new CheepCreateDTO(author.Name, CheepMessageTimeLine);
                    logger.Log("ChepDTO created with message: " + CheepMessageTimeLine + " and author: " + author.Name);
                    await _service.Create(cheep);
                    logger.Log("_service.Create has completed. Will now redirect to userpage");
                    return Redirect(User.Identity.Name);
                }
            }
            catch (Exception e)
            {
                logger.Log("Exception" + e + " exception message" + e.Message);
                logger.Log("Something went wrong we are now trying to add the author to the DB");
                await _service.AddAuthor(User.Identity.Name, userEmailClaim.Value);
                logger.Log("We added the author");
                await _service.Create(new CheepCreateDTO(User.Identity.Name, CheepMessageTimeLine));
                logger.Log("We added the cheep and are now redirecting");
                return Redirect(User.Identity.Name);
            }
        }
        return Redirect("/");
    }

   /*  public bool DoesFollow(int AuthorId) 
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
    } */
}
