using Microsoft.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace DotNetTechWebFormProject.Model
{
    public class Order
    {
        public string? OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerID { get; set; }

        private string GetOrderDesc(string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = "select top 1 OrderID from _Order order by OrderID desc";
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);

            SqlDataReader dr = cmd.ExecuteReader();
            string res = "";
            if (dr != null)
            {
                while (dr.Read())
                {
                    res = dr["OrderID"].ToString();
                }
            }

            conn.Close();
            return res;
        }

        private string GetNewOrderID(string connectionString)
        {
            string res = GetOrderDesc(connectionString);
            if (res != null && !res.Equals(""))
            {
                int order = int.Parse(res.Substring(4)) + 1;
                if (order < 10)
                {
                    res = "ORD000000" + order.ToString();
                }
                else if (order < 100)
                {
                    res = "ORD00000" + order.ToString();
                }
                else if (order < 1000)
                {
                    res = "ORD0000" + order.ToString();
                }
                else if (order < 10000)
                {
                    res = "ORD000" + order.ToString();
                }
                else if (order < 100000)
                {
                    res = "ORD00" + order.ToString();
                }
                else if (order < 1000000)
                {
                    res = "ORD0" + order.ToString();
                }
                else
                {
                    res = "ORD" + order.ToString();
                }
                return res;
            }
            else
            {
                return "ORD0000001";
            }
        }

        public List<Order> GetOrderParameters(string connectionString)
        {
            List<Order> orderParameterList = new List<Order>();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = "select * from _Order";

            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Order order = new Order();
                        order.OrderID = dr["OrderID"].ToString();
                        order.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                        order.CustomerID = dr["CustID"].ToString();

                        orderParameterList.Add(order);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            conn.Close();
            return orderParameterList;
        }

        public Order GetOrder(string connectionString, string orderId)
        {
            Order order = new Order();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = $"select * from _Order where OrderID = '{orderId}'";

            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        order.OrderID = dr["OrderID"].ToString();
                        order.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                        order.CustomerID = dr["CustID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            conn.Close();
            return order;
        }

        public ResponseStatus AddOrder(string connectionString, DateTime orderDate, string customerId)
        {
            string newOrderId = GetNewOrderID(connectionString);
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            try
            {
                newOrderId = GetNewOrderID(connectionString);
                string sqlQuery = $"insert into _Order values ('{newOrderId}', '{orderDate}', '{customerId}')";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(false, ex.Message);
            }
            conn.Close();

            return new ResponseStatus(true, "Add new order successfully", newOrderId);
        }
    }
}
