using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Pixel_Drift_Server
{
    public partial class ServerForm : Form
    {
        private TcpListener TCP_Server;
        private Player_Slot Player_1 = new Player_Slot();
        private Player_Slot Player_2 = new Player_Slot();
        private readonly object Player_Lock = new object();

        private System.Threading.Timer Countdown_Timer;
        private int Countdown_Value = 5;
        private System.Threading.Timer Game_Timer;
        private int Game_Time_Remaining = 60;

        private System.Threading.Timer Server_Game_Loop;
        private const int Tick_Rate = 18;
        private const int Player_Move_Speed = 9;

        // Lưu trữ vị trí và kích thước đối tượng game
        private Dictionary<string, Point> Game_Objects = new Dictionary<string, Point>();
        private Dictionary<string, Size> Object_Sizes = new Dictionary<string, Size>();
        private int P1_Speed = 10;
        private int P2_Speed = 10;
        private long P1_Score = 0;
        private long P2_Score = 0;
        private bool P1_Left, P1_Right, P2_Left, P2_Right;

        private const int Game_Height = 800;
        private const int P1_Min_X = -10;
        private const int P1_Max_X = 530;
        private const int P2_Min_X = -10;
        private const int P2_Max_X = 530;

        private Random Server_Rand = new Random();

        // CHỐNG LOGIN TRÙNG - SỬA: Dùng email làm key thay vì username
        private Dictionary<string, DateTime> Logged_In_Users = new Dictionary<string, DateTime>();
        private Dictionary<string, TcpClient> Active_Connections = new Dictionary<string, TcpClient>();
        private readonly object Login_Lock = new object();
        private readonly object Db_Lock = new object();

        // QUẢN LÝ TOKEN RESET PASSWORD
        private Dictionary<string, string> PasswordResetTokens = new Dictionary<string, string>();
        private readonly object PasswordResetLock = new object();

        public class Player_Slot
        {
            public TcpClient Client { get; set; }
            public NetworkStream Stream { get; set; }
            public string Username { get; set; }
            public bool Is_Ready { get; set; } = false;
        }

        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            try
            {
                SQL_Helper.Initialize();
                Log("SQLite đã sẵn sàng!");
            }
            catch (Exception Ex)
            {
                Log("Lỗi SQLite: " + Ex.Message);
            }
        }

        private void btn_Start_Server_Click(object sender, EventArgs e)
        {
            Thread Server_Thread = new Thread(Start_Server);
            Server_Thread.IsBackground = true;
            Server_Thread.Start();
            btn_Start_Server.Enabled = false;
        }

        private void Log(string Message)
        {
            if (tb_hienthi.InvokeRequired)
            {
                tb_hienthi.Invoke(new Action(() => tb_hienthi.AppendText(Message + Environment.NewLine)));
            }
            else
            {
                tb_hienthi.AppendText(Message + Environment.NewLine);
            }
        }

        private void Start_Server()
        {
            try
            {
                TCP_Server = new TcpListener(IPAddress.Any, 1111);
                TCP_Server.Start();
                Log("Server is listening on port 1111...");

                while (true)
                {
                    TcpClient TCP_Client = TCP_Server.AcceptTcpClient();
                    Log($"Client {TCP_Client.Client.RemoteEndPoint} đã kết nối!");

                    Thread Client_Thread = new Thread(() => Handle_Client(TCP_Client));
                    Client_Thread.IsBackground = true;
                    Client_Thread.Start();
                }
            }
            catch (Exception Ex)
            {
                Log($"Lỗi máy chủ: {Ex.Message}");
            }
        }

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
                Log($"Lỗi khi gửi tin nhắn: {Ex.Message}");
            }
        }

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

        private void Check_Start_Countdown()
        {
            lock (Player_Lock)
            {
                if (Player_1.Is_Ready && Player_2.Is_Ready && Countdown_Timer == null)
                {
                    Log("Cả hai đã sẵn sàng. Bắt đầu đếm ngược 5s...");
                    Countdown_Value = 5;
                    Countdown_Timer = new System.Threading.Timer(Countdown_Tick, null, 1000, 1000);
                }
            }
        }

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
                Log("Bắt đầu game!");

                Initialize_Game();
                Server_Game_Loop = new System.Threading.Timer(Server_Game_Loop_Tick, null, 0, Tick_Rate);

                Game_Time_Remaining = 60;
                Game_Timer = new System.Threading.Timer(Game_Timer_Tick, null, 1000, 1000);
            }
        }

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
                    Log($"Lỗi khi cập nhật điểm: {ex.Message}");
                }

                Game_Time_Remaining--;
            }
            else
            {
                Game_Timer?.Dispose();
                Game_Timer = null;

                Server_Game_Loop?.Dispose();
                Server_Game_Loop = null;

                // Lưu điểm số khi game kết thúc (TÍNH NĂNG SCOREBOARD)
                SaveGameScores();

                Broadcast(JsonSerializer.Serialize(new { action = "game_over" }));
                Log("Hết giờ! Trò chơi kết thúc.");

                lock (Player_Lock)
                {
                    if (Player_1 != null) Player_1.Is_Ready = false;
                    if (Player_2 != null) Player_2.Is_Ready = false;
                }
                Broadcast_Ready_Status();
            }
        }

        // TÍNH NĂNG SCOREBOARD
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
                        Log($"Đã lưu điểm cho {Player_1.Username}: {P1_Score} điểm");
                    }

                    // Lưu điểm cho Player 2
                    if (!string.IsNullOrEmpty(Player_2.Username))
                    {
                        SQL_Helper.AddScore(Player_2.Username, p2WinCount, p2CrashCount, P2_Score);
                        Log($"Đã lưu điểm cho {Player_2.Username}: {P2_Score} điểm");
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Lỗi khi lưu điểm số: {ex.Message}");
            }
        }

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

                // SỬ DỤNG KÍCH THƯỚC CHUẨN
                int Road_Width = 500;
                int Safe_Margin = 50;
                int Min_X = Safe_Margin;
                int Max_X = Road_Width - Safe_Margin;

                // KHỞI TẠO PLAYER - kích thước chuẩn
                Object_Sizes["ptb_player1"] = new Size(72, 117);
                Game_Objects["ptb_player1"] = new Point(202, 470);

                Object_Sizes["ptb_player2"] = new Size(72, 117);
                Game_Objects["ptb_player2"] = new Point(202, 470);

                // KHỞI TẠO ĐƯỜNG ĐUA - kích thước chuẩn
                Object_Sizes["ptb_roadtrack1"] = new Size(617, 734);
                Game_Objects["ptb_roadtrack1"] = new Point(0, -2);

                Object_Sizes["ptb_roadtrack1dup"] = new Size(617, 734);
                Game_Objects["ptb_roadtrack1dup"] = new Point(0, 734);

                Object_Sizes["ptb_roadtrack2"] = new Size(458, 596);
                Game_Objects["ptb_roadtrack2"] = new Point(0, 2);

                Object_Sizes["ptb_roadtrack2dup"] = new Size(458, 596);
                Game_Objects["ptb_roadtrack2dup"] = new Point(0, 596);

                // KHỞI TẠO AI CAR & ITEM - kích thước chuẩn
                Object_Sizes["ptb_AICar1"] = new Size(50, 100);
                Game_Objects["ptb_AICar1"] = Reposition_Object("ptb_AICar1", Min_X, Max_X);

                Object_Sizes["ptb_AICar3"] = new Size(74, 128);
                Game_Objects["ptb_AICar3"] = Reposition_Object("ptb_AICar3", Min_X, Max_X);

                Object_Sizes["ptb_AICar5"] = new Size(50, 100);
                Game_Objects["ptb_AICar5"] = Reposition_Object("ptb_AICar5", Min_X, Max_X);

                Object_Sizes["ptb_AICar6"] = new Size(74, 128);
                Game_Objects["ptb_AICar6"] = Reposition_Object("ptb_AICar6", Min_X, Max_X);

                // Buffs/Debuffs - kích thước chuẩn
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
                int Random_X = Server_Rand.Next(Min_X, Safe_Max_X);
                int Random_Y = Server_Rand.Next(-1000, -150);
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

        private void Server_Game_Loop_Tick(object State)
        {
            try
            {
                lock (Player_Lock)
                {
                    if (Game_Timer == null) return;

                    // DI CHUYỂN PLAYER
                    Point P1_Pos = Game_Objects["ptb_player1"];
                    if (P1_Left && P1_Pos.X > P1_Min_X) P1_Pos.X -= Player_Move_Speed;
                    if (P1_Right && P1_Pos.X < P1_Max_X - Object_Sizes["ptb_player1"].Width) P1_Pos.X += Player_Move_Speed;
                    Game_Objects["ptb_player1"] = P1_Pos;

                    Point P2_Pos = Game_Objects["ptb_player2"];
                    if (P2_Left && P2_Pos.X > P2_Min_X) P2_Pos.X -= Player_Move_Speed;
                    if (P2_Right && P2_Pos.X < P2_Max_X - Object_Sizes["ptb_player2"].Width) P2_Pos.X += Player_Move_Speed;
                    Game_Objects["ptb_player2"] = P2_Pos;

                    // DI CHUYỂN OBJECTS 
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

                    // CHECK COLLISIONS 
                    if (Check_Collision("ptb_player1", "ptb_increasingroad1"))
                    {
                        P1_Speed += 3;
                        Reposition_Object("ptb_increasingroad1", P1_Min_X, P1_Max_X);
                        Send_Message(Player_1.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "buff" }));
                    }
                    if (Check_Collision("ptb_player1", "ptb_decreasingroad1"))
                    {
                        P1_Speed -= 3;
                        Reposition_Object("ptb_decreasingroad1", P1_Min_X, P1_Max_X);
                        Send_Message(Player_1.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "debuff" }));
                    }
                    if (Check_Collision("ptb_player1", "ptb_AICar1"))
                    {
                        P1_Speed -= 4;
                        Reposition_Object("ptb_AICar1", P1_Min_X, P1_Max_X);
                        Send_Message(Player_1.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "hit_car" }));
                    }
                    if (Check_Collision("ptb_player1", "ptb_AICar5"))
                    {
                        P1_Speed -= 4;
                        Reposition_Object("ptb_AICar5", P1_Min_X, P1_Max_X);
                        Send_Message(Player_1.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "hit_car" }));
                    }

                    if (Check_Collision("ptb_player2", "ptb_increasingroad2"))
                    {
                        P2_Speed += 3;
                        Reposition_Object("ptb_increasingroad2", P2_Min_X, P2_Max_X);
                        Send_Message(Player_2.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "buff" }));
                    }
                    if (Check_Collision("ptb_player2", "ptb_decreasingroad2"))
                    {
                        P2_Speed -= 3;
                        Reposition_Object("ptb_decreasingroad2", P2_Min_X, P2_Max_X);
                        Send_Message(Player_2.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "debuff" }));
                    }
                    if (Check_Collision("ptb_player2", "ptb_AICar3"))
                    {
                        P2_Speed -= 4;
                        Reposition_Object("ptb_AICar3", P2_Min_X, P2_Max_X);
                        Send_Message(Player_2.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "hit_car" }));
                    }
                    if (Check_Collision("ptb_player2", "ptb_AICar6"))
                    {
                        P2_Speed -= 4;
                        Reposition_Object("ptb_AICar6", P2_Min_X, P2_Max_X);
                        Send_Message(Player_2.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = "hit_car" }));
                    }

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
                Log($"LỖI GAME LOOP SERVER: {Ex.Message}");
            }
        }

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

        private bool Check_Collision(string Player, string Obj)
        {
            if (!Game_Objects.ContainsKey(Player) || !Game_Objects.ContainsKey(Obj)) return false;

            Rectangle Rect_Player = new Rectangle(Game_Objects[Player], Object_Sizes[Player]);
            Rectangle Rect_Obj = new Rectangle(Game_Objects[Obj], Object_Sizes[Obj]);
            return Rect_Player.IntersectsWith(Rect_Obj);
        }

        // XỬ LÝ CLIENT VỚI TẤT CẢ TÍNH NĂNG
        private void Handle_Client(TcpClient Client)
        {
            NetworkStream Stream = Client.GetStream();
            StreamReader Reader = new StreamReader(Stream, Encoding.UTF8);
            Player_Slot Current_Player_Slot = null;
            string Message;

            try
            {
                while ((Message = Reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(Message)) continue;

                    Log($"[{Client.Client.RemoteEndPoint}] gửi: {Message}");

                    string Response = null;
                    try
                    {
                        var Data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Message);
                        if (!Data.ContainsKey("action")) continue;
                        string Action = Data["action"].GetString();

                        switch (Action)
                        {
                            case "login":
                                {
                                    var Login_Data = new Dictionary<string, string>
                                    {
                                        { "username", Data["username"].GetString() },
                                        { "password", Data["password"].GetString() }
                                    };
                                    Response = Handle_Login(Login_Data, Client); // Truyền Client vào
                                    break;
                                }

                            case "register":
                                var Reg_Data = new Dictionary<string, string> {
                                    { "email", Data["email"].GetString() },
                                    { "username", Data["username"].GetString() },
                                    { "password", Data["password"].GetString() },
                                    { "birthday", Data["birthday"].GetString() }
                                };
                                Response = Handle_Register(Reg_Data);
                                break;

                            case "get_info":
                                var Info_Data = new Dictionary<string, string> { { "username", Data["username"].GetString() } };
                                Response = Handle_Get_Info(Info_Data);
                                break;

                            case "forgot_password":
                                var Forgot_Data = new Dictionary<string, string> { { "email", Data["email"].GetString() } };
                                Response = Handle_Forgot_Password(Forgot_Data);
                                break;

                            case "change_password":
                                var Change_Data = new Dictionary<string, string> {
                                    { "email", Data["email"].GetString() },
                                    { "token", Data["token"].GetString() },
                                    { "new_password", Data["new_password"].GetString() }
                                };
                                Response = Handle_Change_Password(Change_Data);
                                break;

                            case "join_lobby":
                                lock (Player_Lock)
                                {
                                    string Username = Data.ContainsKey("username") ? Data["username"].GetString() : "Player";

                                    // CHỐNG LOGIN TRÙNG: Lấy email từ username để kiểm tra
                                    string userEmail = GetEmailFromUsername(Username);
                                    if (string.IsNullOrEmpty(userEmail))
                                    {
                                        userEmail = Username; // Fallback nếu không tìm thấy email
                                    }

                                    lock (Login_Lock)
                                    {
                                        if (Active_Connections.ContainsKey(userEmail)) // Kiểm tra bằng email
                                        {
                                            TcpClient Old_Client = Active_Connections[userEmail];
                                            Log($"Phát hiện join_lobby trùng: {Username} (Email: {userEmail})");

                                            try
                                            {
                                                if (Old_Client != null && Old_Client.Connected)
                                                {
                                                    NetworkStream Old_Stream = Old_Client.GetStream();
                                                    string kickMsg = JsonSerializer.Serialize(new
                                                    {
                                                        action = "force_logout",
                                                        Message = "Tài khoản của bạn đã được đăng nhập từ nơi khác."
                                                    });
                                                    Send_Message(Old_Stream, kickMsg);
                                                    System.Threading.Thread.Sleep(200);
                                                    Old_Stream.Close();
                                                    Old_Client.Close();
                                                    Log($"Đã kick kết nối cũ của {Username} (Email: {userEmail})");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Log($"Lỗi khi kick: {ex.Message}");
                                            }

                                            Active_Connections.Remove(userEmail); // Xóa bằng email
                                        }

                                        Active_Connections[userEmail] = Client; // Lưu bằng email
                                        Log($"Đã lưu kết nối cho {Username} (Email: {userEmail})");
                                    }

                                    if (Player_1.Client == null)
                                    {
                                        Player_1.Client = Client;
                                        Player_1.Stream = Stream;
                                        Player_1.Username = Username;
                                        Current_Player_Slot = Player_1;
                                        Response = JsonSerializer.Serialize(new { action = "assign_player", player_number = 1 });
                                        Log($"Player 1 ({Player_1.Username}) đã kết nối.");
                                    }
                                    else if (Player_2.Client == null)
                                    {
                                        Player_2.Client = Client;
                                        Player_2.Stream = Stream;
                                        Player_2.Username = Username;
                                        Current_Player_Slot = Player_2;
                                        Response = JsonSerializer.Serialize(new { action = "assign_player", player_number = 2 });
                                        Log($"Player 2 ({Player_2.Username}) đã kết nối.");
                                    }
                                    else
                                    {
                                        Response = JsonSerializer.Serialize(new { action = "lobby_full" });
                                    }
                                }
                                Broadcast_Ready_Status();
                                break;

                            case "set_ready":
                                bool Is_Ready = Data["ready_status"].GetString() == "true";
                                if (Current_Player_Slot != null)
                                {
                                    Current_Player_Slot.Is_Ready = Is_Ready;
                                    Log($"Player {(Current_Player_Slot == Player_1 ? 1 : 2)} {(Is_Ready ? "Sẵn sàng" : "Chưa")}.");
                                    Broadcast_Ready_Status();
                                    Check_Start_Countdown();
                                }
                                break;

                            case "move":
                                int Player = Data["player"].GetInt32();
                                string Direction = Data["direction"].GetString();
                                string State = Data["state"].GetString();
                                bool Is_Pressed = (State == "down");

                                lock (Player_Lock)
                                {
                                    if (Player == 1)
                                    {
                                        if (Direction == "left") P1_Left = Is_Pressed;
                                        else if (Direction == "right") P1_Right = Is_Pressed;
                                    }
                                    else if (Player == 2)
                                    {
                                        if (Direction == "left") P2_Left = Is_Pressed;
                                        else if (Direction == "right") P2_Right = Is_Pressed;
                                    }
                                }
                                break;

                            // TÍNH NĂNG SCOREBOARD
                            case "get_scoreboard":
                                int TopCount = Data.ContainsKey("top_count") ? Data["top_count"].GetInt32() : 50;
                                string Scoreboard_Data = SQL_Helper.GetTopScores(TopCount);
                                Response = JsonSerializer.Serialize(new
                                {
                                    action = "scoreboard_data",
                                    data = Scoreboard_Data
                                });
                                break;

                            case "search_player":
                                string Search_Text = Data["search_text"].GetString();
                                string Search_Result = SQL_Helper.SearchPlayer(Search_Text);
                                Response = JsonSerializer.Serialize(new
                                {
                                    action = "search_result",
                                    data = Search_Result
                                });
                                break;

                            case "add_score":
                                string PlayerName = Data["player_name"].GetString();
                                int WinCount = Data["win_count"].GetInt32();
                                int CrashCount = Data["crash_count"].GetInt32();
                                double TotalScore = Data["total_score"].GetDouble();

                                bool Success = SQL_Helper.AddScore(PlayerName, WinCount, CrashCount, TotalScore);
                                Response = JsonSerializer.Serialize(new
                                {
                                    action = "add_score_result",
                                    success = Success
                                });
                                break;

                            default:
                                Response = JsonSerializer.Serialize(new { Status = "error", Message = "Unknown action" });
                                break;
                        }

                        if (Response != null)
                        {
                            Send_Message(Stream, Response);
                        }
                    }
                    catch (JsonException Ex) { Log($" Lỗi JSON: {Ex.Message}. Data: {Message}"); }
                    catch (Exception Ex) { Log($" Lỗi xử lý client: {Ex.Message}. Data: {Message}"); }
                }
            }
            catch (Exception Ex)
            {
                Log($"Lỗi Client: {Ex.Message}");
            }
            finally
            {
                try
                {
                    Log($"Client {Client.Client.RemoteEndPoint} đã ngắt kết nối.");
                }
                catch { }
                lock (Login_Lock)
                {
                    string Email_To_Remove = null;
                    foreach (var kvp in Active_Connections)
                    {
                        if (kvp.Value == Client)
                        {
                            Email_To_Remove = kvp.Key;
                            break;
                        }
                    }
                    if (Email_To_Remove != null)
                    {
                        Active_Connections.Remove(Email_To_Remove);
                        Log($"Đã xóa {Email_To_Remove} khỏi Active_Connections.");
                    }
                }
                try
                {
                    lock (Player_Lock)
                    {
                        string Disconnected_Player_Name = "Unknown";
                        if (Current_Player_Slot == Player_1)
                        {
                            Disconnected_Player_Name = Player_1.Username;
                            Player_1 = new Player_Slot();
                        }
                        else if (Current_Player_Slot == Player_2)
                        {
                            Disconnected_Player_Name = Player_2.Username;
                            Player_2 = new Player_Slot();
                        }
                        try { Countdown_Timer?.Dispose(); } catch { }
                        try { Game_Timer?.Dispose(); } catch { }
                        try { Server_Game_Loop?.Dispose(); } catch { }

                        Countdown_Timer = null;
                        Game_Timer = null;
                        Server_Game_Loop = null;

                        try
                        {
                            Broadcast(JsonSerializer.Serialize(new { action = "player_disconnected", Name = Disconnected_Player_Name }));
                            Broadcast_Ready_Status();
                        }
                        catch (Exception ex)
                        {
                            Log($"Lỗi khi broadcast disconnect: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log($"Lỗi trong player cleanup: {ex.Message}");
                }

                try { Reader?.Close(); } catch { }
                try { Client?.Close(); } catch { }
            }
        }

        // Lấy email từ username
        private string GetEmailFromUsername(string username)
        {
            lock (Db_Lock)
            {
                try
                {
                    string query = "SELECT Email FROM Info_User WHERE Username=@username";
                    using (var cmd = new SQLiteCommand(query, SQL_Helper.Connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        var result = cmd.ExecuteScalar();
                        return result?.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Log($"Lỗi khi lấy email từ username {username}: {ex.Message}");
                    return null;
                }
            }
        }

        // CÁC PHƯƠNG THỨC AUTHENTICATION 
        private string Handle_Login(Dictionary<string, string> data, TcpClient client)
        {
            try
            {
                string username = data["username"];
                string password = data["password"];

                // Lấy email từ username trước khi kiểm tra
                string userEmail = GetEmailFromUsername(username);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return JsonSerializer.Serialize(new
                    {
                        Status = "error",
                        Message = "Tài khoản không tồn tại"
                    });
                }

                // CHỐNG LOGIN TRÙNG - Dùng email thay vì username
                lock (Login_Lock)
                {
                    if (Active_Connections.ContainsKey(userEmail))
                    {
                        TcpClient Old_Client = Active_Connections[userEmail];
                        Log($"Kick user đang chơi: {username} (Email: {userEmail})");
                        try
                        {
                            if (Old_Client != null && Old_Client.Connected)
                            {
                                NetworkStream Old_Stream = Old_Client.GetStream();
                                string kickMsg = JsonSerializer.Serialize(new
                                {
                                    action = "force_logout",
                                    Message = "Tài khoản của bạn đã được đăng nhập từ nơi khác."
                                });
                                Send_Message(Old_Stream, kickMsg);
                                System.Threading.Thread.Sleep(200);
                                try { Old_Stream?.Close(); } catch { }
                                try { Old_Client?.Close(); } catch { }

                                Log($"Đã kick {username} (Email: {userEmail}) khỏi game");
                            }
                        }
                        catch (ObjectDisposedException)
                        {
                            Log($"Client {username} đã bị disposed - bỏ qua");
                        }
                        catch (IOException ioEx)
                        {
                            Log($"Lỗi IO khi kick {username}: {ioEx.Message}");
                        }
                        catch (Exception ex)
                        {
                            Log($"Lỗi khi kick {username}: {ex.Message}");
                        }
                        finally
                        {
                            // đảm bảo luôn remote ACTIVE_CONNECTIONS
                            Active_Connections.Remove(userEmail);
                        }

                        // Xóa khỏi Player slot nếu có
                        lock (Player_Lock)
                        {
                            if (Player_1.Username != null && Player_1.Username.Contains(username))
                            {
                                Player_1 = new Player_Slot();
                            }
                            if (Player_2.Username != null && Player_2.Username.Contains(username))
                            {
                                Player_2 = new Player_Slot();
                            }
                        }
                    }

                    // Kiểm tra trong database
                    long count = 0;
                    lock (Db_Lock)
                    {
                        string query = "SELECT COUNT(*) FROM Info_User WHERE Username=@username AND Password=@password";
                        using (var cmd = new SQLiteCommand(query, SQL_Helper.Connection))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.Parameters.AddWithValue("@password", password);
                            count = (long)cmd.ExecuteScalar();
                        }
                    }

                    if (count > 0)
                    {
                        // Lưu kết nối mới bằng email
                        Active_Connections[userEmail] = client;
                        Logged_In_Users[username] = DateTime.Now;

                        return JsonSerializer.Serialize(new
                        {
                            Status = "success",
                            Message = "Đăng nhập thành công",
                            username = username
                        });
                    }
                    else
                    {
                        return JsonSerializer.Serialize(new
                        {
                            Status = "error",
                            Message = "Sai tên đăng nhập hoặc mật khẩu"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    Status = "error",
                    Message = $"Lỗi đăng nhập: {ex.Message}"
                });
            }
        }

        // Các phương thức khác giữ nguyên
        private string Handle_Register(Dictionary<string, string> data)
        {
            try
            {
                string email = data["email"];
                string username = data["username"];
                string password = data["password"];
                string birthday = data["birthday"];

                // Kiểm tra xem email đã tồn tại chưa
                string checkEmailQuery = "SELECT COUNT(*) FROM Info_User WHERE Email=@email";
                using (var checkCmd = new SQLiteCommand(checkEmailQuery, SQL_Helper.Connection))
                {
                    checkCmd.Parameters.AddWithValue("@email", email);
                    long emailCount = (long)checkCmd.ExecuteScalar();
                    if (emailCount > 0)
                    {
                        return JsonSerializer.Serialize(new
                        {
                            Status = "error",
                            Message = "Email đã tồn tạ"
                        });
                    }
                }

                // Kiểm tra xem username đã tồn tại chưa
                string checkUserQuery = "SELECT COUNT(*) FROM Info_User WHERE Username=@username";
                using (var checkCmd = new SQLiteCommand(checkUserQuery, SQL_Helper.Connection))
                {
                    checkCmd.Parameters.AddWithValue("@username", username);
                    long userCount = (long)checkCmd.ExecuteScalar();
                    if (userCount > 0)
                    {
                        return JsonSerializer.Serialize(new
                        {
                            Status = "error",
                            Message = "Tên người dùng đã tồn tại"
                        });
                    }
                }

                // Thêm user mới
                string insertQuery = "INSERT INTO Info_User (Username, Email, Password, Birthday) VALUES (@username, @email, @password, @birthday)";
                using (var insertCmd = new SQLiteCommand(insertQuery, SQL_Helper.Connection))
                {
                    insertCmd.Parameters.AddWithValue("@username", username);
                    insertCmd.Parameters.AddWithValue("@email", email);
                    insertCmd.Parameters.AddWithValue("@password", password);
                    insertCmd.Parameters.AddWithValue("@birthday", birthday);

                    int rowsAffected = insertCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return JsonSerializer.Serialize(new
                        {
                            Status = "success",
                            Message = "Đăng ký thành công"
                        });
                    }
                    else
                    {
                        return JsonSerializer.Serialize(new
                        {
                            Status = "error",
                            Message = "Đăng ký thất bại"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    Status = "error",
                    Message = $"Lỗi đăng ký: {ex.Message}"
                });
            }
        }

        private string Handle_Get_Info(Dictionary<string, string> data)
        {
            try
            {
                string username = data["username"];

                string query = "SELECT Username, Email, Birthday FROM Info_User WHERE Username=@username";
                using (var cmd = new SQLiteCommand(query, SQL_Helper.Connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return JsonSerializer.Serialize(new
                            {
                                Status = "success",
                                username = reader["Username"].ToString(),
                                email = reader["Email"].ToString(),
                                birthday = reader["Birthday"].ToString()
                            });
                        }
                        else
                        {
                            return JsonSerializer.Serialize(new
                            {
                                Status = "error",
                                Message = "Không tìm thấy người dùng"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    Status = "error",
                    Message = $"Lỗi lấy thông tin: {ex.Message}"
                });
            }
        }

        private string Handle_Forgot_Password(Dictionary<string, string> data)
        {
            try
            {
                string email = data["email"];

                // Kiểm tra email có tồn tại không
                string query = "SELECT Username, Password FROM Info_User WHERE Email=@email";
                using (var cmd = new SQLiteCommand(query, SQL_Helper.Connection))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string username = reader["Username"].ToString();
                            string password = reader["Password"].ToString();

                            // Tạo token reset password
                            string token = GenerateResetToken();
                            lock (PasswordResetLock)
                            {
                                PasswordResetTokens[email] = token;
                            }

                            // Gửi email với token
                            bool sent = Send_Email(email, "Password Reset",
                                $"Hello {username}, your password reset token is: {token}");

                            if (sent)
                            {
                                return JsonSerializer.Serialize(new
                                {
                                    Status = "success",
                                    Message = "Đã gửi token đặt lại mật khẩu"
                                });
                            }
                            else
                            {
                                return JsonSerializer.Serialize(new
                                {
                                    Status = "error",
                                    Message = "Không thể gửi token"
                                });
                            }
                        }
                        else
                        {
                            return JsonSerializer.Serialize(new
                            {
                                Status = "error",
                                Message = "Email không tồn tại"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    Status = "error",
                    Message = $"Lỗi quên mật khẩu: {ex.Message}"
                });
            }
        }

        private string Handle_Change_Password(Dictionary<string, string> data)
        {
            try
            {
                string email = data["email"];
                string token = data["token"];
                string newPassword = data["new_password"];

                // Kiểm tra token
                lock (PasswordResetLock)
                {
                    if (!PasswordResetTokens.ContainsKey(email) || PasswordResetTokens[email] != token)
                    {
                        return JsonSerializer.Serialize(new
                        {
                            Status = "error",
                            Message = "Mã xác thực không hợp lệ hoặc đã hết hạn"
                        });
                    }

                    // Xóa token sau khi sử dụng
                    PasswordResetTokens.Remove(email);
                }

                // Cập nhật mật khẩu
                string updateQuery = "UPDATE Info_User SET Password=@password WHERE Email=@email";
                using (var updateCmd = new SQLiteCommand(updateQuery, SQL_Helper.Connection))
                {
                    updateCmd.Parameters.AddWithValue("@password", newPassword);
                    updateCmd.Parameters.AddWithValue("@email", email);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return JsonSerializer.Serialize(new
                        {
                            Status = "success",
                            Message = "Đổi mật khẩu thành công"
                        });
                    }
                }

                return JsonSerializer.Serialize(new
                {
                    Status = "error",
                    Message = "Không thể đổi mật khẩu"
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    Status = "error",
                    Message = $"Lỗi đổi mật khẩu: {ex.Message}"
                });
            }
        }

        private string GenerateResetToken()
        {
            return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }

        private bool Send_Email(string To, string Subject, string Body)
        {
            try
            {
                MailMessage Mail = new MailMessage();
                Mail.From = new MailAddress("hoangphihung200706@gmail.com");
                Mail.To.Add(To);
                Mail.Subject = Subject;
                Mail.Body = Body;
                SmtpClient Smtp = new SmtpClient("smtp.gmail.com", 587);

                Smtp.Credentials = new System.Net.NetworkCredential("hoangphihung200706@gmail.com", "jhtp vhhn bavf bqeo");
                Smtp.EnableSsl = true;
                Smtp.Send(Mail);
                return true;
            }
            catch (Exception Ex)
            {
                Log("Lỗi gửi mail: " + Ex.Message);
                return false;
            }
        }
    }
}