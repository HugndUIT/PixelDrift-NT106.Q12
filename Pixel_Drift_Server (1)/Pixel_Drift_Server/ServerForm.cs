using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Text;

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

        // Lưu trữ vị trí (Point) của tất cả các đối tượng trong game
        private Dictionary<string, Point> Game_Objects = new Dictionary<string, Point>();
        // Lưu trữ kích thước (Size) của các đối tượng trong game
        private Dictionary<string, Size> Object_Sizes = new Dictionary<string, Size>();
        private int P1_Speed = 10;
        private int P2_Speed = 10;
        private long P1_Score = 0;
        private long P2_Score = 0;
        private bool P1_Left, P1_Right, P2_Left, P2_Right;

        private const int Game_Height = 800;
        private const int P1_Min_X = -5;
        private const int P1_Max_X = 475;
        private const int P2_Min_X = -5;
        private const int P2_Max_X = 475;

        private Random Server_Rand = new Random();

        // Lớp chứa thông tin chi tiết về một slot người chơi
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

        // Xử lý sự kiện khi Form Server được load, dùng để khởi tạo cơ sở dữ liệu
        private async void ServerForm_Load(object sender, EventArgs e)
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

        // Xử lý sự kiện click nút Start Server, khởi động luồng lắng nghe chính
        private void btn_Start_Server_Click(object sender, EventArgs e)
        {
            Thread Server_Thread = new Thread(Start_Server);
            Server_Thread.IsBackground = true;
            Server_Thread.Start();
            btn_Start_Server.Enabled = false;
        }

        // Ghi log ra TextBox trên Form (đảm bảo Thread-safe)
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

        // Khởi động TCP Listener và chấp nhận kết nối client
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
                    Thread Client_Thread = new Thread(() => Handle_Client(TCP_Client));
                    Client_Thread.IsBackground = true;
                    Client_Thread.Start();
                }
            }
            catch (Exception Ex)
            {
                Log($"Server Error: {Ex.Message}");
            }
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
                Log($"Lỗi khi gửi tin nhắn: {Ex.Message}");
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
                    Log("Cả hai đã sẵn sàng. Bắt đầu đếm ngược 5s...");
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
                Log("Bắt đầu game!");

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
                Object_Sizes["ptb_roadtrack1"] = new Size(611, 734); 
                Game_Objects["ptb_roadtrack1"] = new Point(0, -2);

                Object_Sizes["ptb_roadtrack1dup"] = new Size(611, 734);
                Game_Objects["ptb_roadtrack1dup"] = new Point(0, 734);

                Object_Sizes["ptb_roadtrack2"] = new Size(611, 734); 
                Game_Objects["ptb_roadtrack2"] = new Point(0, 2); 

                Object_Sizes["ptb_roadtrack2dup"] = new Size(611, 734);
                Game_Objects["ptb_roadtrack2dup"] = new Point(0, 734); 

                // --- KHỞI TẠO AI CAR & ITEM (QUAN TRỌNG: SIZE TRƯỚC - VỊ TRÍ SAU) ---

                // AI Car 1
                Object_Sizes["ptb_AICar1"] = new Size(50, 100); // Khai báo size trước
                Game_Objects["ptb_AICar1"] = Reposition_Object("ptb_AICar1", Min_X, Max_X); 

                // AI Car 3
                Object_Sizes["ptb_AICar3"] = new Size(74, 128);
                Game_Objects["ptb_AICar3"] = Reposition_Object("ptb_AICar3", Min_X, Max_X);

                // AI Car 5
                Object_Sizes["ptb_AICar5"] = new Size(50, 100);
                Game_Objects["ptb_AICar5"] = Reposition_Object("ptb_AICar5", Min_X, Max_X);

                // AI Car 6
                Object_Sizes["ptb_AICar6"] = new Size(74, 128);
                Game_Objects["ptb_AICar6"] = Reposition_Object("ptb_AICar6", Min_X, Max_X);

                // Buffs / Debuffs
                Object_Sizes["ptb_increasingroad1"] = new Size(15, 15);
                Game_Objects["ptb_increasingroad1"] = Reposition_Object("ptb_increasingroad1", Min_X, Max_X);

                Object_Sizes["ptb_decreasingroad1"] = new Size(15, 15);
                Game_Objects["ptb_decreasingroad1"] = Reposition_Object("ptb_decreasingroad1", Min_X, Max_X);

                Object_Sizes["ptb_increasingroad2"] = new Size(15, 15);
                Game_Objects["ptb_increasingroad2"] = Reposition_Object("ptb_increasingroad2", Min_X, Max_X);

                Object_Sizes["ptb_decreasingroad2"] = new Size(15, 15);
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
                Log($"LỖI GAME LOOP SERVER: {Ex.Message}");
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

        // Hàm chính xử lý tất cả tin nhắn đến từ client
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
                            // Logic đăng nhập, đăng ký và lấy thông tin tài khoản
                            case "login":
                                var Login_Data = new Dictionary<string, string> { { "username", Data["username"].GetString() }, { "password", Data["password"].GetString() } };
                                Response = Handle_Login(Login_Data);
                                break;

                            case "register":
                                var Reg_Data = new Dictionary<string, string> { { "email", Data["email"].GetString() }, { "username", Data["username"].GetString() }, { "password", Data["password"].GetString() }, { "birthday", Data["birthday"].GetString() } };
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
                                var Change_Data = new Dictionary<string, string> { { "email", Data["email"].GetString() }, { "token", Data["token"].GetString() }, { "new_password", Data["new_password"].GetString() } };
                                Response = Handle_Change_Password(Change_Data);
                                break;

                            // Logic phòng chờ (Lobby) và sẵn sàng (Ready)
                            case "join_lobby":
                                lock (Player_Lock)
                                {
                                    string Username = Data.ContainsKey("username") ? Data["username"].GetString() : "Player";
                                    if (Player_1.Client == null)
                                    {
                                        Player_1.Client = Client; Player_1.Stream = Stream;
                                        Player_1.Username = Username + " (Xe Đỏ)";
                                        Current_Player_Slot = Player_1;
                                        Response = JsonSerializer.Serialize(new { action = "assign_player", player_number = 1 });
                                        Log($"Player 1 ({Player_1.Username}) đã kết nối.");
                                    }
                                    else if (Player_2.Client == null)
                                    {
                                        Player_2.Client = Client; Player_2.Stream = Stream;
                                        Player_2.Username = Username + " (Xe Xanh)";
                                        Current_Player_Slot = Player_2;
                                        Response = JsonSerializer.Serialize(new { action = "assign_player", player_number = 2 });
                                        Log($"Player 2 ({Player_2.Username}) đã kết nối.");
                                    }
                                    else { Response = JsonSerializer.Serialize(new { action = "lobby_full" }); }
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

                            // Logic điều khiển game (Movement)
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
                    catch (Exception Ex) { Log($" Lỗi Handle_Client: {Ex.Message}. Data: {Message}"); }
                }
            }
            catch (Exception Ex)
            {
                Log($"Client Error: {Ex.Message}");
            }
            finally
            {
                Log($"Client {Client.Client.RemoteEndPoint} đã ngắt kết nối.");
                lock (Player_Lock)
                {
                    string Disconnected_Player_Name = "Unknown";
                    if (Current_Player_Slot == Player_1) { Disconnected_Player_Name = Player_1.Username; Player_1 = new Player_Slot(); }
                    else if (Current_Player_Slot == Player_2) { Disconnected_Player_Name = Player_2.Username; Player_2 = new Player_Slot(); }

                    Countdown_Timer?.Dispose(); Countdown_Timer = null;
                    Game_Timer?.Dispose(); Game_Timer = null;
                    Server_Game_Loop?.Dispose(); Server_Game_Loop = null;

                    Broadcast(JsonSerializer.Serialize(new { action = "player_disconnected", Name = Disconnected_Player_Name }));
                    Broadcast_Ready_Status();
                }
                Reader.Close();
                Client.Close();
            }
        }

        // Xử lý yêu cầu đăng nhập bằng cách kiểm tra trong SQLite
        private string Handle_Login(Dictionary<string, string> Data)
        {
            string Username = Data["username"];
            string Password = Data["password"];
            try
            {
                using (var Cmd = new SQLiteCommand("SELECT COUNT(*) FROM Info_User WHERE Email=@u AND Password=@p", SQL_Helper.Connection))
                {
                    Cmd.Parameters.AddWithValue("@u", Username);
                    Cmd.Parameters.AddWithValue("@p", Password);
                    long Count = (long)Cmd.ExecuteScalar();
                    if (Count > 0)
                        return JsonSerializer.Serialize(new { Status = "success", Message = "Đăng nhập thành công!" });
                    else
                        return JsonSerializer.Serialize(new { Status = "error", Message = "Sai tài khoản hoặc mật khẩu!" });
                }
            }
            catch (Exception Ex)
            {
                return JsonSerializer.Serialize(new { Status = "error", Message = "Lỗi SQLite: " + Ex.Message });
            }
        }

        // Xử lý yêu cầu đăng ký người dùng mới vào cơ sở dữ liệu SQLite
        private string Handle_Register(Dictionary<string, string> Data)
        {
            string Email = Data["email"];
            string Username = Data["username"];
            string Password = Data["password"];
            string Birthday = Data["birthday"];
            try
            {
                using (var Cmd = new SQLiteCommand("SELECT COUNT(*) FROM Info_User WHERE Username=@u OR Email=@e", SQL_Helper.Connection))
                {
                    Cmd.Parameters.AddWithValue("@u", Username);
                    Cmd.Parameters.AddWithValue("@e", Email);
                    long Count = (long)Cmd.ExecuteScalar();
                    if (Count > 0)
                        return JsonSerializer.Serialize(new { Status = "error", Message = "Tên người dùng hoặc email đã tồn tại!" });
                }

                using (var Cmd = new SQLiteCommand("INSERT INTO Info_User (Username, Email, Password, Birthday) VALUES (@u, @e, @p, @b)", SQL_Helper.Connection))
                {
                    Cmd.Parameters.AddWithValue("@u", Username);
                    Cmd.Parameters.AddWithValue("@e", Email);
                    Cmd.Parameters.AddWithValue("@p", Password);
                    Cmd.Parameters.AddWithValue("@b", Birthday);
                    Cmd.ExecuteNonQuery();
                }
                return JsonSerializer.Serialize(new { Status = "success", Message = "Đăng ký thành công!" });
            }
            catch (Exception Ex)
            {
                return JsonSerializer.Serialize(new { Status = "error", Message = "Lỗi SQLite: " + Ex.Message });
            }
        }

        // Xử lý yêu cầu lấy thông tin cá nhân của người dùng
        private string Handle_Get_Info(Dictionary<string, string> Data)
        {
            if (!Data.ContainsKey("username"))
                return JsonSerializer.Serialize(new { Status = "error", Message = "Thiếu tham số 'username'." });
            string Username = Data["username"];
            try
            {
                using (var Cmd = new SQLiteCommand("SELECT Username, Email, Birthday FROM Info_User WHERE Email=@u", SQL_Helper.Connection))
                {
                    Cmd.Parameters.AddWithValue("@u", Username);
                    using (var Reader = Cmd.ExecuteReader())
                    {
                        if (!Reader.Read())
                            return JsonSerializer.Serialize(new { Status = "error", Message = "Không tìm thấy người dùng!" });
                        return JsonSerializer.Serialize(new
                        {
                            Status = "success",
                            Data = new
                            {
                                Username = Reader["Username"].ToString(),
                                Email = Reader["Email"].ToString(),
                                Birthday = Reader["Birthday"].ToString()
                            }
                        });
                    }
                }
            }
            catch (Exception Ex)
            {
                return JsonSerializer.Serialize(new { Status = "error", Message = "Lỗi SQLite: " + Ex.Message });
            }
        }

        // Xử lý yêu cầu quên mật khẩu (Gửi mật khẩu qua email)
        private string Handle_Forgot_Password(Dictionary<string, string> Data)
        {
            try
            {
                if (!Data.ContainsKey("email"))
                    return JsonSerializer.Serialize(new { Status = "error", Message = "Thiếu email!" });
                string Email = Data["email"];
                string Password = null;
                string Username = null;

                using (var Cmd = new SQLiteCommand("SELECT Username, Password FROM Info_User WHERE Email=@e", SQL_Helper.Connection))
                {
                    Cmd.Parameters.AddWithValue("@e", Email);
                    using (var Reader = Cmd.ExecuteReader())
                    {
                        if (!Reader.Read())
                            return JsonSerializer.Serialize(new { Status = "error", Message = "Email không tồn tại!" });
                        Username = Reader["Username"].ToString();
                        Password = Reader["Password"].ToString();
                    }
                }

                bool Sent = Send_Email(Email, "Khôi phục mật khẩu", $"Xin chào {Username}, mật khẩu của bạn là: {Password}");
                if (Sent)
                    return JsonSerializer.Serialize(new { Status = "success", Message = "Mật khẩu đã được gửi đến email của bạn!" });
                else
                    return JsonSerializer.Serialize(new { Status = "error", Message = "Không thể gửi email!" });
            }
            catch (Exception Ex)
            {
                return JsonSerializer.Serialize(new { Status = "error", Message = "Lỗi xử lý forgot_password: " + Ex.Message });
            }
        }

        // Xử lý yêu cầu đổi mật khẩu sau khi xác thực
        private string Handle_Change_Password(Dictionary<string, string> Data)
        {
            try
            {
                string Email = Data["email"];
                string Token = Data["token"];
                string New_Password = Data["new_password"];
                string Old_Password = null;

                using (var Cmd = new SQLiteCommand("SELECT Password FROM Info_User WHERE Email=@e", SQL_Helper.Connection))
                {
                    Cmd.Parameters.AddWithValue("@e", Email);
                    object Result = Cmd.ExecuteScalar();
                    if (Result == null)
                        return JsonSerializer.Serialize(new { Status = "error", Message = "Email không tồn tại!" });

                    Old_Password = Result.ToString();

                    if (Old_Password != Token)
                    {

                        if (Ma_Hoa(Old_Password) != Token && Old_Password != Token)
                            return JsonSerializer.Serialize(new { Status = "error", Message = "Mã xác thực không đúng!" });
                    }
                }

                using (var Cmd = new SQLiteCommand("UPDATE Info_User SET Password=@p WHERE Email=@e", SQL_Helper.Connection))
                {
                    Cmd.Parameters.AddWithValue("@p", New_Password);
                    Cmd.Parameters.AddWithValue("@e", Email);
                    Cmd.ExecuteNonQuery();
                }
                return JsonSerializer.Serialize(new { Status = "success", Message = "Đổi mật khẩu thành công!" });
            }
            catch (Exception Ex)
            {
                return JsonSerializer.Serialize(new { Status = "error", Message = "Lỗi khi đổi mật khẩu: " + Ex.Message });
            }
        }

        // Gửi email bằng SmtpClient
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

        // Hàm mã hóa SHA256 cho mật khẩu (được dùng để kiểm tra mã xác thực/token)
        private string Ma_Hoa(string Password)
        {
            using (var Sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] Bytes = Sha.ComputeHash(Encoding.UTF8.GetBytes(Password));
                StringBuilder Builder = new StringBuilder();
                foreach (byte B in Bytes)
                    Builder.Append(B.ToString("x2"));
                return Builder.ToString();
            }
        }
    }
}