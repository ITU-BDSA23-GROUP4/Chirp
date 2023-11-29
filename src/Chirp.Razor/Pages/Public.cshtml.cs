﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

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

    [FromQuery(Name = "{page}")]
    public int? pageNum { get; set; }
    
    [FromQuery(Name ="follow")]
    public int? follow{ get; set; }

    [FromQuery(Name ="unfollow")]
    public int? unfollow{ get; set; }
    
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

        var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");
        if(User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null && userEmailClaim != null)
        {
            try{
                _service.AddAuthor(User.Identity.Name, userEmailClaim.Value);
            } catch (Exception) {
                //Do nothing as the author already exists
            }
        }
        
        if (User.Identity?.IsAuthenticated == true  && User.Identity.Name != null) {
            AuthorDTO currentUser = _service.GetAuthorByName(User.Identity.Name);
            if (follow.HasValue && follow != null) 
            {
                _service.AddFollowee(currentUser.AuthorId, (int)follow);
            } 
            else if (unfollow.HasValue && unfollow != null) 
            {
                _service.RemoveFollowee(currentUser.AuthorId, (int)unfollow);
            }
        }

        return Page();
    }
    public IActionResult OnPost()
    {
        var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == "emails");
        if(User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null && userEmailClaim != null)
        {
            try
            {
                var author = _service.GetAuthorByEmail(userEmailClaim.Value);
                if (author != null)
                {
                    var cheep = new CheepCreateDTO(author.Name, CheepMessageTimeLine);
                    _service.Create(cheep);
                    return Redirect(User.Identity.Name);
                }
            }
            catch (Exception)
            {
                _service.AddAuthor(User.Identity.Name, userEmailClaim.Value);
                _service.Create(new CheepCreateDTO(User.Identity.Name, CheepMessageTimeLine));
                return Redirect(User.Identity.Name);
            }
        }
        return Redirect("/");
    }

    public bool DoesFollow(int AuthorId) 
    {
        AuthorDTO? author = null;
        // Needs to be refactored into the get method so we does not call it 32 times per page load
        if (User?.Identity?.IsAuthenticated == true && User?.Identity?.Name != null) {
            
            if (User.Identity.Name != null) {
                author = _service.GetAuthorByName(User.Identity.Name);
            }
            
            if (author != null && author.Followed != null) {
                foreach (AuthorDTO followingAuthor in author.Followed) {
                    if (followingAuthor.AuthorId == AuthorId) {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
