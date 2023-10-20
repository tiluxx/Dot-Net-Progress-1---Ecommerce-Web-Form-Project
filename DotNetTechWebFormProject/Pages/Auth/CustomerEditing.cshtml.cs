using DotNetTechWebFormProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace DotNetTechWebFormProject.Pages
{
    public class CustomerEditingModel : PageModel
    {
        readonly IConfiguration _configuration;
        private string _customerId;
        private Customer _curCustomer = new Customer();
        public string connectionString;

        public CustomerEditingModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetCustomerId()
        {
            return _customerId;
        }

        public Customer GetCurCustomer()
        {
            return _curCustomer;
        }

        public void OnGetCustomerId(string customerid)
        {
            _customerId = customerid;
            OnGet();
        }

        public void OnGet()
        {
            Customer customer = new Customer();
            connectionString = _configuration.GetConnectionString("ConnectionString");
            _curCustomer = customer.GetCustomer(connectionString, _customerId);
        }

        public IActionResult OnPost(string customerId, string customerName, string address)
        {
            Customer customer = new Customer();
            connectionString = _configuration.GetConnectionString("ConnectionString");
            ResponseStatus res = customer.UpdateCustomer(
                connectionString,
                customerId,
                customerName,
                address);
            if (res.Status)
            {
                return Redirect("/Auth/CustomerManagement");
            }
            else
            {
                return Redirect("/Auth/CustomerManagement");
            }
        }
    }
}
