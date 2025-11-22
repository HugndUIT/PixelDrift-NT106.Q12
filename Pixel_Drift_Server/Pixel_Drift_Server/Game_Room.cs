using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Pixel_Drift_Server
{
    public class Game_Room
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public bool Is_In_Game { get; private set; } = false;

        public Game_Player Player_1 { get; set; } // Host
        public Game_Player Player_2 { get; set; } // Guest

        public readonly object Room_Lock = new object();

        // Timers
        private System.Threading.Timer Countdown_Timer;
        private int Countdown_Value = 5;
        private System.Threading.Timer Game_Timer;
        private int Game_Time_Remaining = 60;
        private System.Threading.Timer Server_Game_Loop;
        private const int Tick_Rate = 18;
        private const int Player_Move_Speed = 9;

        // Game Objects
        private Dictionary<string, Point> Game_Objects = new Dictionary<string, Point>();
        private Dictionary<string, Size> Object_Sizes = new Dictionary<string, Size>();

        // Game Constants
        private const int Game_Height = 800;
        private const int P1_Min_X = 0, P1_Max_X = 475;
        private const int P2_Min_X = 0, P2_Max_X = 475;

        // State
        private int P1_Speed = 10, P2_Speed = 10;
        private long P1_Score = 0, P2_Score = 0;
        private bool P1_Left, P1_Right, P2_Left, P2_Right;
        private Random Rand = new Random();

        public Game_Room(string id, string name, Game_Player host)
        {
            ID = id;
            Name = name;
            Player_1 = host;
            Player_1.Is_Ready = false;
            Initialize_Game();
        }

        public bool Join(Game_Player guest)
        {
            lock (Room_Lock)
            {
                if (Player_2 != null || Is_In_Game) return false;

                Player_2 = guest;
                Player_2.Is_Ready = false;

                var data = new
                {
                    action = "player_joined",
                    P1_Name = Player_1.Username,
                    P2_Name = Player_2.Username,
                };
                Broadcast(JsonSerializer.Serialize(data));
                Broadcast_Ready_Status();
                return true;
            }
        }

        public bool IsEmpty()
        {
            return Player_1 == null && Player_2 == null;
        }

        public void Handle_Disconnect(TcpClient client)
        {
            lock (Room_Lock)
            {
                string leaverName = "Unknown";
                bool isHostLeaving = false;

                if (Player_1 != null && Player_1.Client == client)
                {
                    leaverName = Player_1.Username;
                    Player_1 = null; // Xóa Host
                    isHostLeaving = true;
                }
                else if (Player_2 != null && Player_2.Client == client)
                {
                    leaverName = Player_2.Username;
                    Player_2 = null; // Xóa Guest
                }
                else
                {
                    return;
                }

                // Dừng game ngay lập tức
                Stop_All_Timers();
                Is_In_Game = false;

                // Thông báo cho người còn lại
                Broadcast(JsonSerializer.Serialize(new { action = "player_disconnected", Name = leaverName, IsHost = isHostLeaving }));

                // Reset trạng thái người còn lại
                if (Player_1 != null) Player_1.Is_Ready = false;
                if (Player_2 != null) Player_2.Is_Ready = false;

                Broadcast_Ready_Status();
            }
        }

        private void Stop_All_Timers()
        {
            Countdown_Timer?.Dispose(); Countdown_Timer = null;
            Game_Timer?.Dispose(); Game_Timer = null;
            Server_Game_Loop?.Dispose(); Server_Game_Loop = null;
        }

        public void Handle_Player(int playerNum, string action, JsonElement data)
        {
            lock (Room_Lock)
            {
                switch (action)
                {
                    case "set_ready":
                        if (data.TryGetProperty("ready_status", out JsonElement readyEl))
                        {
                            bool isReady = readyEl.GetString() == "true";
                            if (playerNum == 1 && Player_1 != null) Player_1.Is_Ready = isReady;
                            else if (playerNum == 2 && Player_2 != null) Player_2.Is_Ready = isReady;

                            Broadcast_Ready_Status();
                            Check_Start_Countdown();
                        }
                        break;

                    case "move":
                        if (data.TryGetProperty("direction", out JsonElement dirEl) &&
                            data.TryGetProperty("state", out JsonElement stateEl))
                        {
                            string direction = dirEl.GetString();
                            bool isPressed = stateEl.GetString() == "down";

                            if (playerNum == 1)
                            {
                                if (direction == "left") P1_Left = isPressed;
                                else if (direction == "right") P1_Right = isPressed;
                            }
                            else if (playerNum == 2)
                            {
                                if (direction == "left") P2_Left = isPressed;
                                else if (direction == "right") P2_Right = isPressed;
                            }
                        }
                        break;
                }
            }
        }

        private void Broadcast(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message + "\n");
            try
            {
                if (Player_1?.Stream != null && Player_1.Client.Connected) Player_1.Stream.Write(data, 0, data.Length);
            }
            catch { }
            try
            {
                if (Player_2?.Stream != null && Player_2.Client.Connected) Player_2.Stream.Write(data, 0, data.Length);
            }
            catch { }
        }

        private void Broadcast_Ready_Status()
        {
            var status = new
            {
                action = "update_ready_status",
                player1_ready = Player_1?.Is_Ready ?? false,
                player1_name = Player_1?.Username ?? "Waiting...",
                player2_ready = Player_2?.Is_Ready ?? false,
                player2_name = Player_2?.Username ?? "Waiting..."
            };
            Broadcast(JsonSerializer.Serialize(status));
        }

        private void Check_Start_Countdown()
        {
            if (Player_1 != null && Player_1.Is_Ready &&
                Player_2 != null && Player_2.Is_Ready &&
                Countdown_Timer == null)
            {
                Countdown_Value = 5;
                Countdown_Timer = new System.Threading.Timer(Countdown_Tick, null, 1000, 1000);
            }
        }

        private void Countdown_Tick(object state)
        {
            if (Countdown_Value > 0)
            {
                Broadcast(JsonSerializer.Serialize(new { action = "countdown", time = Countdown_Value }));
                Countdown_Value--;
            }
            else
            {
                Countdown_Timer?.Dispose();
                Countdown_Timer = null;
                Is_In_Game = true; // Đánh dấu game đang chạy
                Broadcast(JsonSerializer.Serialize(new { action = "start_game" }));
                Initialize_Game();
                Server_Game_Loop = new System.Threading.Timer(Server_Game_Loop_Tick, null, 0, Tick_Rate);
                Game_Time_Remaining = 60;
                Game_Timer = new System.Threading.Timer(Game_Timer_Tick, null, 1000, 1000);
            }
        }

        private void Game_Timer_Tick(object state)
        {
            if (Game_Time_Remaining > 0)
            {
                Broadcast(JsonSerializer.Serialize(new { action = "update_time", time = Game_Time_Remaining }));
                lock (Room_Lock)
                {
                    P1_Score += P1_Speed;
                    P2_Score += P2_Speed;
                }
                Broadcast(JsonSerializer.Serialize(new { action = "update_score", p1_score = P1_Score, p2_score = P2_Score }));
                Game_Time_Remaining--;
            }
            else
            {
                Stop_All_Timers();
                SaveGameScores();
                Broadcast(JsonSerializer.Serialize(new { action = "game_over" }));

                lock (Room_Lock)
                {
                    Is_In_Game = false;
                    if (Player_1 != null) Player_1.Is_Ready = false;
                    if (Player_2 != null) Player_2.Is_Ready = false;
                }
                Broadcast_Ready_Status();
            }
        }

        private void Initialize_Game()
        {
            P1_Score = 0; P2_Score = 0;
            P1_Speed = 10; P2_Speed = 10;
            P1_Left = P1_Right = P2_Left = P2_Right = false;
            Game_Objects.Clear();
            Object_Sizes.Clear();

            Object_Sizes["ptb_player1"] = new Size(72, 117); Game_Objects["ptb_player1"] = new Point(202, 470);
            Object_Sizes["ptb_player2"] = new Size(72, 117); Game_Objects["ptb_player2"] = new Point(202, 470);

            Object_Sizes["ptb_roadtrack1"] = new Size(617, 734); Game_Objects["ptb_roadtrack1"] = new Point(0, -2);
            Object_Sizes["ptb_roadtrack1dup"] = new Size(617, 734); Game_Objects["ptb_roadtrack1dup"] = new Point(0, 734);
            Object_Sizes["ptb_roadtrack2"] = new Size(458, 596); Game_Objects["ptb_roadtrack2"] = new Point(0, 2);
            Object_Sizes["ptb_roadtrack2dup"] = new Size(458, 596); Game_Objects["ptb_roadtrack2dup"] = new Point(0, 596);

            string[] objects = { "ptb_AICar1", "ptb_AICar3", "ptb_AICar5", "ptb_AICar6",
                                 "ptb_increasingroad1", "ptb_decreasingroad1", "ptb_increasingroad2", "ptb_decreasingroad2" };

            foreach (var obj in objects)
            {
                Object_Sizes[obj] = obj.Contains("Car") ? new Size(50, 100) : new Size(30, 30);
                Game_Objects[obj] = Reposition_Object(obj, 50, 450);
            }
        }

        private Point Reposition_Object(string name, int minX, int maxX)
        {
            Size size = Object_Sizes.ContainsKey(name) ? Object_Sizes[name] : new Size(30, 30);
            int safeMaxX = maxX - size.Width;
            int x = Rand.Next(minX, safeMaxX);
            int y = Rand.Next(-1000, -150);
            return new Point(x, y);
        }

        private bool Check_Collision(string player, string obj)
        {
            if (!Game_Objects.ContainsKey(player) || !Game_Objects.ContainsKey(obj)) return false;
            Rectangle r1 = new Rectangle(Game_Objects[player], Object_Sizes[player]);
            Rectangle r2 = new Rectangle(Game_Objects[obj], Object_Sizes[obj]);
            return r1.IntersectsWith(r2);
        }

        private void Server_Game_Loop_Tick(object state)
        {
            try
            {
                lock (Room_Lock)
                {
                    if (Game_Timer == null) return;

                    // Xử lý di chuyển P1
                    if (Game_Objects.ContainsKey("ptb_player1"))
                    {
                        Point p1 = Game_Objects["ptb_player1"];
                        if (P1_Left && p1.X > P1_Min_X) p1.X -= Player_Move_Speed;
                        if (P1_Right && p1.X < P1_Max_X - 72) p1.X += Player_Move_Speed;
                        Game_Objects["ptb_player1"] = p1;
                    }

                    // Xử lý di chuyển P2
                    if (Game_Objects.ContainsKey("ptb_player2"))
                    {
                        Point p2 = Game_Objects["ptb_player2"];
                        if (P2_Left && p2.X > P2_Min_X) p2.X -= Player_Move_Speed;
                        if (P2_Right && p2.X < P2_Max_X - 72) p2.X += Player_Move_Speed;
                        Game_Objects["ptb_player2"] = p2;
                    }

                    // Scroll Map & Objects (Rút gọn code cho dễ nhìn, logic giữ nguyên)
                    Move_Object_Down("ptb_roadtrack1", P1_Speed, Game_Height, true);
                    Move_Object_Down("ptb_roadtrack1dup", P1_Speed, Game_Height, true);
                    Move_Object_Down("ptb_AICar1", P1_Speed, Game_Height, false, 50, 450);
                    Move_Object_Down("ptb_AICar5", P1_Speed, Game_Height, false, 50, 450);
                    Move_Object_Down("ptb_increasingroad1", P1_Speed, Game_Height, false, 50, 450);
                    Move_Object_Down("ptb_decreasingroad1", P1_Speed, Game_Height, false, 50, 450);

                    Move_Object_Down("ptb_roadtrack2", P2_Speed, Game_Height, true);
                    Move_Object_Down("ptb_roadtrack2dup", P2_Speed, Game_Height, true);
                    Move_Object_Down("ptb_AICar3", P2_Speed, Game_Height, false, 50, 450);
                    Move_Object_Down("ptb_AICar6", P2_Speed, Game_Height, false, 50, 450);
                    Move_Object_Down("ptb_increasingroad2", P2_Speed, Game_Height, false, 50, 450);
                    Move_Object_Down("ptb_decreasingroad2", P2_Speed, Game_Height, false, 50, 450);

                    // Collision Check P1
                    if (Check_Collision("ptb_player1", "ptb_increasingroad1")) { P1_Speed += 3; Reposition_Object("ptb_increasingroad1", 50, 450); }
                    if (Check_Collision("ptb_player1", "ptb_decreasingroad1")) { P1_Speed -= 3; Reposition_Object("ptb_decreasingroad1", 50, 450); }

                    // Collision Check P2
                    if (Check_Collision("ptb_player2", "ptb_increasingroad2")) { P2_Speed += 3; Reposition_Object("ptb_increasingroad2", 50, 450); }
                    if (Check_Collision("ptb_player2", "ptb_decreasingroad2")) { P2_Speed -= 3; Reposition_Object("ptb_decreasingroad2", 50, 450); }

                    // Gửi trạng thái
                    var gameState = new Dictionary<string, object> { { "action", "update_game_state" } };
                    foreach (var pair in Game_Objects) gameState[pair.Key] = pair.Value;
                    Broadcast(JsonSerializer.Serialize(gameState));
                }
            }
            catch { }
        }

        private void Move_Object_Down(string name, int speed, int screenHeight, bool isRoad, int minX = 0, int maxX = 0)
        {
            if (!Game_Objects.ContainsKey(name)) return;
            Point pos = Game_Objects[name];
            pos.Y += speed;

            if (pos.Y > screenHeight)
            {
                if (isRoad)
                {
                    string dupName = name.EndsWith("dup") ? name.Replace("dup", "") : name + "dup";
                    if (Game_Objects.ContainsKey(dupName)) pos.Y = Game_Objects[dupName].Y - screenHeight;
                }
                else
                {
                    pos = Reposition_Object(name, minX, maxX);
                }
            }
            Game_Objects[name] = pos;
        }

        private void SaveGameScores()
        {
            try
            {
                if (Player_1 != null) SQL_Helper.AddScore(Player_1.Username, P1_Score > P2_Score ? 1 : 0, 0, P1_Score);
                if (Player_2 != null) SQL_Helper.AddScore(Player_2.Username, P2_Score > P1_Score ? 1 : 0, 0, P2_Score);
            }
            catch { }
        }
    }
}