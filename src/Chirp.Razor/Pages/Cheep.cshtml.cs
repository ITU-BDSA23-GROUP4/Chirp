using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class CheepModel : PageModel
    {

        [BindProperty]
        public required string Author { get; set; }

        [BindProperty]
        public required string Message { get; set; }

        public void onGet()
        {

        }
        public void OnPost()
        {
            string author = Author;
            string message = Message;

            Console.WriteLine($"Author: {author}, Message: {message}");
            
        }
    }
}
