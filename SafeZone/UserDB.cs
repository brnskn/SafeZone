using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SafeZone
{
    static class UserDB
    {
        private static SQLiteConnection Connection = new SQLiteConnection("Data Source=User.db;Version=3;");
        private static List<UserModel> List = new List<UserModel>();

        public static UserModel CurrentUser = null;

        static UserDB()
        {
            checkDb();
            Connection.Open();
            checkTable();
            LoadList();
        }
        private static void checkDb()
        {
            if (!File.Exists("User.db"))
            {
                SQLiteConnection.CreateFile("User.db");
            }
        }
        private static void checkTable()
        {
            SQLiteCommand command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS `users` ( `id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, `username` TEXT NOT NULL, `password` TEXT NOT NULL )", Connection);
            command.ExecuteNonQuery();
        }
        public static void Init()
        {

        }

        public static long Insert(UserModel user)
        {
            SQLiteCommand command = new SQLiteCommand("insert into users (username, password) values('" + Hash(user.username) + "','"+ Hash(user.password) + "')", Connection);
            command.ExecuteNonQuery();
            LoadList();
            return Connection.LastInsertRowId;
        }

        public static UserModel GetUser(int id)
        {
            return List.Where(x => x.id == id).First();
        }

        public static UserModel GetUser(string username)
        {
            return List.Where(x => x.username == Hash(username)).First();
        }

        public static List<UserModel> UserList
        {
            get{
                return List;
            }
        }

        public static bool Any(string username, string password)
        {
            return List.Any(x => x.username == Hash(username) && x.password == Hash(password));
        }

        private static List<UserModel> LoadList()
        {
            string sql = "select * from users";
            SQLiteCommand command = new SQLiteCommand(sql, Connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                List.Add(new UserModel(Convert.ToInt32(reader["id"]), reader["username"].ToString(), reader["password"].ToString()));
            return List;
        }

        public static string Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
