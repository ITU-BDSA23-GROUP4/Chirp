using FluentValidation;

namespace Chirp.Core;

public record CheepCreateDTO(string Author, string Text);

public class CheepCreateValidator : AbstractValidator<CheepCreateDTO>
{
    public CheepCreateValidator()
    {   
        RuleFor(c => c.Author).NotEmpty().MaximumLength(50);
        RuleFor(c => c.Text).NotEmpty().MaximumLength(160);
    }
}