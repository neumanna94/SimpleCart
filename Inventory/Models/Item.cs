using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Inventory;
using System;

namespace Inventory.Models
{
    public class Item
    {
        private int _id;
        private string _name;
        private int _cost;
        private string _postDate = "";

        public Item(string name, int cost, string postDate, int Id = 0)
        {
            _id = Id;
            _name = name;
            _cost = cost;
            _postDate = postDate;
        }
        public int GetId()
        {
            return _id;
        }
        public string GetName()
        {
            return _name;
        }
        public int GetCost()
        {
            return _cost;
        }
        public string GetPostDate()
        {
            return _postDate;
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO inventory_amazon (name, cost, postDate) VALUES (@ItemName, @ItemCost, @ItemPostDate);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@ItemName";
            name.Value = _name;
            cmd.Parameters.Add(name);

            MySqlParameter cost = new MySqlParameter();
            cost.ParameterName = "@ItemCost";
            cost.Value = _cost;
            cmd.Parameters.Add(cost);

            MySqlParameter postDate = new MySqlParameter();
            postDate.ParameterName = "@ItemPostDate";
            postDate.Value = _postDate;
            cmd.Parameters.Add(postDate);
            cmd.ExecuteNonQuery();

            _id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Item> GetAll()
        {
            //Opening Database Connection.
            List<Item> allItems = new List<Item> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            //Casting and Executing Commands.
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM inventory_amazon;";
            //End of CommandText
            //Using Data Reader Object(Represents actual reading of SQL database.)
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            //Contains built in method .Read()
            while(rdr.Read())
            {
              int itemId = rdr.GetInt32(0);
              string name = rdr.GetString(1);
              int cost = rdr.GetInt32(2);
              string postDate = rdr.GetString(3);

              Item newItem = new Item(name, cost, postDate, itemId);
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
            cmd.CommandText = @"SELECT * FROM items WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int itemId = 0;
            string name = "";
            int cost = 0;
            string postDate = "";

            while(rdr.Read())
            {
                itemId = rdr.GetInt32(0);
                name = rdr.GetString(1);
                cost = rdr.GetInt32(2);
                postDate = rdr.GetString(3);
            }
            Item newItem = new Item(name, cost, postDate, itemId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newItem;
        }
        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM items;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static void DeleteRow(int idDelete)
        {

        }
        public static string DisplayList()
        {
            List<Item> allItems = new List<Item>{};
            string outputString = "";
            allItems = GetAll();
            for(int i = 0; i < allItems.Count; i ++)
            {
                outputString += "(" + allItems[i].GetId() + ", " + allItems[i].GetName() + ", " + allItems[i].GetCost() + ", " + allItems[i].GetPostDate() +  ") ";
            }
            return outputString;
        }
    }

}
