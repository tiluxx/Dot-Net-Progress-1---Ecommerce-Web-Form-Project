using DotNetTechWebFormProject.Model;
using DotNetTechWebFormProject.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetTechWebFormProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private ResponseStatus res;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public ResponseStatus GetResponseStatus() 
        {
            return res;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                res = new ResponseStatus(false, "Please enter username and password");
                return Page();
            }

            if (username != "admin" || password != "123456")
            {
                res = new ResponseStatus(false, "Invalid username or password");
                return Page();
            }

            EmailSender emailSender = new EmailSender();
            string token = "3242fEFWEWEFW43FGGWwe";
            string returnUrl = $"http://localhost:5073/LoginPage?username={username}&token={token}";
            string link = $"<a href='{returnUrl}'>this link</a>";
            string htmlMessage = $"We noticed a new sign-in to your Admin Account on a strange device. If this was you, please click {link} to confirm.";
            await emailSender.SendEmailAsync("work.lethanhtien@gmail.com", "Ecommerce Security Alert", htmlMessage);

            res = new ResponseStatus(true, "A confirmation email sent to your admin email, please confirm your activity");
            return Page();
            /*HttpContext.Session.SetString("username", username);
            return Redirect("/Auth/Index");*/
        }
    }
}