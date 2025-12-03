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
using System.Threading.Tasks;
using System.Windows.Forms;

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

        // Quản lý người dùng đã dăng nhập
        private Dictionary<string, TcpClient> Active_Connections = new Dictionary<string, TcpClient>();
        private readonly object Login_Lock = new object();
        private readonly object Db_Lock = new object();

        // Quản lý token reset pass
        private Dictionary<string, string> PasswordResetTokens = new Dictionary<string, string>();
        private readonly object PasswordResetLock = new object();

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

        public static void Broadcast_IP()
        {
            Task.Run(async () =>
            {
                using (UdpClient udpServer = new UdpClient(2222))
                {
                    while (true)
                    {
                        var result = await udpServer.ReceiveAsync();
                        string message = Encoding.UTF8.GetString(result.Buffer);

                        if (message == "discover_server")
                        {
                            byte[] response = Encoding.UTF8.GetBytes("server_here");
                            await udpServer.SendAsync(response, response.Length, result.RemoteEndPoint);
                            Console.WriteLine($"Đã gửi IP cho {result.RemoteEndPoint.Address}");
                        }
                    }
                }
            });
        }

        private void btn_Start_Server_Click(object sender, EventArgs e)
        {
            Task.Run(() => Start_Server());
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

                Broadcast_IP();
                Log("Server is broadcasting on port 2222...");

                while (true)
                {
                    TcpClient TCP_Client = TCP_Server.AcceptTcpClient();
                    Log($"Client {TCP_Client.Client.RemoteEndPoint} đã kết nối!");
                    Task.Run(() => Handle_Client(TCP_Client));
                }
            }
            catch (Exception Ex)
            {
                Log($"Lỗi máy chủ: {Ex.Message}");
            }
        }

        private async void Send_Message(NetworkStream Stream, string Message)
        {
            if (Stream == null || !Stream.CanWrite) return;
            try
            {
                byte[] Response_Bytes = Encoding.UTF8.GetBytes(Message + "\n");
                await Stream.WriteAsync(Response_Bytes, 0, Response_Bytes.Length);
            }
            catch (Exception Ex)
            {
                Log($"Lỗi khi gửi tin nhắn: {Ex.Message}");
            }
        }

        // Hàm xử lý client 
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

                    if (!Message.Contains("move"))
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
                                    Response = Handle_Login(Login_Data, Client);
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

                    }
                    catch (Exception Ex)
                    {
                        Log($" Lỗi xử lý client: {Ex.Message}. Data: {Message}");
                    }
                }
            }
            catch (Exception Ex)
            {
                Log($"Lỗi Client: {Ex.Message}");
            }
            finally
            {
                Handle_Disconnect(Client);
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

        // Hàm xử lý đăng nhập 
        private string Handle_Login(Dictionary<string, string> data, TcpClient client)
        {
            try
            {
                string username = data["username"];
                string password = data["password"];

                string userEmail = GetEmailFromUsername(username);
                if (string.IsNullOrEmpty(userEmail))
                {
                    return JsonSerializer.Serialize(new { status = "error", message = "Tài khoản không tồn tại" });
                }

                lock (Login_Lock)
                {
                    if (Active_Connections.ContainsKey(userEmail))
                    {
                        // Kick người cũ
                        TcpClient Old_Client = Active_Connections[userEmail];
                        try
                        {
                            if (Old_Client != null && Old_Client.Connected)
                            {
                                NetworkStream Old_Stream = Old_Client.GetStream();
                                string kickMsg = JsonSerializer.Serialize(new { status = "force_logout", message = "Tài khoản đã bị đăng nhập nơi khác" });
                                Send_Message(Old_Stream, kickMsg);
                                Old_Client.Close();
                            }
                        }
                        catch { }
                        Active_Connections.Remove(userEmail);
                    }
                }

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
                    lock (Login_Lock)
                    {
                        Active_Connections[userEmail] = client;
                    }
                    return JsonSerializer.Serialize(new { status = "success", message = "Login success", username = username });
                }
                else
                {
                    return JsonSerializer.Serialize(new { status = "error", message = "Sai mật khẩu" });
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new { status = "error", message = ex.Message });
            }
        }

        // Hàm xử lý đăng kí
        private string Handle_Register(Dictionary<string, string> data)
        {
            lock (Db_Lock)
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
                                message = "Email đã tồn tại"
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
                                message = "Tên người dùng đã tồn tại"
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
                                message = "Đăng ký thành công"
                            });
                        }
                        else
                        {
                            return JsonSerializer.Serialize(new
                            {
                                status = "error",
                                message = "Đăng ký thất bại"
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status = "error",
                        message = $"Lỗi đăng ký: {ex.Message}"
                    });
                }
            }
        }

        // Hàm lấy thông tin người dùng
        private string Handle_Get_Info(Dictionary<string, string> data)
        {
            lock (Db_Lock)
            {
                try
                {
                    string query = "SELECT Username, Email, Birthday FROM Info_User WHERE Username=@u";
                    using (var cmd = new SQLiteCommand(query, SQL_Helper.Connection))
                    {
                        cmd.Parameters.AddWithValue("@u", data["username"]);
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
                        }
                    }
                    return JsonSerializer.Serialize(new { status = "error", message = "User not found" });
                }
                catch (Exception ex) { return JsonSerializer.Serialize(new { status = "error", message = ex.Message }); }
            }
        }

        private string Handle_Forgot_Password(Dictionary<string, string> data)
        {
            lock (Db_Lock)
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
                                        status = "success",
                                        message = "Đã gửi token đặt lại mật khẩu"
                                    });
                                }
                                else
                                {
                                    return JsonSerializer.Serialize(new
                                    {
                                        status = "error",
                                        message = "Không thể gửi token"
                                    });
                                }
                            }
                            else
                            {
                                return JsonSerializer.Serialize(new
                                {
                                    status = "error",
                                    message = "Email không tồn tại"
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
                        message = $"Lỗi quên mật khẩu: {ex.Message}"
                    });
                }
            }
        }

        // Hàm xử lý đổi mật khẩu
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
                            status = "error",
                            message = "Mã xác thực không hợp lệ hoặc đã hết hạn"
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
                            status = "success",
                            message = "Đổi mật khẩu thành công"
                        });
                    }
                }

                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Không thể đổi mật khẩu"
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = $"Lỗi đổi mật khẩu: {ex.Message}"
                });
            }
        }

        private string GenerateRoomID()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        // Hàm xử lý tạo phòng
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
                return JsonSerializer.Serialize(new { status = "create_room_success", room_id = ID_Room, player_number = Player_Number });
            }
        }

        // Hàm xử lí vào phòng
        private string Handle_Join_Room(TcpClient Client, Dictionary<string, JsonElement> Data)
        {
            string Username = Data.ContainsKey("username") ? Data["username"].GetString() : "Unknown";

            string ID_Room = Data.ContainsKey("room_id") ? Data["room_id"].GetString() : "";

            if (string.IsNullOrEmpty(ID_Room))
            {
                return JsonSerializer.Serialize(new { status = "error", message = "Vui lòng nhập ID phòng" });
            }

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
                        return JsonSerializer.Serialize(new
                        {
                            status = "join_room_success",
                            room_id = ID_Room,
                            player_number = Player_Number
                        });
                    }
                    else
                    {
                        return JsonSerializer.Serialize(new { status = "error", message = "Phòng đã đầy" });
                    }
                }
                else
                {
                    return JsonSerializer.Serialize(new { status = "error", message = "Không tìm thấy phòng này" });
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

                if (action == "leave_room")
                {
                    lock (Room_Lock)
                    {
                        Players.Remove(Client);

                        if (Rooms.ContainsKey(ID_Room) && Rooms[ID_Room].IsEmpty())
                        {
                            Rooms.Remove(ID_Room);
                            Log($"Phòng {ID_Room} đã đóng do không còn người chơi.");
                        }
                    }
                }
            }
        }

        private void Handle_Disconnect(TcpClient Client)
        {
            // Xử lý đăng xuất ở trong phòng
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

            // Xử lý đăng nhập trùng
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
                    Log($"User {Email_To_Remove} đã đăng xuất.");
                }
            }

            try { Client?.Close(); } catch { }
            Log($"Client ngắt kết nối.");
        }

        private string GenerateResetToken()
        {
            return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }

        // Hàm gửi mail
        private bool Send_Email(string To, string Subject, string Body)
        {
            try
            {
                MailMessage Mail = new MailMessage();
                Mail.From = new MailAddress("pixeldriftsysop@gmail.com");
                Mail.To.Add(To);
                Mail.Subject = Subject;
                Mail.Body = Body;
                SmtpClient Smtp = new SmtpClient("smtp.gmail.com", 587);

                Smtp.Credentials = new NetworkCredential("pixeldriftsysop@gmail.com", "empr bqxh cdwt vyrj");
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