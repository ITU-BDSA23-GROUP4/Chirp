using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

/*
<Summary>
This is the User Pages model
The user page is where the user can see their own information, so this is used for the forget me feature.
</Summary>
*/

namespace Chirp.Razor.Pages
{
    [Authorize]
    public class UserPage : PageModel
    {
       
    }
}