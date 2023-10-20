using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetTechWebFormProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string message = "";


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                message = "Please enter username and password";
                return Page();
            }

            if (username != "admin" || password != "123456")
            {
                message = "Invalid username or password";
                return Page();
            }

            HttpContext.Session.SetString("username", username);
            return Redirect("/Auth/Index");
        }
    }
}