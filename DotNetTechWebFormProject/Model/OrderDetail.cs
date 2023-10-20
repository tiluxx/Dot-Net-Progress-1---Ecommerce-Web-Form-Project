using Microsoft.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace DotNetTechWebFormProject.Model
{
    public class OrderDetail
    {
        public string? OrderDetailID { get; set; }
        public string? OrderID { get; set; }
        public string? ItemId { get; set; }
        public int? Quantity { get; set; }
        public string? UnitAmount { get; set; }
        public string? ItemName { get; set; }
        public decimal? ItemPrice { get; set; }

        private string GetOrderDetailDesc(string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = "select top 1 ID from OrderDetail order by ID desc";
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);

            SqlDataReader dr = cmd.ExecuteReader();
            string res = "";
            if (dr != null)
            {
                while (dr.Read())
                {
                    res = dr["ID"].ToString();
                }
            }

            conn.Close();
            return res;
        }

        private string GetNewOrderDetailID(string connectionString)
        {
            string res = GetOrderDetailDesc(connectionString);
            if (res != null && !res.Equals(""))
            {
                int order = int.Parse(res.Substring(4)) + 1;
                if (order < 10)
                {
                    res = "ODT000000" + order.ToString();
                }
                else if (order < 100)
                {
                    res = "ODT00000" + order.ToString();
                }
                else if (order < 1000)
                {
                    res = "ODT0000" + order.ToString();
                }
                else if (order < 10000)
                {
                    res = "ODT000" + order.ToString();
                }
                else if (order < 100000)
                {
                    res = "ODT00" + order.ToString();
                }
                else if (order < 1000000)
                {
                    res = "ODT0" + order.ToString();
                }
                else
                {
                    res = "ODT" + order.ToString();
                }
                return res;
            }
            else
            {
                return "ODT0000001";
            }
        }

        public List<OrderDetail> GetOrderParametersById(string connectionString, string orderId)
        {
            List<OrderDetail> orderDetailParameterList = new List<OrderDetail>();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = $"select O.*, I.ItemName, I.Price from OrderDetail O, Item I where O.ItemID = I.ItemID and O.OrderID = '{orderId}'";

            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderDetailID = dr["ID"].ToString();
                        orderDetail.OrderID = dr["OrderID"].ToString();
                        orderDetail.ItemId = dr["ItemID"].ToString();
                        orderDetail.Quantity = Convert.ToInt32(dr["Quantity"]);
                        orderDetail.UnitAmount = dr["UnitAmount"].ToString();
                        orderDetail.ItemName = dr["ItemName"].ToString();
                        orderDetail.ItemPrice = Convert.ToDecimal(dr["Price"]);

                        orderDetailParameterList.Add(orderDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            conn.Close();
            return orderDetailParameterList;
        }

        public ResponseStatus AddOrderDetail(string connectionString, string orderId, string itemId, int quantity, string unitAmount)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            try
            {
                string newOrderDetailId = GetNewOrderDetailID(connectionString);
                string sqlQuery = $"insert into OrderDetail values ('{newOrderDetailId}', '{orderId}', '{itemId}', {quantity}, '{unitAmount}')";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(false, ex.Message);
            }
            conn.Close();

            return new ResponseStatus(true, "Add new order detail successfully");
        }
    }
}
