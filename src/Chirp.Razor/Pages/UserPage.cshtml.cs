using Chirp.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages
{
    [Authorize]
    public class UserPage : PageModel
    {
        public IActionResult OnPostForgetMe()
        {
            var authorIdClaim = User.FindFirst("authorId");
            if (authorIdClaim == null)
            {
                Console.WriteLine("AuthorId claim not found");
            }
            
            try
            {
                //Calls to deleteCheepsFromAuthor for the specific author
                var authorId = int.Parse(authorIdClaim.Value);
                var cheepRepo = new CheepRepository();
                cheepRepo.deleteCheepsFromAuthor(authorId);
                Console.WriteLine("Cheeps deleted");

                //Deletes all following relationships for the specific author and who they are following
                var authorRepo = new AuthorRepository();
                authorRepo.deleteAuthorsFollowing(authorId);
                authorRepo.deleteAuthorsFollowers(authorId);

                //Deletes the author
                authorRepo.deleteAuthor(authorId);
                Console.WriteLine("Author deleted");

                //Logs the user out
                HttpContext.SignOutAsync();
                return Redirect("/");
            }catch (Exception)
            {
                return Redirect("/Profilepage");
            }
        }
    }
}