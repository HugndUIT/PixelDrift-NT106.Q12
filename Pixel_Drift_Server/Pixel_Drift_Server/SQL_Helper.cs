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
        SELECT PlayerName, WinCount, CrashCount, TotalScore, DatePlayed 
        FROM ScoreBoard 
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

                            // Format: rank|playerName|winCount|crashCount|totalScore|datePlayed
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
                SELECT PlayerName, WinCount, CrashCount, TotalScore, DatePlayed 
                FROM ScoreBoard 
                WHERE PlayerName LIKE @SearchText 
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

        public static bool ClearScoreBoard()
        {
            try
            {
                string query = "DELETE FROM ScoreBoard";
                using (var cmd = new SQLiteCommand(query, Connection))
                {
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing scoreboard: {ex.Message}");
                return false;
            }
        }
        public static void AddSampleData()
        {
            AddScore("Nguyễn Văn A", 15, 3, 2850.5);
            AddScore("Trần Thị B", 12, 5, 2456.8);
            AddScore("Lê Văn C", 18, 2, 3120.9);
            AddScore("Phạm Thị D", 10, 7, 2100.4);
            AddScore("Hoàng Văn E", 14, 4, 2680.3);
            AddScore("Đặng Thị F", 9, 8, 1950.7);
            AddScore("Vũ Văn G", 16, 3, 2920.1);
            AddScore("Bùi Thị H", 11, 6, 2230.6);

            Console.WriteLine("Sample data added successfully!");
        }

        public static void ClearSampleData()
        {
            try
            {
                string query = "DELETE FROM ScoreBoard WHERE PlayerName IN ('Nguyễn Văn A', 'Trần Thị B', 'Lê Văn C', 'Phạm Thị D', 'Hoàng Văn E', 'Đặng Thị F', 'Vũ Văn G', 'Bùi Thị H')";
                using (var cmd = new SQLiteCommand(query, Connection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    Console.WriteLine($"Đã xóa {rowsAffected} bản ghi mẫu");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing sample data: {ex.Message}");
            }
        }
    }
}