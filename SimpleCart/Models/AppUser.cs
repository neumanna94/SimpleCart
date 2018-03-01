using System.Collections.Generic;
using BCrypt.Net;
using MySql.Data.MySqlClient;
using SimpleCart;
using System;

namespace SimpleCart.Models
{
  public class AppUser
  {
    private int _id;
    private string _name;
    private string _login;
    private string _password;
    private string _address;
    private string _email;
    Cart _myCart = new Cart();

    public AppUser(string name,string login,string password,string address,string email)
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
        cmd.CommandText = @"INSERT INTO users (login, password, name, email, address) VALUES (@userLogin, @userPassword, @userName, @userEmail, @userAddress);";

        MySqlParameter name = new MySqlParameter("@userName", _name);
        MySqlParameter login = new MySqlParameter("@userLogin", _login);
        MySqlParameter password = new MySqlParameter("@userPassword", BCrypt.Net.BCrypt.HashPassword(_password));
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
    public static int Login(string login, string password)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users WHERE login=@userLogin;";

      MySqlParameter userLogin = new MySqlParameter("@userLogin", login);
      cmd.Parameters.Add(userLogin);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      bool flag = false;
      int myUserId = 0;
      int mySessionId = -1;
      string databasePassword = "";

      while (rdr.Read())
      {
        flag = true;
        myUserId = rdr.GetInt32(0);
        databasePassword = rdr.GetString(2);
        Console.WriteLine(databasePassword);
      }

      rdr.Dispose();
      Console.WriteLine(flag);
      Console.WriteLine("BCrypt Verify: " + BCrypt.Net.BCrypt.Verify(password, databasePassword));
      if (flag && BCrypt.Net.BCrypt.Verify(password, databasePassword))
      {
        Console.WriteLine("Logged in successfully");
        cmd.CommandText = @"INSERT INTO sessions (user_id, session_id) VALUES (@userId, @sessionId);";

        MySqlParameter userId = new MySqlParameter("@userId", myUserId);
        cmd.Parameters.Add(userId);
        MySqlParameter sessionId = new MySqlParameter();
        sessionId.ParameterName = "@sessionId";
        Random newRandom = new Random();
        int randomNumber = newRandom.Next(10000000);
        sessionId.Value = randomNumber;
        cmd.Parameters.Add(sessionId);

        cmd.ExecuteNonQuery();
        mySessionId = randomNumber;
      }

      conn.Close();
      if (!(conn == null))
      {
        conn.Dispose();
      }

      return mySessionId;
    }

    public static void Logout(int sessionId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM sessions WHERE session_id=@sessionId;";

      MySqlParameter mySessionId = new MySqlParameter("@sessionId", sessionId);
      cmd.Parameters.Add(mySessionId);

      cmd.ExecuteNonQuery();

      conn.Dispose();
    }

    public static List<string> Forgot(string name, string login, string email)
    {
      List<string> info = new List<string>();
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users WHERE name=@userName AND login=@userLogin AND email=@userEmail;";

      MySqlParameter userName = new MySqlParameter("@userName", name);
      MySqlParameter userLogin = new MySqlParameter("@userLogin", login);
      MySqlParameter userPassword = new MySqlParameter("@userEmail", email);
      cmd.Parameters.Add(userName);
      cmd.Parameters.Add(userLogin);
      cmd.Parameters.Add(userPassword);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

      bool flag = false;
      int myUserId = 0;

      while (rdr.Read())
      {
        flag = true;
        myUserId = rdr.GetInt32(0);
      }

      rdr.Dispose();
      if (flag)
      {
        cmd.CommandText = @"SELECT * FROM users WHERE id = @userId;";

        MySqlParameter userId = new MySqlParameter("@userId", myUserId);
        cmd.Parameters.Add(userId);
        MySqlDataReader rdr2 = cmd.ExecuteReader() as MySqlDataReader;
        while (rdr2.Read())
        {
            string tempLogin = rdr2.GetString(1);
            info.Add(tempLogin);
            string tempPass = rdr2.GetString(2);
            info.Add(tempPass);
        }
      }
      conn.Dispose();
      return info;
    }

    public static AppUser Find(int userId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM users WHERE id=@userId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@userId";
      searchId.Value = userId;
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
        login = rdr.GetString(1);
        password = rdr.GetString(2);
        name = rdr.GetString(3);
        email = rdr.GetString(4);
        address = rdr.GetString(5);
      }

      AppUser myUser = new AppUser(name, login, password, address, email);
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

    public static void Update(string name,string login,string password,string address,string email, int userId)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE users SET login=@login, password = @password, name = @name, email = @email, address = @address WHERE id = @userId;";

        MySqlParameter tempName = new MySqlParameter("@name", name);
        MySqlParameter tempLogin = new MySqlParameter("@login", login);
        MySqlParameter tempPassword = new MySqlParameter("@password", password);
        MySqlParameter tempAddress = new MySqlParameter("@address", address);
        MySqlParameter tempEmail = new MySqlParameter("@email", email);
        MySqlParameter tempUserId = new MySqlParameter("@userId", userId);
        Console.WriteLine("Name: " + tempName.Value + " Login: " + tempLogin.Value + " Password: " + tempPassword.Value + " Address: " + tempAddress.Value + " Email: " + tempEmail.Value + " ID: " + tempUserId.Value);

        cmd.Parameters.Add(tempName);
        cmd.Parameters.Add(tempLogin);
        cmd.Parameters.Add(tempPassword);
        cmd.Parameters.Add(tempAddress);
        cmd.Parameters.Add(tempEmail);
        cmd.Parameters.Add(tempUserId);


        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
  }
}
