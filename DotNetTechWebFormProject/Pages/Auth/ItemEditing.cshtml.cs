using DotNetTechWebFormProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetTechWebFormProject.Pages
{
    public class ItemEditingModel : PageModel
    {
        readonly IConfiguration _configuration;
        private string _itemId;
        private Item _curItem = new Item();
        public string connectionString;

        public ItemEditingModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetItemId()
        {
            return _itemId;
        }

        public Item GetCurItem()
        {
            return _curItem;
        }

        public void OnGetItemId(string itemid)
        {
            _itemId = itemid;
            OnGet();
        }

        public void OnGet()
        {
            Item item = new Item();
            connectionString = _configuration.GetConnectionString("ConnectionString");
            _curItem = item.GetItem(connectionString, _itemId);
        }

        public IActionResult OnPost(
            string itemId,
            string itemName, 
            string itemSize, 
            string itemUnitSize, 
            string itemBrand,
            string itemOrigin,
            int itemQuantity,
            decimal itemPrice
            )
        {
            Item item = new Item();
            connectionString = _configuration.GetConnectionString("ConnectionString");
            ResponseStatus res = item.UpdateItem(
                connectionString,
                itemId,
                itemName,
                itemSize,
                itemUnitSize,
                itemBrand,
                itemOrigin,
                itemQuantity,
                itemPrice);
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
