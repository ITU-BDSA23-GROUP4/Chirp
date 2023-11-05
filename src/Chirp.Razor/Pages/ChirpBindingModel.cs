using System.ComponentModel.DataAnnotations;

namespace Chirp.Razor.pages;

public class ChirpBindingModel
{
    [Required, MaxLength(160), Display(Name = "Message text")]
    public string text { get; set; } = string.Empty;
}