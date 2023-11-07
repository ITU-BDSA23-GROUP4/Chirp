using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore;
using System.Security.Claims;

namespace Chirp.Razor.Pages;

public class ClaimsModel : PageModel
{
    // Code is copied from Microsoft documentation: https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://bdsagroup4chirprazor.azurewebsites.net/"),
    };

    public async Task<string> GetIdentityAsync() {
            
        string output = "";
        using HttpResponseMessage response = await sharedClient.GetAsync(".auth/me");
    
        output = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"{output}\n");

        return output;
    }
}