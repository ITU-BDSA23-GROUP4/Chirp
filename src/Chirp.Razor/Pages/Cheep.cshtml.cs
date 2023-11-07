using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure;
using Chirp.Razor.pages;
using System.ComponentModel.DataAnnotations;
using Chirp.Core;

namespace Chirp.Razor.Pages;

public class CheepModel : PageModel
{
    CheepRepository cheepRepo = new CheepRepository();
    AuthorRepository authorRepo = new AuthorRepository();


    [BindProperty]
    public string Author { get; set; } = "";

    [BindProperty]
    public ChirpBindingModel? chirpBinding {get; set;}

    [BindProperty, Required, StringLength(160)]
    public string CheepMessage { get; set; } = string.Empty; //= string.Empty; fixes null error
    
 
    public async Task<IActionResult> OnPostAsync()
    {
        try
        {

            //cheepRepo.AddCheep(authorRepo.GetAuthorByName(Author).AuthorId, CheepMessage);

            var cheep = new CheepCreateDTO(CheepMessage, authorRepo.GetAuthorByName(Author).Name);
            await cheepRepo.Create(cheep);

            return Redirect($"/{Author}");   
        }
        catch (Exception)
        { 
            return Redirect("/");
        }
    }
}