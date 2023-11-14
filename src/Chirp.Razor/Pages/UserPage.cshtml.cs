using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

[AllowAnonymous]

public class UserPage : PageModel
{
    public void OnGet()
    {
    }
}