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
                Console.WriteLine("authorID claim not found");
            }
            
            try
            {
                //Calls to deleteCheepsFromAuthor for the specific author
                var authorID = int.Parse(authorIdClaim.Value);
                _service.DeleteCheepsFromAuthor(authorID);
                Console.WriteLine("Cheeps deleted");

                //Deletes all following relationships for the specific author and who they are following
                /*
                foreach (var followee in _service.GetAllFollowees(authorID))
                {
                    _service.RemoveFollowee(authorID, followee.AuthorId);
                }*/

                //Deletes the author
                _service.DeleteAuthor(authorID);
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