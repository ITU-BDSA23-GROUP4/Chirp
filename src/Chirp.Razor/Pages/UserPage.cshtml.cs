using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Chirp.Infrastructure;

namespace Chirp.Razor.Pages
{
    [Authorize]
    public class UserPage : PageModel
    {
        public PublicModel(ICheepService service)
        {
            _service = service;
        }
       
    }
}