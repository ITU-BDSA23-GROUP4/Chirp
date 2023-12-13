using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

/*
<Summary>
This is the User razor Page's page model
The user page is where the user can see their own information, so this is used for the forget me feature.
</Summary>
*/

namespace Chirp.Razor.Pages
{
    [Authorize]
    public class UserPage : PageModel
    {
        public UserPage(ICheepService service)
        {
            _service = service;
        }
        public ICheepService _service;

        public async Task<IActionResult> OnPostForgetMeAsync()
        {
            try
            {       
                if (User != null && User?.Identity?.Name != null) {
                    var author = await _service.GetAuthorByName(User.Identity.Name);
                    
                    //Calls to deleteCheepsFromAuthor for the specific author
                    await _service.DeleteCheepsFromAuthor(author.AuthorId);

                    //Deletes the author
                    await _service.DeleteAuthor(author.AuthorId);

                    //Logs the user out
                    
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
                return Redirect("/");
            }
            catch (Exception)
            {
                return Redirect("/Profilepage");
            }
        }
    }
}