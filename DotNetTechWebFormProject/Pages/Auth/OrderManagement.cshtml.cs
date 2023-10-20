using DotNetTechWebFormProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetTechWebFormProject.Pages
{
    public class OrderManagementModel : PageModel
    {
        readonly IConfiguration _configuration;
        public List<Customer> customerParameterList = new List<Customer>();
        public List<Item> itemParameterList = new List<Item>();
        public List<Order> orderParameterList = new List<Order>();
        public string connectionString;

        public OrderManagementModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            connectionString = _configuration.GetConnectionString("ConnectionString");
            Customer customer = new Customer();
            customerParameterList = customer.GetCustomerParameters(connectionString);
            Item item = new Item();
            itemParameterList = item.GetItemParameters(connectionString);
            Order order = new Order();
            orderParameterList = order.GetOrderParameters(connectionString);
        }

        public IActionResult OnPost(string chooseCustomer, List<string> chooseProduct)
        {
            Order order = new Order();
            ResponseStatus res;

            connectionString = _configuration.GetConnectionString("ConnectionString");
            res = order.AddOrder(connectionString, DateTime.Now, chooseCustomer);
            if (!res.Status)
            {
                return Redirect("/Auth/OrderManagement");
            }

            string newOrderId = res.Data;
            foreach (string itemId in chooseProduct)
            {
                OrderDetail orderDetail = new OrderDetail();
                res = orderDetail.AddOrderDetail(connectionString, newOrderId, itemId, 10, "phone");
            }

            if (res.Status)
            {
                return Redirect("/Auth/OrderManagement");
            }
            else
            {
                return Redirect("/Auth/OrderManagement");
            }
        }
    }
}
