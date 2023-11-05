using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.pages;

public class NewCheepModel : PageModel
{
    CheepRepository _repository = new CheepRepository();

    [BindProperty]
    public ChirpBindingModel? chirpBinding {get; set;}

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var newCheep = new NewCheep();
            if (newCheep != null && newCheep.Message != null && User.Identity != null && User.Identity.Name != null)
            {
                var cheep = new CheepCreateDTO(newCheep.Message, User.Identity.Name);
                await _repository.Create(cheep);
                return RedirectToPage("/UserPage");
            }
        }
        return Page();
    }
}

public class NewCheep
{
    [Required, StringLength(160)]
    public string Message { get; set; } = string.Empty; //= string.Empty; fixes null error
}