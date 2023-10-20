using DotNetTechWebFormProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetTechWebFormProject.Pages.Auth
{
    public class ItemFilterModel : PageModel
    {
        readonly IConfiguration _configuration;
        public List<Item> itemParameterList = new List<Item>();
        public string connectionString;

        public ItemFilterModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
        }

        public void OnPostItemFilter(string itemFilterOption, string itemId, string customerId)
        {
            Item item = new Item();
            connectionString = _configuration.GetConnectionString("ConnectionString");

            switch (itemFilterOption)
            {
                case "BestSellingItem":
                    itemParameterList = item.GetBestSellingItem(connectionString, itemId);
                    break;
                case "ItemsPurchasedByCustomers":
                    itemParameterList = item.GetPurchasedItemByItemId(connectionString, itemId);
                    break;
                case "CustomerPurchasesItems":
                    itemParameterList = item.GetPurchasedItemByCustomerId(connectionString, customerId);
                    break;
                default:
                    break;
            }
        }
    }
}
