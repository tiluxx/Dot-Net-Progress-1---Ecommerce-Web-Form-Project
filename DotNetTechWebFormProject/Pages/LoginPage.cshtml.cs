using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetTechWebFormProject.Pages
{
    public class LoginPageModel : PageModel
    {
        public string message = "";

        public void OnGet()
        {
        }

        public void OnPost(string username, string password) 
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                message = "Please enter username and password";
                return;
            }

            if (username != "admin" && password != "123456")
            {
                message = "Invalid username or password";
                return;
            }

            Redirect("/Auth/Index");
        }
    }
}
