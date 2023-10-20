using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetTechWebFormProject.Pages.Auth
{
    public class PaymentResponseModel : PageModel
    {
        public string Message { get; set; }
        public void OnGet()
        {
            Message = Request.Query["message"];
        }
    }
}
