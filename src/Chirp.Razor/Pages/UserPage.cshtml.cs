using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

/*
<Summary>
This is the User Pages model
The user page is where the user can see their own information, so this is used for the forget me feature.
</Summary>
*/

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
                var authorID = Guid.Parse(authorIdClaim.Value);
                _service.DeleteCheepsFromAuthor(authorID);

                //Deletes all following relationships for the specific author and who they are following
                /*
                foreach (var followee in _service.GetAllFollowees(authorID))
                {
                    _service.RemoveFollowee(authorID, followee.AuthorId);
                }*/

                //Deletes the author
                _service.DeleteAuthor(authorID);

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