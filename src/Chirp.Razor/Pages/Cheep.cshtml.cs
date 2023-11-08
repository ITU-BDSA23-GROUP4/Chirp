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
    private readonly CheepCreateValidator? validator; //Validator for the database
   
    [BindProperty]
    public string Author { get; set; } = "";

    [BindProperty]
    public ChirpBindingModel? chirpBinding {get; set;}

    [BindProperty, Required, StringLength(160)]
    public string CheepMessage { get; set; } = string.Empty; //= string.Empty; fixes null error
 
    public IActionResult OnPost()
    {
        try
        {
            var cheep = new CheepCreateDTO(authorRepo.GetAuthorByName(Author).Name, CheepMessage);
            
            cheepRepo.Create(cheep);

            return Redirect($"/{Author}");   
        }
        catch (Exception)
        { 
            return Redirect("/");
        }
    }
}