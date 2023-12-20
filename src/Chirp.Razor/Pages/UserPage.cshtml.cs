using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;

namespace Chirp.Razor.Pages;

/*
<Summary>
This is the model for the UserPage page.

The user page is where the user can see their own information.
It also include the forget me feature.
</Summary>
*/

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

            await _service.DeleteCheepsFromAuthor(author.AuthorId);
            await _service.DeleteAuthor(author.AuthorId);
        
            // Logs the user out and deletes the cookie stored in the browser
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Redirect("/");
        }
        return Redirect("/");
    }
}