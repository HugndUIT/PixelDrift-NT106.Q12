using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

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

            string SQL_Query_User = @"
            CREATE TABLE IF NOT EXISTS Info_User (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT UNIQUE,
                Email TEXT UNIQUE,
                Password TEXT,
                Birthday TEXT
            )";

            using (var Command = new SQLiteCommand(SQL_Query_User, Connection))
            {
                Command.ExecuteNonQuery();
            }

            string SQL_Query_ScoreBoard = @"
            CREATE TABLE IF NOT EXISTS ScoreBoard (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PlayerName TEXT NOT NULL,
                WinCount INTEGER DEFAULT 0,
                CrashCount INTEGER DEFAULT 0,
                TotalScore REAL DEFAULT 0,
                DatePlayed DATETIME DEFAULT CURRENT_TIMESTAMP
            )";

            using (var Command = new SQLiteCommand(SQL_Query_ScoreBoard, Connection))
            {
                Command.ExecuteNonQuery();
            }

            Console.WriteLine("Database initialized successfully!");
        }

        public static bool AddScore(string playerName, int winCount, int crashCount, double totalScore)
        {
            try
            {
                string query = @"
                INSERT INTO ScoreBoard (PlayerName, WinCount, CrashCount, TotalScore) 
                VALUES (@PlayerName, @WinCount, @CrashCount, @TotalScore)";

                using (var cmd = new SQLiteCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@PlayerName", playerName);
                    cmd.Parameters.AddWithValue("@WinCount", winCount);
                    cmd.Parameters.AddWithValue("@CrashCount", crashCount);
                    cmd.Parameters.AddWithValue("@TotalScore", totalScore);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding score: {ex.Message}");
                return false;
            }
        }

        public static string GetTopScores(int limit = 50)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                string query = @"
                SELECT 
                    PlayerName, 
                    SUM(WinCount) as WinCount, 
                    SUM(CrashCount) as CrashCount, 
                    SUM(TotalScore) as TotalScore, 
                    MAX(DatePlayed) as DatePlayed 
                FROM ScoreBoard 
                GROUP BY PlayerName
                ORDER BY TotalScore DESC, WinCount DESC, CrashCount ASC 
                LIMIT @Limit";

                using (var cmd = new SQLiteCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@Limit", limit);

                    using (var reader = cmd.ExecuteReader())
                    {
                        int rank = 1;
                        while (reader.Read())
                        {
                            string playerName = reader.GetString(0);
                            int winCount = reader.GetInt32(1);
                            int crashCount = reader.GetInt32(2);
                            double totalScore = reader.GetDouble(3);
                            string datePlayed = reader.GetString(4);

                            result.AppendLine($"{rank}|{playerName}|{winCount}|{crashCount}|{totalScore:F2}|{datePlayed}");
                            rank++;
                        }
                    }
                }

                return result.Length > 0 ? result.ToString() : "EMPTY";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting top scores: {ex.Message}");
                return "ERROR";
            }
        }

        public static string SearchPlayer(string searchText)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                string query = @"
                SELECT 
                    PlayerName, 
                    SUM(WinCount) as WinCount, 
                    SUM(CrashCount) as CrashCount, 
                    SUM(TotalScore) as TotalScore, 
                    MAX(DatePlayed) as DatePlayed 
                FROM ScoreBoard 
                WHERE PlayerName LIKE @SearchText 
                GROUP BY PlayerName
                ORDER BY TotalScore DESC 
                LIMIT 50";

                using (var cmd = new SQLiteCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@SearchText", $"%{searchText}%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        int rank = 1;
                        while (reader.Read())
                        {
                            string playerName = reader.GetString(0);
                            int winCount = reader.GetInt32(1);
                            int crashCount = reader.GetInt32(2);
                            double totalScore = reader.GetDouble(3);
                            string datePlayed = reader.GetString(4);

                            result.AppendLine($"{rank}|{playerName}|{winCount}|{crashCount}|{totalScore:F2}|{datePlayed}");
                            rank++;
                        }
                    }
                }

                return result.Length > 0 ? result.ToString() : "EMPTY";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching player: {ex.Message}");
                return "ERROR";
            }
        }
    }
}