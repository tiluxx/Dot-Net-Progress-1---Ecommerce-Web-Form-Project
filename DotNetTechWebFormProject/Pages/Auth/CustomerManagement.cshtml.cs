using DotNetTechWebFormProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotNetTechWebFormProject.Pages
{
    public class CustomerManagementModel : PageModel
    {
        readonly IConfiguration _configuration;
        public List<Customer> customerParameterList = new List<Customer>();
        public string connectionString;

        public CustomerManagementModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            Customer customer = new Customer();
            connectionString = _configuration.GetConnectionString("ConnectionString");
            customerParameterList = customer.GetCustomerParameters(connectionString);
        }

        public IActionResult OnPostAddCustomer(string customerName, string address)
        {
            Customer customer = new Customer();
            connectionString = _configuration.GetConnectionString("ConnectionString");
            ResponseStatus res = customer.AddCustomer(connectionString, customerName, address);
            if (res.Status)
            {
                return Redirect("/Auth/CustomerManagement");
            }
            else
            {
                return Redirect("/Auth/CustomerManagement");
            }

        }

        public IActionResult OnPostDeleteCustomer(string customerId)
        {
            Customer customer = new Customer();
            connectionString = _configuration.GetConnectionString("ConnectionString");
            ResponseStatus res = customer.DeleteCustomer(connectionString, customerId);
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
