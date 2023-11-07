using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class ClaimsModel : PageModel {

    public string responseBody = "";

    public async Task<ActionResult> OnGet() {
        
        responseBody = await ClaimsHandler.ClaimsHandler.GetClaimsAsync();
        
        return Page();
    }
}