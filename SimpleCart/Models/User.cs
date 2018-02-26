using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SimpleCart;
using System;

namespace SimpleCart.Models
{
  public class User
  {
    private int _id;
    private string _name;
    private string _login;
    private string _password;
    private string _address;
    private string _email;
    Cart _myCart = new Cart();

    public User(name, login, password, address, email)
    {
      _name = name;
      _login = login;
      _password = password;
      _address = address;
      _email = email;
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

    public string GetLogin()
    {
      return _login;
    }

    public string GetPassword()
    {
      return _password;
    }

    public string GetAddress()
    {
      return _address;
    }

    public string GetEmail()
    {
      return _email;
    }

    public void Save()
    {
      MySqlConnection conn = DB.connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO users (name, login, password, address, email) VALUES (@userName, @userLogin, @userPassword, @userAddress, @userEmail);";

      MySqlParameter name = new MySqlParameter("@userName", _name);
      MySqlParameter login = new MySqlParameter("@userLogin", _login);
      MySqlParameter password = new MySqlParameter("@userPassword", _password);
      MySqlParameter address = new MySqlParameter("@userAddress", _address);
      MySqlParameter email = new MySqlParameter ("@userEmail", _email);
      cmd.Parameters.Add(name);
      cmd.Parameters.Add(login);
      cmd.Parameters.Add(password);
      cmd.Parameters.Add(address);
      cmd.Paramters.Add(email);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      _myCart = new Cart(_id);

      conn.Close()
      if (!(conn == null))
      {
        conn.Dispose()
      }
    }

    public User Find(int userId)
    {
      MySqlConnection conn = DB.connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users WHERE id=@userId;";

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      int id = 0;
      string name = "";
      string login = "";
      string password = "";
      string address = "";
      string email = "";

      while (rdr.Read())
      {
        id = rdr.GetInt32(0);
        name = rdr.GetString(1);
        login = rdr.GetString(2);
        password = rdr.GetString(3);
        address = rdr.GetString(4);
        email = rdr.GetString(5);
      }

      User myUser = new User(name, login, password, address, email);
      myUser.SetId(id);

      conn.Close();
      if (!(conn == null))
      {
        conn.Dispose();
      }
      return myUser;
    }

    public static void Delete(int idDelete)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM users WHERE id = @searchId;";

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
