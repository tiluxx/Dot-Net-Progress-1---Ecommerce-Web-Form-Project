using Microsoft.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace DotNetTechWebFormProject.Model
{
    public class Item
    {
        public string? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemSize { get; set; }
        public string? ItemUnitSize { get; set; }
        public string? ItemBrand { get; set; }
        public string? ItemOrigin { get; set; }
        public int? ItemQuantity { get; set; }
        public decimal ItemPrice { get; set; }
        public string? CustomerName { get; set; }

        private string GetItemDesc(string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = "select top 1 ItemID from Item order by ItemID desc";
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);

            SqlDataReader dr = cmd.ExecuteReader();
            string res = "";
            if (dr != null)
            {
                while (dr.Read())
                {
                    res = dr["ItemID"].ToString();
                }
            }
            
            conn.Close();
            return res;
        }

        private string GetNewItemID(string connectionString)
        {
            string res = GetItemDesc(connectionString);
            if (res != null && !res.Equals(""))
            {
                int order = int.Parse(res.Substring(4)) + 1;
                if (order < 10)
                {
                    res = "ITM000000" + order.ToString();
                }
                else if (order < 100)
                {
                    res = "ITM00000" + order.ToString();
                }
                else if (order < 1000)
                {
                    res = "ITM0000" + order.ToString();
                }
                else if (order < 10000)
                {
                    res = "ITM000" + order.ToString();
                }
                else if (order < 100000)
                {
                    res = "ITM00" + order.ToString();
                }
                else if (order < 1000000)
                {
                    res = "ITM0" + order.ToString();
                }
                else
                {
                    res = "ITM" + order.ToString();
                }
                return res;
            }
            else
            {
                return "ITM0000001";
            }
        }

        public List<Item> GetItemParameters(string connectionString)
        {
            List<Item> itemParameterList = new List<Item>();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = "select * from Item";

            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Item item = new Item();
                        item.ItemId = dr["ItemID"].ToString();
                        item.ItemName = dr["ItemName"].ToString();
                        item.ItemSize = dr["Size"].ToString();
                        item.ItemUnitSize = dr["UnitSize"].ToString();
                        item.ItemBrand = dr["Brand"].ToString();
                        item.ItemOrigin = dr["Origin"].ToString();
                        item.ItemQuantity = Convert.ToInt32(dr["Quantity"]);
                        item.ItemPrice = Convert.ToDecimal(dr["Price"]);
                        item.CustomerName = "";

                        itemParameterList.Add(item);
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            conn.Close();
            return itemParameterList;
        }

        public Item GetItem(string connectionString, string itemId)
        {
            Item item = new Item();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = $"select * from Item where ItemID = '{itemId}'";

            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        item.ItemId = dr["ItemID"].ToString();
                        item.ItemName = dr["ItemName"].ToString();
                        item.ItemSize = dr["Size"].ToString();
                        item.ItemUnitSize = dr["UnitSize"].ToString();
                        item.ItemBrand = dr["Brand"].ToString();
                        item.ItemOrigin = dr["Origin"].ToString();
                        item.ItemQuantity = Convert.ToInt32(dr["Quantity"]);
                        item.ItemPrice = Convert.ToDecimal(dr["Price"]);
                        item.ItemId = dr["ItemID"].ToString();
                        item.CustomerName = "";
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            conn.Close();
            return item;
        }

        public ResponseStatus AddItem(
            string connectionString,
            string itemName,
            string itemSize,
            string itemUnitSize,
            string itemBrand,
            string itemOrigin,
            int itemQuantity,
            decimal itemPrice)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            try
            {
                string newItemId = GetNewItemID(connectionString);
                string sqlQuery = $"insert into Item values ('{newItemId}', '{itemName}', '{itemSize}', '{itemUnitSize}', '{itemBrand}', '{itemOrigin}', {itemQuantity}, {itemPrice})";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.ExecuteNonQuery();
            } catch (Exception ex)
            {
                return new ResponseStatus(false, ex.Message);
            }
            conn.Close();

            return new ResponseStatus(true, "Add new item successfully");
        }

        public ResponseStatus UpdateItem(
            string connectionString,
            string itemId,
            string itemName,
            string itemSize,
            string itemUnitSize,
            string itemBrand,
            string itemOrigin,
            int itemQuantity,
            decimal itemPrice)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            try
            {
                string sqlQuery = $"update Item set ItemName = '{itemName}', Size = '{itemSize}', UnitSize = '{itemUnitSize}', Brand = '{itemBrand}', Origin = '{itemOrigin}', Quantity = {itemQuantity}, Price = {itemPrice} where ItemID = '{itemId}'";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(false, ex.Message);
            }
            conn.Close();

            return new ResponseStatus(true, "Update item successfully");
        }

        public ResponseStatus DeleteItem(string connectionString, string itemId)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            try
            {
                string sqlQuery = $"delete from Item where ItemID = '{itemId}'";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(false, ex.Message);
            }
            conn.Close();

            return new ResponseStatus(true, "Delete item successfully");
        }

        public List<Item> GetBestSellingItem(string connectionString, string itemId)
        {
            List<Item> bestSellingItem = new List<Item>();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = "select I1.ItemID as ID, I1.ItemName as 'Product name', I1.Size, I1.UnitSize as 'Unit size', I1.Brand, I1.Origin, I1.Price, sum(O1.Quantity) as 'Selling amount'" +
                        " from Item I1, OrderDetail O1" +
                        " where I1.ItemID = O1.ItemID" +
                        " group by I1.ItemID, I1.ItemID, I1.ItemName, I1.Size, I1.UnitSize, I1.Brand, I1.Origin, I1.Price" +
                        " having sum(O1.Quantity) >= all(" +
                            " select sum(O2.Quantity)" +
                            " from Item I2, OrderDetail O2" +
                            " where I2.ItemID = O2.ItemID" +
                            " group by I2.ItemID)";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Item item = new Item();
                        item.ItemId = dr["ID"].ToString();
                        item.ItemName = dr["Product name"].ToString();
                        item.ItemSize = dr["Size"].ToString();
                        item.ItemUnitSize = dr["Unit size"].ToString();
                        item.ItemBrand = dr["Brand"].ToString();
                        item.ItemOrigin = dr["Origin"].ToString();
                        item.ItemQuantity = Convert.ToInt32(dr["Selling amount"]);
                        item.ItemPrice = Convert.ToDecimal(dr["Price"]);
                        item.CustomerName = "";

                        bestSellingItem.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            conn.Close();
            return bestSellingItem;
        }

        public List<Item> GetPurchasedItemByItemId(string connectionString, string itemId)
        {
            List<Item> itemList = new List<Item>();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = "select distinct I.*, A.CustName" +
                        " from Item I, OrderDetail OD, _Order O, Customer A" +
                        $" where O.OrderID = OD.OrderID and I.ItemID = OD.ItemID and A.CustID = O.CustID and I.ItemID = '{itemId}'";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Item item = new Item();
                        item.ItemId = dr["ItemID"].ToString();
                        item.ItemName = dr["ItemName"].ToString();
                        item.ItemSize = dr["Size"].ToString();
                        item.ItemUnitSize = dr["UnitSize"].ToString();
                        item.ItemBrand = dr["Brand"].ToString();
                        item.ItemOrigin = dr["Origin"].ToString();
                        item.ItemQuantity = Convert.ToInt32(dr["Quantity"]);
                        item.ItemPrice = Convert.ToDecimal(dr["Price"]);
                        item.CustomerName = dr["CustName"].ToString();
                        itemList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            conn.Close();
            return itemList;
        }

        public List<Item> GetPurchasedItemByCustomerId(string connectionString, string customerId)
        {
            List<Item> itemList = new List<Item>();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string sqlQuery = "select distinct I.*, A.CustName" +
                        " from Item I, OrderDetail OD, _Order O, Customer A" +
                        $" where O.OrderID = OD.OrderID and I.ItemID = OD.ItemID and A.CustID = O.CustID and A.CustID = '{customerId}'";
            try
            {
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        Item item = new Item();
                        item.ItemId = dr["ItemID"].ToString();
                        item.ItemName = dr["ItemName"].ToString();
                        item.ItemSize = dr["Size"].ToString();
                        item.ItemUnitSize = dr["UnitSize"].ToString();
                        item.ItemBrand = dr["Brand"].ToString();
                        item.ItemOrigin = dr["Origin"].ToString();
                        item.ItemQuantity = Convert.ToInt32(dr["Quantity"]);
                        item.ItemPrice = Convert.ToDecimal(dr["Price"]);
                        item.CustomerName = dr["CustName"].ToString();
                        itemList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            conn.Close();
            return itemList;
        }
    }
}
