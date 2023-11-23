using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Chirp.Infrastructure;
using Chirp.Core;

namespace Chirp.Razor.Pages;


public class PublicModel : PageModel
{

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    public int Count { get; set; }
    public readonly ICheepService _service;
    
    public PublicModel(ICheepService service)
    {
        _service = service;
    }
    // private readonly ICheepService _service;
    [BindProperty]
    public string CheepMessageTimeLine { get; set; } = "";
    public List<CheepDTO>? Cheeps { get; set; }

    

    [FromQuery(Name = "page")]
    public int? pageNum { get; set; }
    public ActionResult OnGet()
    {
        if (pageNum.HasValue)
        {
            Cheeps = _service.GetCheeps(pageNum);
        }
        else
        {
            Cheeps = _service.GetCheeps(pageNum);
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        if(User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null)
        {
            var userName = User.Identity.Name;
            var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");

<<<<<<< HEAD
        try{
            _service.GetAuthorByName(User.Identity.Name);
        }
        catch (Exception){
            _service.AddAuthor(User.Identity.Name, User.Claims.FirstOrDefault(c => c.Type == "emails").Value);
        }
        
        try
        {
            var cheep = new CheepCreateDTO(_service.GetAuthorByName(User.Identity.Name).Name, CheepMessageTimeLine);
            _service.Create(cheep);
            return Redirect(User.Identity.Name);
        }
        catch (Exception)
        {
            return Redirect("/");
=======
            if (userName != null && userEmailClaim != null)
            {
                try
                {
                    var author = authorRepo.GetAuthorByName(userName);
                    if (author != null)
                    {
                        var cheep = new CheepCreateDTO(author.Name, CheepMessageTimeLine);
                        cheepRepo.Create(cheep);
                        return Redirect(userName);
                    }
                }
                catch (Exception)
                {
                    authorRepo.AddAuthor(userName, userEmailClaim.Value);
                    cheepRepo.Create(new CheepCreateDTO(userName, CheepMessageTimeLine));
                    return Redirect(userName);
                }
            }
>>>>>>> main
        }
        return Redirect("/");
    }
}
