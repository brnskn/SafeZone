using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace SafeZone
{
    static class FileDB
    {
        private static SQLiteConnection Connection = new SQLiteConnection("Data Source=File.db;Version=3;");
        private static List<FileModel> List = new List<FileModel>();

        static FileDB()
        {
            Connection.Open();
            LoadList();
        }

        public static void Init()
        {

        }

        public static void Insert(SafeFile file)
        {
            SQLiteCommand command = new SQLiteCommand("insert into files (path, hash) values('"+file.FullPath+"','"+file.Hash+"')", Connection);
            command.ExecuteNonQuery();
            LoadList();
        }
        
        public static void Update(SafeFile file)
        {
            FileModel fm = List.Where(x => x.title == file.Title).First();
            SQLiteCommand command = new SQLiteCommand("update files set path='"+file.FullPath+"', hash='"+file.Hash+"' where id=" + fm.id, Connection);
            command.ExecuteNonQuery();
            LoadList();
        }
        public static void Update(FileModel fileModel)
        {
            SafeFile file = new SafeFile(fileModel.path);
            SQLiteCommand command = new SQLiteCommand("update files set path='" + file.FullPath + "', hash='" + file.Hash + "' where id=" + fileModel.id, Connection);
            command.ExecuteNonQuery();
            LoadList();
        }
        public static void Delete(int id)
        {
            SQLiteCommand command = new SQLiteCommand("delete from files where id="+id, Connection);
            command.ExecuteNonQuery();
            LoadList();
        }

        public static void Delete(string path)
        {
            FileModel file = List.Where(x => x.title == path).First();
            SQLiteCommand command = new SQLiteCommand("delete from files where path='" + file.path + "'", Connection);
            command.ExecuteNonQuery();
            LoadList();
        }

        public static FileModel GetFile(int id)
        {
            return List.Where(x => x.id == id).First();
        }

        public static FileModel GetFile(string path)
        {
            return List.Where(x => x.path == path).First();
        }

        public static List<FileModel> FileList
        {
            get{
                return List;
            }
        }

        private static List<FileModel> LoadList()
        {
            string sql = "select * from files";
            SQLiteCommand command = new SQLiteCommand(sql, Connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                List.Add(new FileModel(Convert.ToInt32(reader["id"]), reader["path"].ToString(), reader["hash"].ToString()));
            return List;
        }
    }
}
