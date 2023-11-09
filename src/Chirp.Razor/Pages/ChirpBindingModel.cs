using System.ComponentModel.DataAnnotations;

namespace Chirp.Razor.pages;
//The binding model Creates a role set for the Chirps displayed
public class ChirpBindingModel
{
    [Required, MaxLength(160), Display(Name = "Message text")]
    public string text { get; set; } = string.Empty;
}