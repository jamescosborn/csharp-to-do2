using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
    public class Category
    {
        public string Name {get; private set;}
        public int Id {get; private set;}

        public Category(string name, int id = 0)
        {
            Name = name;
            Id = id;
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO `categories` (`name`) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = Name;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            Id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM categories";
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static List<Category> GetAll()
        {
            List<Category> allCategories = new List<Category> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM categories;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
              int categoryId = rdr.GetInt32(0);
              string categoryName = rdr.GetString(1);
              Category newCategory = new Category(categoryName, categoryId);
              allCategories.Add(newCategory);
            }
            conn.Close();
            if (conn != null)
            {
              conn.Dispose();
            }
            return allCategories;
        }
        public static Category Find(int searchId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText =  @"SELECT * FROM `categories` WHERE id = @thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = searchId;
            cmd.Parameters.Add(thisId);

            int categoryId = 0;
            string categoryName = "";

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                categoryId = rdr.GetInt32(0);
                categoryName = rdr.GetString(1);
            }

            Category foundCategory = new Category(categoryName, categoryId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundCategory;
        }


        public override bool Equals(System.Object otherCategory)
        {
            if (!(otherCategory is Category))
            {
                return false;
            }
            else
            {
                Category newCategory = (Category) otherCategory;
                bool idEquality = (this.Id == newCategory.Id);
                bool nameEquality = (this.Name == newCategory.Name);
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
