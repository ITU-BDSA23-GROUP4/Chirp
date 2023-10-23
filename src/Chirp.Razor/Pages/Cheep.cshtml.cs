using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Core;

namespace Chirp.Razor.Pages;

public class CheepModel : PageModel
{

    CheepRepository cheepRepo = new CheepRepository();
    AuthorRepository authorRepo = new AuthorRepository();


    [BindProperty]
    public string Author { get; set; } = "";

    [BindProperty]
    public string CheepMessage { get; set; } = "";
 
    public IActionResult OnPost()
    {
        Console.WriteLine($"Author: {Author}, Message: {CheepMessage}");
        //Console.WriteLine($"Author ID: {authorRepo.GetAuthorByName(Author).AuthorId}");

        if(authorRepo.GetAuthorByName(Author)!=null){
            cheepRepo.AddCheep(authorRepo.GetAuthorByName(Author).AuthorId, CheepMessage);
            return Redirect($"/{Author}");
        }
        else
        {
            Console.WriteLine("Author Doesn't Exist"); 
            return Redirect("/");
        }

    }
}
    
    


