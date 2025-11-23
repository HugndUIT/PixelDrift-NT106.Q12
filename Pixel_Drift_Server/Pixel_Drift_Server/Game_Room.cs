using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Pixel_Drift_Server.ServerForm;

namespace Pixel_Drift_Server
{
    public class Game_Room
    {
        public string Room_ID { get; private set; }
        public Action<string> Log_Action;

        // Khởi tạo người chơi
        private Game_Player Player_1 = new Game_Player();
        private Game_Player Player_2 = new Game_Player();
        private readonly object Player_Lock = new object();

        // Khởi tạo thời gian
        private System.Threading.Timer Countdown_Timer;
        private int Countdown_Value = 5;
        private System.Threading.Timer Game_Timer;
        private int Game_Time_Remaining = 60;
        private System.Threading.Timer Server_Game_Loop;
        private const int Tick_Rate = 18;
        private const int Player_Move_Speed = 9;

        // Lưu trữ vị trí (Point) của tất cả các đối tượng trong game
        private Dictionary<string, Point> Game_Objects = new Dictionary<string, Point>();

        // Lưu trữ kích thước (Size) của các đối tượng trong game
        private Dictionary<string, Size> Object_Sizes = new Dictionary<string, Size>();

        // Khởi tạo hai xe
        private int P1_Speed = 10;
        private int P2_Speed = 10;
        private long P1_Score = 0;
        private long P2_Score = 0;
        private bool P1_Left, P1_Right, P2_Left, P2_Right;
        private const int Game_Height = 800;
        private const int P1_Min_X = 0;
        private const int P1_Max_X = 475;
        private const int P2_Min_X = 0;
        private const int P2_Max_X = 475;

        // Quản lý ai đang đăng nhập theo email
        private Dictionary<string, Game_Player> ConnectedEmails = new Dictionary<string, Game_Player>();
        private readonly object ConnectedEmailsLock = new object();

        // Hàm random
        private Random Rand = new Random();

        public Game_Room(string ID)
        {
            this.Room_ID = ID;
        }

        public bool IsEmpty()
        {
            return Player_1 == null && Player_2 == null;
        }

        // Hàm được ServerForm gọi khi có người Join
        public int Add_Player(TcpClient client, string username)
        {
            lock (Player_Lock)
            {
                if (Player_1 == null)
                {
                    Player_1 = new Game_Player { Client = client, Stream = client.GetStream(), Username = username, Player_ID = 1 };
                    Broadcast_Ready_Status();
                    return 1; // Là Player 1
                }
                else if (Player_2 == null)
                {
                    Player_2 = new Game_Player { Client = client, Stream = client.GetStream(), Username = username, Player_ID = 2 };
                    Broadcast_Ready_Status();
                    return 2; // Là Player 2
                }
                return -1; // Phòng đầy
            }
        }

        public void Remove_Player(TcpClient client)
        {
            lock (Player_Lock)
            {
                string leftUser = "Unknown";
                if (Player_1 != null && Player_1.Client == client)
                {
                    leftUser = Player_1.Username;
                    Player_1 = null;
                }
                else if (Player_2 != null && Player_2.Client == client)
                {
                    leftUser = Player_2.Username;
                    Player_2 = null;
                }

                // Dừng game nếu đang chạy
                StopGame();
                Broadcast(JsonSerializer.Serialize(new { action = "player_disconnected", username = leftUser }));
                Broadcast_Ready_Status();
            }
        }

        public void Handle_Input(TcpClient client, string action, Dictionary<string, JsonElement> data)
        {
            int playerID = 0;
            if (Player_1 != null && Player_1.Client == client) playerID = 1;
            else if (Player_2 != null && Player_2.Client == client) playerID = 2;

            if (playerID == 0) return;

            switch (action)
            {
                case "set_ready":
                    bool ready = data["ready_status"].GetString() == "true";
                    if (playerID == 1) Player_1.Is_Ready = ready;
                    else Player_2.Is_Ready = ready;

                    Broadcast_Ready_Status();
                    Check_Start_Countdown();
                    break;

                case "move":
                    string direction = data["direction"].GetString();
                    string state = data["state"].GetString();
                    bool isPressed = (state == "down");

                    lock (Player_Lock)
                    {
                        if (playerID == 1)
                        {
                            if (direction == "left") P1_Left = isPressed;
                            else if (direction == "right") P1_Right = isPressed;
                        }
                        else if (playerID == 2)
                        {
                            if (direction == "left") P2_Left = isPressed;
                            else if (direction == "right") P2_Right = isPressed;
                        }
                    }
                    break;

                case "leave_room":
                    Remove_Player(client);
                    break;
            }
        }

        private void StopGame()
        {
            Server_Game_Loop?.Dispose();
            Server_Game_Loop = null;
            Game_Timer?.Dispose();
            Game_Timer = null;
            Countdown_Timer?.Dispose();
            Countdown_Timer = null;
        }

        // Hàm gọi invoke cho server hiển thị tin nhắn
        private void Log_Game_Room(string Message)
        {
            Log_Action?.Invoke(Message);
        }

        // Gửi tin nhắn đến một luồng mạng cụ thể
        private void Send_Message(NetworkStream Stream, string Message)
        {
            if (Stream == null || !Stream.CanWrite) return;
            try
            {
                byte[] Response_Bytes = Encoding.UTF8.GetBytes(Message + "\n");
                Stream.Write(Response_Bytes, 0, Response_Bytes.Length);
                Stream.Flush();
            }
            catch (Exception Ex)
            {
                Log_Game_Room($"Lỗi khi gửi tin nhắn: {Ex.Message}");
            }
        }

        // Phát sóng tin nhắn đến tất cả người chơi đang kết nối
        private void Broadcast(string Message)
        {
            lock (Player_Lock)
            {
                if (Player_1.Stream != null)
                    Send_Message(Player_1.Stream, Message);
                if (Player_2.Stream != null)
                    Send_Message(Player_2.Stream, Message);
            }
        }

        // Phát sóng trạng thái sẵn sàng (Ready Status) của cả hai người chơi
        private void Broadcast_Ready_Status()
        {
            lock (Player_Lock)
            {
                var Status = new
                {
                    action = "update_ready_status",
                    player1_ready = Player_1.Is_Ready,
                    player1_name = Player_1.Username ?? "Waiting...",
                    player2_ready = Player_2.Is_Ready,
                    player2_name = Player_2.Username ?? "Waiting..."
                };
                Broadcast(JsonSerializer.Serialize(Status));
            }
        }

        // Kiểm tra xem cả hai người chơi đã sẵn sàng chưa để bắt đầu đếm ngược
        private void Check_Start_Countdown()
        {
            lock (Player_Lock)
            {
                if (Player_1.Is_Ready && Player_2.Is_Ready && Countdown_Timer == null)
                {
                    Log_Game_Room("Cả hai đã sẵn sàng. Bắt đầu đếm ngược 5s...");
                    Countdown_Value = 5;
                    Countdown_Timer = new System.Threading.Timer(Countdown_Tick, null, 1000, 1000);
                }
            }
        }

        // Xử lý mỗi tick của bộ đếm ngược
        private void Countdown_Tick(object State)
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

                Broadcast(JsonSerializer.Serialize(new { action = "start_game" }));
                Log_Game_Room("Bắt đầu game!");

                Initialize_Game();
                Server_Game_Loop = new System.Threading.Timer(Server_Game_Loop_Tick, null, 0, Tick_Rate);

                Game_Time_Remaining = 60;
                Game_Timer = new System.Threading.Timer(Game_Timer_Tick, null, 1000, 1000);
            }
        }

        // Xử lý mỗi giây của Game Timer
        private void Game_Timer_Tick(object State)
        {
            if (Game_Time_Remaining > 0)
            {
                Broadcast(JsonSerializer.Serialize(new { action = "update_time", time = Game_Time_Remaining }));
                try
                {
                    lock (Player_Lock)
                    {
                        // Cộng điểm dựa trên tốc độ hiện tại
                        P1_Score += P1_Speed;
                        P2_Score += P2_Speed;
                    }

                    // Gửi điểm số mới cho clients
                    var scoreUpdate = new
                    {
                        action = "update_score",
                        p1_score = P1_Score,
                        p2_score = P2_Score
                    };
                    Broadcast(JsonSerializer.Serialize(scoreUpdate));
                }
                catch (Exception ex)
                {
                    Log_Game_Room($"Lỗi khi cập nhật điểm: {ex.Message}");
                }

                Game_Time_Remaining--;
            }
            else
            {
                Game_Timer?.Dispose();
                Game_Timer = null;

                Server_Game_Loop?.Dispose();
                Server_Game_Loop = null;

                // Lưu điểm số khi game kết thúc
                SaveGameScores();

                Broadcast(JsonSerializer.Serialize(new { action = "game_over" }));
                Log_Game_Room("Hết giờ! Trò chơi kết thúc.");

                lock (Player_Lock)
                {
                    if (Player_1 != null) Player_1.Is_Ready = false;
                    if (Player_2 != null) Player_2.Is_Ready = false;
                }
                Broadcast_Ready_Status();
            }
        }

        // Lưu điểm số vào database khi game kết thúc
        private void SaveGameScores()
        {
            try
            {
                lock (Player_Lock)
                {
                    // Xác định người thắng cuộc
                    int p1WinCount = P1_Score > P2_Score ? 1 : 0;
                    int p2WinCount = P2_Score > P1_Score ? 1 : 0;

                    // Tính toán số lần va chạm (giả định dựa trên tốc độ giảm)
                    int p1CrashCount = Math.Max(0, (100 - P1_Speed) / 10);
                    int p2CrashCount = Math.Max(0, (100 - P2_Speed) / 10);

                    // Lưu điểm cho Player 1
                    if (!string.IsNullOrEmpty(Player_1.Username))
                    {
                        SQL_Helper.AddScore(Player_1.Username, p1WinCount, p1CrashCount, P1_Score);
                        Log_Game_Room($"Đã lưu điểm cho {Player_1.Username}: {P1_Score} điểm");
                    }

                    // Lưu điểm cho Player 2
                    if (!string.IsNullOrEmpty(Player_2.Username))
                    {
                        SQL_Helper.AddScore(Player_2.Username, p2WinCount, p2CrashCount, P2_Score);
                        Log_Game_Room($"Đã lưu điểm cho {Player_2.Username}: {P2_Score} điểm");
                    }
                }
            }
            catch (Exception ex)
            {
                Log_Game_Room($"Lỗi khi lưu điểm số: {ex.Message}");
            }
        }

        // Khởi tạo/Thiết lập lại vị trí và trạng thái ban đầu của các đối tượng game
        private void Initialize_Game()
        {
            lock (Player_Lock)
            {
                P1_Score = 0;
                P2_Score = 0;
                P1_Speed = 10;
                P2_Speed = 10;

                P1_Left = P1_Right = P2_Left = P2_Right = false;

                Game_Objects.Clear();
                Object_Sizes.Clear();

                // --- CẤU HÌNH KÍCH THƯỚC ĐƯỜNG ĐUA ---
                int Road_Width = 500;
                int Safe_Margin = 50;
                int Min_X = Safe_Margin;
                int Max_X = Road_Width - Safe_Margin;

                // --- KHỞI TẠO PLAYER ---
                Object_Sizes["ptb_player1"] = new Size(72, 117);
                Game_Objects["ptb_player1"] = new Point(202, 470);

                Object_Sizes["ptb_player2"] = new Size(72, 117);
                Game_Objects["ptb_player2"] = new Point(202, 470);

                // --- KHỞI TẠO ĐƯỜNG ĐUA (ROAD) ---
                Object_Sizes["ptb_roadtrack1"] = new Size(617, 734);
                Game_Objects["ptb_roadtrack1"] = new Point(0, -2);

                Object_Sizes["ptb_roadtrack1dup"] = new Size(617, 734);
                Game_Objects["ptb_roadtrack1dup"] = new Point(0, 734);

                Object_Sizes["ptb_roadtrack2"] = new Size(458, 596);
                Game_Objects["ptb_roadtrack2"] = new Point(0, 2);

                Object_Sizes["ptb_roadtrack2dup"] = new Size(458, 596);
                Game_Objects["ptb_roadtrack2dup"] = new Point(0, 596);

                // --- KHỞI TẠO AI CAR & ITEM ---
                Object_Sizes["ptb_AICar1"] = new Size(50, 100);
                Game_Objects["ptb_AICar1"] = Reposition_Object("ptb_AICar1", Min_X, Max_X);

                Object_Sizes["ptb_AICar3"] = new Size(74, 128);
                Game_Objects["ptb_AICar3"] = Reposition_Object("ptb_AICar3", Min_X, Max_X);

                Object_Sizes["ptb_AICar5"] = new Size(50, 100);
                Game_Objects["ptb_AICar5"] = Reposition_Object("ptb_AICar5", Min_X, Max_X);

                Object_Sizes["ptb_AICar6"] = new Size(74, 128);
                Game_Objects["ptb_AICar6"] = Reposition_Object("ptb_AICar6", Min_X, Max_X);

                // Buffs/Debuffs
                Object_Sizes["ptb_increasingroad1"] = new Size(30, 30);
                Game_Objects["ptb_increasingroad1"] = Reposition_Object("ptb_increasingroad1", Min_X, Max_X);

                Object_Sizes["ptb_decreasingroad1"] = new Size(30, 30);
                Game_Objects["ptb_decreasingroad1"] = Reposition_Object("ptb_decreasingroad1", Min_X, Max_X);

                Object_Sizes["ptb_increasingroad2"] = new Size(30, 30);
                Game_Objects["ptb_increasingroad2"] = Reposition_Object("ptb_increasingroad2", Min_X, Max_X);

                Object_Sizes["ptb_decreasingroad2"] = new Size(30, 30);
                Game_Objects["ptb_decreasingroad2"] = Reposition_Object("ptb_decreasingroad2", Min_X, Max_X);
            }
        }

        // Thiết lập lại vị trí ngẫu nhiên cho một đối tượng (AI car hoặc power-up) ở ngoài màn hình
        private Point Reposition_Object(string Name, int Min_X, int Max_X)
        {
            Size Current_Size = Object_Sizes.ContainsKey(Name) ? Object_Sizes[Name] : new Size(30, 30);
            int Safe_Max_X = Max_X - Current_Size.Width - 60;

            if (Safe_Max_X <= Min_X) Safe_Max_X = Min_X + 1;

            int Max_Retries = 20;
            int Attempt = 0;
            Point New_Pos = new Point(0, 0);
            bool Overlap;

            do
            {
                Overlap = false;
                int Random_X = Rand.Next(Min_X, Safe_Max_X);
                int Random_Y = Rand.Next(-1000, -150);
                New_Pos = new Point(Random_X, Random_Y);
                Rectangle New_Rect = new Rectangle(New_Pos, Current_Size);
                New_Rect.Inflate(30, 150);

                foreach (var Key in Game_Objects.Keys)
                {
                    if (Key == Name || Key.Contains("roadtrack") || Key.Contains("player"))
                        continue;

                    if (Object_Sizes.ContainsKey(Key))
                    {
                        Rectangle Existing_Rect = new Rectangle(Game_Objects[Key], Object_Sizes[Key]);
                        if (New_Rect.IntersectsWith(Existing_Rect))
                        {
                            Overlap = true;
                            break;
                        }
                    }
                }
                Attempt++;
            } while (Overlap && Attempt < Max_Retries);

            if (Overlap)
            {
                New_Pos.Y -= 300;
            }

            if (Game_Objects.ContainsKey(Name))
                Game_Objects[Name] = New_Pos;

            return New_Pos;
        }

        // Vòng lặp chính của Game Server (Game Tick) - Xử lý logic di chuyển, va chạm và gửi trạng thái
        private void Server_Game_Loop_Tick(object State)
        {
            try
            {
                lock (Player_Lock)
                {
                    if (Game_Timer == null) return;

                    Point P1_Pos = Game_Objects["ptb_player1"];
                    if (P1_Left && P1_Pos.X > P1_Min_X) P1_Pos.X -= Player_Move_Speed;
                    if (P1_Right && P1_Pos.X < P1_Max_X - Object_Sizes["ptb_player1"].Width) P1_Pos.X += Player_Move_Speed;
                    Game_Objects["ptb_player1"] = P1_Pos;

                    Point P2_Pos = Game_Objects["ptb_player2"];
                    if (P2_Left && P2_Pos.X > P2_Min_X) P2_Pos.X -= Player_Move_Speed;
                    if (P2_Right && P2_Pos.X < P2_Max_X - Object_Sizes["ptb_player2"].Width) P2_Pos.X += Player_Move_Speed;
                    Game_Objects["ptb_player2"] = P2_Pos;

                    Move_Object_Down("ptb_roadtrack1", P1_Speed, Game_Height, true);
                    Move_Object_Down("ptb_roadtrack1dup", P1_Speed, Game_Height, true);
                    Move_Object_Down("ptb_AICar1", P1_Speed, Game_Height, false, P1_Min_X, P1_Max_X);
                    Move_Object_Down("ptb_AICar5", P1_Speed, Game_Height, false, P1_Min_X, P1_Max_X);
                    Move_Object_Down("ptb_increasingroad1", P1_Speed, Game_Height, false, P1_Min_X, P1_Max_X);
                    Move_Object_Down("ptb_decreasingroad1", P1_Speed, Game_Height, false, P1_Min_X, P1_Max_X);

                    Move_Object_Down("ptb_roadtrack2", P2_Speed, Game_Height, true);
                    Move_Object_Down("ptb_roadtrack2dup", P2_Speed, Game_Height, true);
                    Move_Object_Down("ptb_AICar3", P2_Speed, Game_Height, false, P2_Min_X, P2_Max_X);
                    Move_Object_Down("ptb_AICar6", P2_Speed, Game_Height, false, P2_Min_X, P2_Max_X);
                    Move_Object_Down("ptb_increasingroad2", P2_Speed, Game_Height, false, P2_Min_X, P2_Max_X);
                    Move_Object_Down("ptb_decreasingroad2", P2_Speed, Game_Height, false, P2_Min_X, P2_Max_X);

                    // Check Collisions Player 1
                    if (Check_Collision("ptb_player1", "ptb_increasingroad1")) { P1_Speed += 3; Reposition_Object("ptb_increasingroad1", P1_Min_X, P1_Max_X); Send_Message(Player_1.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "buff" })); }
                    if (Check_Collision("ptb_player1", "ptb_decreasingroad1")) { P1_Speed -= 3; Reposition_Object("ptb_decreasingroad1", P1_Min_X, P1_Max_X); Send_Message(Player_1.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "debuff" })); }
                    if (Check_Collision("ptb_player1", "ptb_AICar1")) { P1_Speed -= 4; Reposition_Object("ptb_AICar1", P1_Min_X, P1_Max_X); Send_Message(Player_2.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "hit_car" })); }
                    if (Check_Collision("ptb_player1", "ptb_AICar5")) { P1_Speed -= 4; Reposition_Object("ptb_AICar5", P1_Min_X, P1_Max_X); Send_Message(Player_2.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "hit_car" })); }

                    // Check Collisions Player 2
                    if (Check_Collision("ptb_player2", "ptb_increasingroad2")) { P2_Speed += 3; Reposition_Object("ptb_increasingroad2", P2_Min_X, P2_Max_X); Send_Message(Player_1.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "buff" })); }
                    if (Check_Collision("ptb_player2", "ptb_decreasingroad2")) { P2_Speed -= 3; Reposition_Object("ptb_decreasingroad2", P2_Min_X, P2_Max_X); Send_Message(Player_1.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "debuff" })); }
                    if (Check_Collision("ptb_player2", "ptb_AICar3")) { P2_Speed -= 4; Reposition_Object("ptb_AICar3", P2_Min_X, P2_Max_X); Send_Message(Player_2.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "hit_car" })); }
                    if (Check_Collision("ptb_player2", "ptb_AICar6")) { P2_Speed -= 4; Reposition_Object("ptb_AICar6", P2_Min_X, P2_Max_X); Send_Message(Player_2.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "hit_car" })); }

                    int Min_Speed = 4;
                    P1_Speed = Math.Max(P1_Speed, Min_Speed);
                    P2_Speed = Math.Max(P2_Speed, Min_Speed);

                    var Game_State = new Dictionary<string, object>
            {
                { "action", "update_game_state" }
            };

                    foreach (var Pair in Game_Objects)
                    {
                        Game_State[Pair.Key] = Pair.Value;
                    }

                    Broadcast(JsonSerializer.Serialize(Game_State));
                }
            }
            catch (Exception Ex)
            {
                Log_Game_Room($"LỖI GAME LOOP SERVER: {Ex.Message}");
            }
        }

        // Di chuyển một đối tượng xuống dưới và thiết lập lại vị trí nếu nó đi ra khỏi màn hình
        private void Move_Object_Down(string Name, int Speed, int Screen_Height, bool Is_Road, int Min_X = 0, int Max_X = 0)
        {
            if (!Game_Objects.ContainsKey(Name)) return;

            Point Pos = Game_Objects[Name];
            Pos.Y += Speed;

            if (Pos.Y > Screen_Height)
            {
                if (Is_Road)
                {
                    string Dup_Name = (Name == "ptb_roadtrack1") ? "ptb_roadtrack1dup" :
                                      (Name == "ptb_roadtrack1dup") ? "ptb_roadtrack1" :
                                      (Name == "ptb_roadtrack2") ? "ptb_roadtrack2dup" : "ptb_roadtrack2";
                    Pos.Y = Game_Objects[Dup_Name].Y - Screen_Height;
                }
                else
                {
                    Pos = Reposition_Object(Name, Min_X, Max_X);
                }
            }
            Game_Objects[Name] = Pos;
        }

        // Kiểm tra va chạm giữa hai đối tượng game
        private bool Check_Collision(string Player, string Obj)
        {
            if (!Game_Objects.ContainsKey(Player) || !Game_Objects.ContainsKey(Obj)) return false;

            Rectangle Rect_Player = new Rectangle(Game_Objects[Player], Object_Sizes[Player]);
            Rectangle Rect_Obj = new Rectangle(Game_Objects[Obj], Object_Sizes[Obj]);
            return Rect_Player.IntersectsWith(Rect_Obj);
        }
    }
}