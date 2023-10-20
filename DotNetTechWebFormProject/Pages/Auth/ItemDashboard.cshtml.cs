using DotNetTechWebFormProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Metrics;

namespace DotNetTechWebFormProject.Pages
{
    public class ItemDashboardModel : PageModel
    {
        readonly IConfiguration _configuration;
        public List<Item> itemParameterList = new List<Item>();
        public string connectionString;

        public ItemDashboardModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            Item item = new Item();
            connectionString = _configuration.GetConnectionString("ConnectionString");
            itemParameterList = item.GetItemParameters(connectionString);
        }

        public IActionResult OnPost(string itemId)
        {
            Item item = new Item();
            connectionString = _configuration.GetConnectionString("ConnectionString");
            ResponseStatus res = item.DeleteItem(connectionString, itemId);
            if (res.Status)
            {
                return Redirect("/Auth/ItemDashboard");
            }
            else
            {
                return Redirect("/Auth/ItemDashboard");
            }
        }
    }
}
