using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages
{
    [Authorize]
    public class UserPage : PageModel
    {
        public UserPage(ICheepService service)
        {
            _service = service;
        }
        public ICheepService _service;

        public IActionResult OnPostForgetMe()
        {
            var authorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "authorId");
            if (authorIdClaim == null)
            {
                Console.WriteLine("AuthorId claim not found");
            }

            //Open an alert box to confirm the deletion
            //If pressed okay continue
            
            try
            {
                //Calls to deleteCheepsFromAuthor for the specific author
                var authorId = int.Parse(authorIdClaim.Value);
                _service.DeleteCheepsFromAuthor(authorId);
                Console.WriteLine("Cheeps deleted");

                //Deletes all following relationships for the specific author and who they are following
                _service.deleteAuthorsFollowing(authorId);
                _service.deleteAuthorsFollowers(authorId);

                //Deletes the author
                _service.deleteAuthor(authorId);
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