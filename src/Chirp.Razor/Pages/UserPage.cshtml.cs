using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Infrastructure;

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

        public readonly ICheepService _service;
    
        
        public List<CheepDTO>? Cheeps { get; set; }
        
        [FromQuery(Name = "page")]
        public int? pageNum { get; set; }
        public UserPage(ICheepService service)
            {
                _service = service;
            }
        
    }
}
