using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Pixel_Drift_Server
{
    public partial class ServerForm : Form
    {
        private TcpListener TCP_Server;

        // Danh sách phòng
        private Dictionary<string, Game_Room> Rooms = new Dictionary<string, Game_Room>();

        // Danh sách client theo phòng
        private Dictionary<TcpClient, string> Players = new Dictionary<TcpClient, string>();

        // Biến khóa phòng
        private readonly object Room_Lock = new object();

        public ServerForm()
        {
            InitializeComponent();
        }

        // Xử lý sự kiện khi Form Server được load, dùng để khởi tạo cơ sở dữ liệu
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

        private void Send_Message(NetworkStream Stream, string Message)
        {
            if (Stream.CanWrite)
            {
                byte[] Bytes = Encoding.UTF8.GetBytes(Message + "\n");
                Stream.Write(Bytes, 0, Bytes.Length);
            }
        }

        // Hàm chính xử lý tất cả tin nhắn đến từ client
        private void Handle_Client(TcpClient Client)
        {
            NetworkStream Stream = Client.GetStream();
            StreamReader Reader = new StreamReader(Stream, Encoding.UTF8);
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
                                    Response = Handle_Login(Login_Data, Stream, Client);
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

                            case "create_room":
                                Response = Handle_Create_Room(Client, Data);
                                break;

                            case "join_room":
                                Response = Handle_Join_Room(Client, Data);
                                break;

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

                            case "set_ready":
                                Handle_Game_Action(Client, Action, Data);
                                break;

                            case "move":
                                Handle_Game_Action(Client, Action, Data);
                                break;

                            case "leave_room":
                                Handle_Game_Action(Client, Action, Data);
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
                    catch (JsonException Ex)
                    {
                        Log($" Lỗi JSON: {Ex.Message}. Data: {Message}");
                    }
                    catch (Exception Ex)
                    {
                        Log($" Lỗi Handle_Client: {Ex.Message}. Data: {Message}");
                    }
                    finally
                    {
                        Handle_Disconnect(Client);
                        Reader.Close();
                        Client.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                Log($"Client Error: {Ex.Message}");
            }
            finally
            {
                Log($"Client {Client.Client.RemoteEndPoint} đã ngắt kết nối.");
                Reader.Close();
                Client.Close();
            }
        }

        private string Handle_Login(Dictionary<string, string> data, NetworkStream stream, TcpClient client)
        {
            try
            {
                string username = data["username"];
                string password = data["password"];

                // Kiểm tra trong database
                string query = "SELECT COUNT(*) FROM Info_User WHERE Username=@username AND Password=@password";
                using (var cmd = new SQLiteCommand(query, SQL_Helper.Connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    long count = (long)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        return JsonSerializer.Serialize(new
                        {
                            status = "success",
                            message = "Login successful",
                            username = username
                        });
                    }
                    else
                    {
                        return JsonSerializer.Serialize(new
                        {
                            status = "error",
                            message = "Invalid username or password"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = $"Login error: {ex.Message}"
                });
            }
        }

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
                            status = "error",
                            message = "Email already exists"
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
                            status = "error",
                            message = "Username already exists"
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
                            status = "success",
                            message = "Registration successful"
                        });
                    }
                    else
                    {
                        return JsonSerializer.Serialize(new
                        {
                            status = "error",
                            message = "Registration failed"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = $"Registration error: {ex.Message}"
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
                                status = "success",
                                username = reader["Username"].ToString(),
                                email = reader["Email"].ToString(),
                                birthday = reader["Birthday"].ToString()
                            });
                        }
                        else
                        {
                            return JsonSerializer.Serialize(new
                            {
                                status = "error",
                                message = "User not found"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = $"Get info error: {ex.Message}"
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

                            Log($"Password reset requested for {email}. Password: {password}");

                            return JsonSerializer.Serialize(new
                            {
                                status = "success",
                                message = "Password reset email sent"
                            });
                        }
                        else
                        {
                            return JsonSerializer.Serialize(new
                            {
                                status = "error",
                                message = "Email not found"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = $"Forgot password error: {ex.Message}"
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

                // Kiểm tra email có tồn tại không
                string query = "SELECT Password FROM Info_User WHERE Email=@email";
                using (var cmd = new SQLiteCommand(query, SQL_Helper.Connection))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string oldPassword = reader["Password"].ToString();

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
                                        status = "success",
                                        message = "Password changed successfully"
                                    });
                                }
                            }
                        }
                    }
                }

                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Failed to change password"
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = $"Change password error: {ex.Message}"
                });
            }
        }

        private string GenerateRoomID()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        private string Handle_Create_Room(TcpClient Client, Dictionary<string, JsonElement> Data)
        {
            string Username = Data.ContainsKey("username") ? Data["username"].GetString() : "Unknown";
            string ID_Room = GenerateRoomID();

            lock (Room_Lock)
            {
                if (Rooms.ContainsKey(ID_Room))
                {
                    return JsonSerializer.Serialize(new { status = "error", message = "Phòng đã tồn tại" });
                }

                Game_Room New_Room = new Game_Room(ID_Room);
                New_Room.Log_Action = Log;

                int Player_Number = New_Room.Add_Player(Client, Username);

                Rooms.Add(ID_Room, New_Room);
                Players[Client] = ID_Room;

                Log($"Phòng mới tạo: {ID_Room} bởi {Username}");
                return JsonSerializer.Serialize(new { action = "create_room_succes", room_id = ID_Room, player_number = Player_Number });
            }
        }

        private string Handle_Join_Room(TcpClient Client, Dictionary<string, JsonElement> Data)
        {
            string Username = Data.ContainsKey("username") ? Data["username"].GetString() : "Unknown";
            string ID_Room = GenerateRoomID();

            lock (Room_Lock)
            {
                if (Rooms.ContainsKey(ID_Room))
                {
                    Game_Room Room = Rooms[ID_Room];
                    int Player_Number = Room.Add_Player(Client, Username);

                    if (Player_Number != -1)
                    {
                        Players[Client] = ID_Room;
                        Log($"Client {Username} đã vào phòng {ID_Room}");
                        return JsonSerializer.Serialize(new { action = "join_room_success", room_id = ID_Room, player_number = Player_Number });
                    }
                    else
                    {
                        return JsonSerializer.Serialize(new { status = "error", message = "Room is full" });
                    }
                }
                else
                {
                    return JsonSerializer.Serialize(new { status = "error", message = "Room not found" });
                }
            }
        }

        private void Handle_Game_Action(TcpClient Client, string action, Dictionary<string, JsonElement> Data)
        {
            string ID_Room = null;

            lock (Room_Lock)
            {
                if (Players.ContainsKey(Client))
                    ID_Room = Players[Client];
            }

            if (ID_Room != null && Rooms.ContainsKey(ID_Room))
            {
                Rooms[ID_Room].Handle_Input(Client, action, Data);
            }
        }

        private void Handle_Disconnect(TcpClient Client) 
        {
            lock (Room_Lock)
            {
                if (Players.ContainsKey(Client))
                {
                    string ID_Room = Players[Client];
                    if (Rooms.ContainsKey(ID_Room))
                    {
                        Rooms[ID_Room].Remove_Player(Client);

                        if (Rooms[ID_Room].IsEmpty())
                        {
                            Rooms.Remove(ID_Room);
                            Log($"Phòng {ID_Room} đã đóng do không còn người chơi.");
                        }
                    }
                    Players.Remove(Client);
                }
            }
            Log($"Client ngắt kết nối.");
        }
    }
}