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

    public User(string name,string login,string password,string address,string email)
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

    public bool Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT login FROM users WHERE login=@userLogin;";
      MySqlParameter userLogin = new MySqlParameter("@userLogin", _login);
      cmd.Parameters.Add(userLogin);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      string tempLogin = "";

      while(rdr.Read())
      {
        tempLogin = rdr.GetString(0);
        Console.WriteLine(tempLogin);
      }

      conn.Close();
      if (!(conn == null))
      {
        conn.Dispose();
      }

      if (tempLogin == "")
      {
        conn = DB.Connection();
        conn.Open();
        cmd = conn.CreateCommand() as MySqlCommand;
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
        cmd.Parameters.Add(email);

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;
        _myCart = new Cart(_id);

        conn.Close();
        if (!(conn == null))
        {
          conn.Dispose();
        }
        return true;
      }
      return false;
    }
    public User Login(User inputUser)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users WHERE name=@userName AND password=@userPassword;";

      MySqlParameter userName = new MySqlParameter();
      userName.ParameterName = "@userName";
      userName.Value = inputUser.GetLogin();
      cmd.Parameters.Add(userName);

      MySqlParameter userPassword = new MySqlParameter();
      userPassword.ParameterName = "@userPassword";
      userPassword.Value = inputUser.GetPassword();
      cmd.Parameters.Add(userPassword);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      int id = rdr.GetInt32(0);
      string name = rdr.GetString(1);
      string login = rdr.GetString(2);
      string password = rdr.GetString(3);
      string address = rdr.GetString(4);
      string email = rdr.GetString(5);

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
      if(myUser.GetLogin() == inputUser.GetLogin() && myUser.GetPassword() == inputUser.GetPassword())
      {
          return myUser;
      } else {
          return null;
      }
    }

    public User Find(int userId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users WHERE id=@userId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

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
