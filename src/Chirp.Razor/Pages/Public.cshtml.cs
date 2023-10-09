﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CheepRecord;
using System.Collections.Specialized;
using Repository;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    public int Count { get; set; }
    // private readonly ICheepService _service;
    public List<CheepViewModel>? Cheeps { get; set; }

    // public PublicModel(ICheepService service)
    // {
    //     _service = service;
    // }

    [FromQuery(Name = "page")]
    public int? pageNum { get; set; }
    public ActionResult OnGet()
    {  
        if (pageNum.HasValue){
            Cheeps = CheepRepository.GetCheeps(pageNum);
        } else {
            Cheeps = CheepRepository.GetCheeps(pageNum);
        }

        return Page();
    } 
}
