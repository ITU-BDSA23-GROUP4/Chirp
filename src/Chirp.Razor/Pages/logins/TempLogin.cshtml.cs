using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages
{
    [Authorize]
    public class TempLogin : PageModel
    {
        public IActionResult OnGet()
        {
            // Check if the user is authenticated
            if (User.Identity?.IsAuthenticated == true && User.Identity?.Name != null)
            {
                // If authenticated, redirect to the front page
                return Redirect("/");
            }

            // If not authenticated, continue with the normal OnGet behavior
            return Redirect("/");
        }
    }
}