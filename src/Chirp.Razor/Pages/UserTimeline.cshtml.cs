﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CheepRecord;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author, int ? change)
    {   
        Console.WriteLine("This is the current Author: " + author + " size of list: " + _service.GetAllCheepsFromAuthor(author).Count );
        if (change.HasValue){
           _service.AlterPage(_service.GetAllCheepsFromAuthor(author).Count, change.Value );
        }
        Cheeps = _service.GetCheepsFromAuthor(author);
        return Page();
    }
      public int getPage(){
        return  _service.GetPage();
    }
}
