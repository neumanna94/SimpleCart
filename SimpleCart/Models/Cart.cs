using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SimpleCart;
using System;

namespace SimpleCart.Models
{
  public class Cart
  {
    private int _sessionId;

    public Cart()
    {
      _sessionId= -1;
    }

    public Cart(int sessionId)
    {
      _sessionId = sessionId;
    }

    public void AddItem(int itemId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cart_items (user_id, item_id) VALUES (@userId, @itemId);";

      MySqlParameter userId = new MySqlParameter("@userId", this.GetUserId());
      MySqlParameter tempItemId = new MySqlParameter("@itemId", itemId);
      cmd.Parameters.Add(userId);
      cmd.Parameters.Add(tempItemId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public int GetUserId()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT user_id FROM sessions WHERE session_id=@sessionId;";

      MySqlParameter sessionId = new MySqlParameter("@sessionId", _sessionId);
      cmd.Parameters.Add(sessionId);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      int myUserId = 0;

      while (rdr.Read())
      {
        myUserId = rdr.GetInt32(0);
      }

      conn.Dispose();
      return myUserId;
    }

    public List<Item> GetItems()
    {
      //Opening Database Connection.
      List<Item> allItems = new List<Item> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      //Casting and Executing Commands.
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT items.* FROM sessions JOIN users ON (sessions.user_id = users.id) JOIN cart_items ON (users.id = cart_items.user_id) JOIN items ON (items.id = cart_items.item_id) WHERE sessions.session_id = @sessionId;";

      MySqlParameter sessionId = new MySqlParameter ("@sessionId", _sessionId);
      cmd.Parameters.Add(sessionId);

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
      cmd.CommandText = @"DELETE FROM cart_items WHERE (item_id = @itemId) AND (user_id = @userId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@itemId";
      searchId.Value = itemId;
      cmd.Parameters.Add(searchId);

      MySqlParameter userId = new MySqlParameter("@userId", this.GetUserId());
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

      MySqlParameter userId = new MySqlParameter ("@userId", this.GetUserId());
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
    //True if items were removed from cart_items. DO not check if those items are now in orders table.
    public bool Checkout()
    {
        bool outputState = true;
        MySqlConnection conn = DB.Connection();
        conn.Open();
        //Casting and Executing Commands.
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO orders SELECT * FROM cart_items WHERE user_id = @userId; DELETE FROM cart_items WHERE user_id = @userId;";
        MySqlParameter userId = new MySqlParameter ("@userId", this.GetUserId());
        cmd.Parameters.Add(userId);
        cmd.ExecuteNonQuery();
        cmd.CommandText = @"SELECT * FROM cart_items WHERE user_id = @userId;";
        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        //Contains built in method .Read()
        while(rdr.Read())
        {
            outputState = false;
        }
        conn.Dispose();
        return outputState;
    }
  }
}
