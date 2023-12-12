using FluentValidation;

namespace Chirp.Core;

/*
<Summary>
This is the CheepCreateDTO
This is the DTO that we send from the application back to the database, for us to create a new Cheep.
</Summary>
*/
public record CheepCreateDTO(string Author, string Text);


/*
<Summary>
This is the Cheep Validator
We use this to validate the CheepCreateDTO, to make sure that the Author and Text is not empty, 
and that the text is not longer than 160 characters.
</Summary>
*/
public class CheepCreateValidator : AbstractValidator<CheepCreateDTO>
{
    public CheepCreateValidator()
    {   
        RuleFor(c => c.Author).NotEmpty().MaximumLength(50);
        RuleFor(c => c.Text).NotEmpty().MaximumLength(160);
    }
}