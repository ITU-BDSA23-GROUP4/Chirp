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
