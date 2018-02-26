using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SimpleCart;
using System;

namespace SimpleCart.Models
{
  public class Cart
  {
    private int _userId;

    public Cart(int userId)
    {
      _userId = userId;
    }

    public void AddItem(int itemId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cart_items (user_id, cart_id) VALUES (@userId, @itemId);";

      MySqlParameter userId = new MySqlParameter("@userId", _userId);
      MySqlParamter itemId = new MySqlParameter("@itemId", itemId);
      cmd.Parameters.Add(userId);
      cmd.Parameters.Add(itemId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (!(conn == null));
      {
        conn.Dispose();
      }
    }

    public List<Item> GetItems()
    {
      //Opening Database Connection.
      List<Item> allItems = new List<Item> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      //Casting and Executing Commands.
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT items.* FROM users JOIN cart_items ON (users.id = cart_items.user_id) JOIN items ON (items.id = cart_items.item_id) WHERE users.id = @userId;";

      MySqlParameter userId = newMySqlParameter ("@userId", _userId);
      cmd.Parameters.Add(userId);

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

    public void DeleteItem(int itemId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM cart_items WHERE JOIN  WHERE (item_id = @itemId) AND (user_id = @userId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@itemId";
      searchId.Value = idDelete;
      cmd.Parameters.Add(searchId);

      MySqlParameter userId = new MySqlParameter("@userId", _userId);
      cmd.Parameters.Add(userId);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public double GetTotalCost()
    {
      double totalCost = 0;
      MySqlConnection conn = DB.Connection();
      conn.Open();
      //Casting and Executing Commands.
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT items.cost FROM users JOIN cart_items ON (users.id = cart_items.user_id) JOIN items ON (items.id = cart_items.item_id) WHERE users.id = @userId;";

      MySqlParameter userId = newMySqlParameter ("@userId", _userId);
      cmd.Parameters.Add(userId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      //Contains built in method .Read()
      while(rdr.Read())
      {
        totalCost += rdr.GetDouble(0);
      }

      conn.Close();
      if (!(conn == null))
      {
        conn.Dispose();
      }
      return totalCost; 
    }
  }
}
