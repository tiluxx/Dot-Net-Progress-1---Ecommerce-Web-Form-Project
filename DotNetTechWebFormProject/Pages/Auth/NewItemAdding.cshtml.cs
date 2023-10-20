using DotNetTechWebFormProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace DotNetTechWebFormProject.Pages
{
    public class NewItemAddingModel : PageModel
    {
        readonly IConfiguration _configuration;
        public List<Item> itemParameterList = new List<Item>();
        public string connectionString;

        public NewItemAddingModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(
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
            ResponseStatus res = item.AddItem(
                connectionString, 
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
            } else
            {
                return Redirect("/Auth/ItemDashboard");
            }
        }
    }
}
