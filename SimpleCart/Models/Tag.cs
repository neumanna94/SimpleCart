using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SimpleCart;
using System;

namespace SimpleCart.Models
{
    public class Tag
    {
        private string _name;
        private int _id;

        public Tag(string name)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetId(int id)
        {
            _id = id;
        }

        public int GetId()
        {
            return _id;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO tags (name) VALUES (@tagName);";

            MySqlParameter name = new MySqlParameter("@tagName", _name);
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();

            conn.Dispose();
        }

        public static List<Tag> GetAll()
        {
            List<Tag> myTags = new List<Tag>();
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM tags;";

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                Tag myTag = new Tag(name);
                myTag.SetId(id);
                myTags.Add(myTag);
            }

            conn.Dispose();
            return myTags;
        }

        public static Tag Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM tags WHERE id=@id;";

            MySqlParameter tagId = new MySqlParameter("@id", id);
            cmd.Parameters.Add(tagId);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            string name = "";
            int myId = 0;

            while (rdr.Read())
            {
                myId = (int) rdr.GetInt32(0);
                name = rdr.GetString(1);
            }

            Tag myTag = new Tag(name);
            myTag.SetId(myId);

            conn.Dispose();

            return myTag;
        }


    }
}
