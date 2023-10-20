using Microsoft.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace DotNetTechWebFormProject.Model
{
    public class Customer
    {
        public string? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerAddress { get; set; }

        private string GetCustomerDesc(string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = "select top 1 CustID from Customer order by CustID desc";
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);

            SqlDataReader dr = cmd.ExecuteReader();
            string res = "";
            if (dr != null)
            {
                while (dr.Read())
                {
                    res = dr["CustID"].ToString();
                }
            }

            conn.Close();
            return res;
        }

        private string GetNewCustomerID(string connectionString)
        {
            string res = GetCustomerDesc(connectionString);
            if (res != null && !res.Equals(""))
            {
                int order = int.Parse(res.Substring(4)) + 1;
                if (order < 10)
                {
                    res = "CUS000000" + order.ToString();
                }
                else if (order < 100)
                {
                    res = "CUS00000" + order.ToString();
                }
                else if (order < 1000)
                {
                    res = "CUS0000" + order.ToString();
                }
                else if (order < 10000)
                {
                    res = "CUS000" + order.ToString();
                }
                else if (order < 100000)
                {
                    res = "CUS00" + order.ToString();
                }
                else if (order < 1000000)
                {
                    res = "CUS0" + order.ToString();
                }
                else
                {
                    res = "CUS" + order.ToString();
                }
                return res;
            }
            else
            {
                return "CUS0000001";
            }
        }

        public List<Customer> GetCustomerParameters(string connectionString)
        {
            List<Customer> customerParameterList = new List<Customer>();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = "select * from Customer";

            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Customer customer = new Customer();
                        customer.CustomerID = dr["CustID"].ToString();
                        customer.CustomerName = dr["CustName"].ToString();
                        customer.CustomerAddress = dr["Address"].ToString();

                        customerParameterList.Add(customer);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            conn.Close();
            return customerParameterList;
        }

        public Customer GetCustomer(string connectionString, string customerId)
        {
            Customer customer = new Customer();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = $"select * from Customer where CustID = '{customerId}'";

            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        customer.CustomerID = dr["CustID"].ToString();
                        customer.CustomerName = dr["CustName"].ToString();
                        customer.CustomerAddress = dr["Address"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            conn.Close();
            return customer;
        }

        public ResponseStatus AddCustomer(string connectionString, string customerName, string address)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            try
            {
                string newCustomerId = GetNewCustomerID(connectionString);
                string sqlQuery = $"insert into Customer values ('{newCustomerId}', '{customerName}', '{address}')";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(false, ex.Message);
            }
            conn.Close();

            return new ResponseStatus(true, "Add new customer successfully");
        }

        public ResponseStatus UpdateCustomer(string connectionString, string customerId, string customerName, string address)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            try
            {
                string sqlQuery = $"update Customer set CustName = '{customerName}', Address = '{address}' where CustID = '{customerId}'";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(false, ex.Message);
            }
            conn.Close();

            return new ResponseStatus(true, "Update customer successfully");
        }

        public ResponseStatus DeleteCustomer(string connectionString, string customerId)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            try
            {
                string sqlQuery = $"delete from Customer where CustID = '{customerId}'";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(false, ex.Message);
            }
            conn.Close();

            return new ResponseStatus(true, "Delete customer successfully");
        }
    }
}
