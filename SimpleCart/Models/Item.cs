using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SimpleCart;
using System;

namespace SimpleCart.Models
{
    public class Item
    {
        private int _id;
        private string _name;
        private string _description;
        private double _cost;
        private string _imgUrl;
        private int _stock;

        public Item(string name, string description, double cost, string imgUrl, int stock)
        {
            _name = name;
            _cost = cost;
            _description = description;
            _imgUrl = imgUrl;
            _stock = stock;
        }

        public void SetId(int id)
        {
          _id = id;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public double GetCost()
        {
            return _cost;
        }

        public string GetDescription()
        {
          return _description;
        }

        public string GetImgUrl()
        {
          return _imgUrl;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO inventory_amazon (name, cost, description, imgUrl, stock ) VALUES (@itemName, @itemCost, @itemDescription, @itemImgUrl, @itemStock;";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@ItemName";
            name.Value = _name;
            cmd.Parameters.Add(name);

            MySqlParameter cost = new MySqlParameter();
            cost.ParameterName = "@ItemCost";
            cost.Value = _cost;
            cmd.Parameters.Add(cost);

            MySqlParameter description = new MySqlParameter("@itemDescription", _description);
            cmd.Parameters.Add(description);

            MySqlParameter imgUrl = new MySqlParameter("@imgUrl", _imgUrl);
            cmd.Parameters.Add(imgUrl);

            MySqlParameter stock = new MySqlParameter("@stock", _stock);
            cmd.Parameters.Add(stock);

            cmd.ExecuteNonQuery();

            _id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Item> GetAll(orderBy)
        {
            //Opening Database Connection.
            List<Item> allItems = new List<Item> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            //Casting and Executing Commands.
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM inventory_amazon ORDER BY "+orderBy+";";

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            //Contains built in method .Read()
            while(rdr.Read())
            {
              int itemId = rdr.GetInt32(0);
              string name = rdr.GetString(1);
              string description = rdr.GetString(2);
              double cost = rdr.GetDouble(3);
              string imgUrl = rdr.GetString(4);
              int stock = rdr.GetInt32(5);


              Item newItem = new Item(name, description, cost, imgUrl, stock);
              newItem.SetId(itemId);
              allItems.Add(newItem);
            }
            //Close connection
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allItems;
        }
        public static Item Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM inventory_amazon WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int itemId = 0;
            string name = "";
            string description = "";
            double cost = 0;
            string imgUrl = "";
            int stock = 0;

            while(rdr.Read())
            {
              itemId = rdr.GetInt32(0);
              name = rdr.GetString(1);
              description = rdr.GetString(2);
              cost = rdr.GetDouble(3);
              imgUrl = rdr.GetString(4);
              stock = rdr.GetInt32(5);
            }

            Item newItem = new Item(name, description, cost, imgUrl, stock);
            newItem.SetId(itemId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newItem;
        }

        // public static void DeleteAll()
        // {
        //     MySqlConnection conn = DB.Connection();
        //     conn.Open();
        //     var cmd = conn.CreateCommand() as MySqlCommand;
        //     cmd.CommandText = @"DELETE FROM items;";
        //     cmd.ExecuteNonQuery();
        //     conn.Close();
        //     if (conn != null)
        //     {
        //         conn.Dispose();
        //     }
        // }

        public static void Delete(int idDelete)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM inventory_amazon WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = idDelete;
            cmd.Parameters.Add(searchId);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        // public static string DisplayList()
        // {
        //     List<Item> allItems = new List<Item>{};
        //     string outputString = "";
        //     allItems = GetAll();
        //     for(int i = 0; i < allItems.Count; i ++)
        //     {
        //         outputString += "(" + allItems[i].GetId() + ", " + allItems[i].GetName() + ", " + allItems[i].GetCost() + ", " + allItems[i].GetPostDate() +  ") ";
        //     }
        //     return outputString;
        // }
    }

}
