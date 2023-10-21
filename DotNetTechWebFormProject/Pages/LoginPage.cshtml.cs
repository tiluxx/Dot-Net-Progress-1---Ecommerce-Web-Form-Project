using DotNetTechWebFormProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetTechWebFormProject.Pages
{
    public class LoginPageModel : PageModel
    {
        private ResponseStatus res;

        public ResponseStatus GetResponseStatus()
        {
            return res;
        }

        public IActionResult OnGet()
        {
            if (Request.Query.ContainsKey("token"))
            {
                string token = "3242fEFWEWEFW43FGGWwe";
                if (Request.Query["username"] == "admin" && Request.Query["token"] == token)
                {
                    HttpContext.Session.SetString("username", Request.Query["username"]);
                    return Redirect("/Auth/Index");
                }
            }

            res = new ResponseStatus(false, "Invalid or expired URL");
            return Page();
        }
    }
}
