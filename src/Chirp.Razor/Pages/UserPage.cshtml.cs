using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;

/*
<Summary>
This is the User razor Page's page model
The user page is where the user can see their own information, so this is used for the forget me feature.
</Summary>
*/

namespace Chirp.Razor.Pages;

[Authorize]

public class UserPage : PageModel
{

    public readonly ICheepService _service;
    public UserPage(ICheepService service)
    {
        _service = service;
    }
    
    public List<CheepDTO>? Cheeps { get; set; }
    
    [FromQuery(Name = "page")]
    public int? pageNum { get; set; }

    public List<AuthorDTO>? Following { get; set; }
    
    public async Task<ActionResult> OnGet()
    {
        var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");
        if(userEmailClaim != null && await _service.DoesAuthorExist(userEmailClaim.Value))
        {
            var author = await _service.GetAuthorByEmail(userEmailClaim.Value);
            Following = author.Followed;
        }
        
        return Page();
    }

    public async Task<IActionResult> OnPostForgetMeAsync()
    {      
        if (User != null && User?.Identity?.Name != null) 
        {
            var author = await _service.GetAuthorByName(User.Identity.Name);

            //Calls to deleteCheepsFromAuthor for the specific author
            await _service.DeleteCheepsFromAuthor(author.AuthorId);

            //Deletes the author
            await _service.DeleteAuthor(author.AuthorId);
                
            //Logs the user out
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Redirect("/");
        }
        
        return Redirect("/");
    }
}