using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Pixel_Drift_Server
{
    public static class SQL_Helper
    {
        public static SQLiteConnection Connection;

        public static void Initialize()
        {
            string Database_Path = "Data Source=Qly_Nguoi_Dung.db;Version=3;";
            Connection = new SQLiteConnection(Database_Path);
            Connection.Open();

            string SQL_Query = @"
            CREATE TABLE IF NOT EXISTS Info_User (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT UNIQUE,
                Email TEXT UNIQUE,
                Password TEXT,
                Birthday TEXT
            )";

            using (var Command = new SQLiteCommand(SQL_Query, Connection))
            {
                Command.ExecuteNonQuery();
            }
        }
    }
}
