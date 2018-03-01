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
        public int GetStock()
        {
            return _stock;
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
            cmd.CommandText = @"INSERT INTO items (name, cost, description, imgUrl, stock ) VALUES (@itemName, @itemCost, @itemDescription, @itemImgUrl, @itemStock);";

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

        public static List<Item> GetAll(string orderByInput)
        {
            //Opening Database Connection.
            List<Item> allItems = new List<Item> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            //Casting and Executing Commands.
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM items ORDER BY "+orderByInput+";";

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

        public List<Tag> GetTags()
        {
            List<Tag> myTags = new List<Tag>();

            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = "SELECT tags.* FROM items JOIN items_tags ON (items.id = items_tags.item_id) JOIN tags ON (tags.id = items_tags.tag_id) WHERE items.id = @item_id;";
            MySqlParameter itemId = new MySqlParameter("@item_id", this.GetId());
            cmd.Parameters.Add(itemId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int tagId = (int) rdr.GetInt32(0);
                string tagName = rdr.GetString(1);
                Tag myTag = new Tag(tagName);
                myTag.SetId(tagId);
                myTags.Add(myTag);
            }
            conn.Dispose();
            return myTags;
        }

        public static List<Item> GetAllByTags(List<Tag> tags)
        {
            List<Item> myItems = new List<Item>();

            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = "SELECT items.id FROM tags JOIN items_tags ON (tags.id = items_tags.tag_id) JOIN items ON (items.id = items_tags.item_id) WHERE ";

            //Dynamically creates commandtext to return only unique items

            int i = 1;
            foreach (Tag tag in tags)
            {
                int id = tag.GetId();
                string stringSegment = "tags.id = @tagId" + i.ToString() + " OR ";

                MySqlParameter myTagId = new MySqlParameter("@tagId" + i.ToString(), tag.GetId());
                cmd.Parameters.Add(myTagId);
                cmd.CommandText += stringSegment;
                i++;
            }

            cmd.CommandText = cmd.CommandText.TrimEnd(new char[]{' ', 'O', 'R', ' '}) + ";";

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<int> itemIds = new List<int>();

            while (rdr.Read())
            {
                int thisItemId = (int) rdr.GetInt32(0);
                if (!itemIds.Contains(thisItemId))
                {
                    itemIds.Add(thisItemId);
                    Console.WriteLine(itemIds.Count);
                }
            }

            conn.Close();
            cmd.CommandText = @"SELECT * FROM items WHERE id=@itemId;";
            MySqlParameter myItemId = new MySqlParameter("@itemId", 0);
            cmd.Parameters.Add(myItemId);

            foreach (int itemId in itemIds)
            {
                conn.Open();
                myItemId.Value = itemId;
                rdr = cmd.ExecuteReader() as MySqlDataReader; 
                while (rdr.Read())
                {
                    int tempItemId = rdr.GetInt32(0);
                    string name = rdr.GetString(1);
                    string description = rdr.GetString(2);
                    double cost = rdr.GetDouble(3);
                    string imgUrl = rdr.GetString(4);
                    int stock = rdr.GetInt32(5);


                    Item newItem = new Item(name, description, cost, imgUrl, stock);
                    newItem.SetId(tempItemId);
                    myItems.Add(newItem);
                }
                conn.Close();
            }
            conn.Dispose();
            return myItems;
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

        public void AddTag(int tagId)
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"INSERT INTO items_tags (item_id, tag_id) VALUES (@itemId, @tagId);";

          MySqlParameter itemId = new MySqlParameter("@itemId", _id);
          MySqlParameter myTagId = new MySqlParameter ("@tagId", tagId);
          cmd.Parameters.Add(itemId);
          cmd.Parameters.Add(myTagId);

          cmd.ExecuteNonQuery();

          conn.Dispose();
        }

        public static void Delete(int idDelete)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM items WHERE id = @searchId;";

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
    }

}
