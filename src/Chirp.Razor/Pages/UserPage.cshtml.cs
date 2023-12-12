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
        [BindProperty(SupportsGet = true)]

        public List<CheepDTO>? Cheeps { get; set; }
        
        [FromQuery(Name = "page")]
        public int? pageNum { get; set; }
        public PublicModel(ICheepService service)
            {
                _service = service;
            }


        public async Task<ActionResult> OnGet()
        {
        if (pageNum.HasValue)
        {
            Cheeps = _service.GetCheepsFromAuthorAndFollowers(pageNum);
        }
        else
        {
            Cheeps = _service.GetCheepsFromAuthorAndFollowers(1);
        }
    }
}