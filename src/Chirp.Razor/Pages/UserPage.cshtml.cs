using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Chirp.Razor.Pages;

[Authorize]

public class UserPage : PageModel
{
    public void OnGet()
    {
    }
}