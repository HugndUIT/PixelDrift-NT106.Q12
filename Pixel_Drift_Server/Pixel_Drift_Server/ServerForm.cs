using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;

namespace Pixel_Drift_Server
{
    public partial class ServerForm : Form
    {
        private TcpListener TCP_Server;
        private const int PORT = 1111;
        private bool IsRunning = false;

        // Danh sách các phòng đang hoạt động
        public List<Game_Room> Rooms = new List<Game_Room>();

        // Map nhanh từ Client -> Phòng (để biết client đang ở phòng nào)
        public Dictionary<TcpClient, Game_Room> ClientRoom = new Dictionary<TcpClient, Game_Room>();

        // Map để kiểm tra user đang online (tránh đăng nhập 2 nơi)
        public Dictionary<string, Game_Player> ConnectedUsers = new Dictionary<string, Game_Player>();

        // Object dùng để khóa luồng (Lock)
        private readonly object MainLock = new object();

        private Random Server_Rand = new Random();

        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            try
            {
                SQL_Helper.Initialize();
                Log("Cơ sở dữ liệu SQLite đã sẵn sàng!");
            }
            catch (Exception ex)
            {
                Log("Lỗi khởi tạo Database: " + ex.Message);
            }
        }

        private void btn_Start_Server_Click(object sender, EventArgs e)
        {
            if (IsRunning) return;

            IsRunning = true;
            btn_Start_Server.Enabled = false;
            Thread serverThread = new Thread(Start_Server);
            serverThread.IsBackground = true;
            serverThread.Start();
        }

        private void Start_Server()
        {
            try
            {
                TCP_Server = new TcpListener(IPAddress.Any, PORT);
                TCP_Server.Start();
                Log($"Server đang lắng nghe tại Port {PORT}...");

                while (IsRunning)
                {
                    TcpClient client = TCP_Server.AcceptTcpClient();
                    Log($"Client kết nối mới từ: {client.Client.RemoteEndPoint}");

                    // Tạo luồng riêng cho mỗi Client
                    Thread clientThread = new Thread(() => Handle_Client(client));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Log("Lỗi Server Listener: " + ex.Message);
            }
        }

        private void Handle_Client(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);

            // Tạo đối tượng Player tạm thời để theo dõi phiên này
            Game_Player currentPlayer = new Game_Player(client, "Unknown");

            try
            {
                string message;
                while ((message = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(message)) continue;

                    Dictionary<string, JsonElement> data;
                    try
                    {
                        data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(message);
                    }
                    catch { continue; } 

                    if (!data.ContainsKey("action")) continue;
                    string action = data["action"].GetString();

                    if (action != "move") Log($"[{currentPlayer.Username}]: {action}");

                    string response = null;

                    switch (action)
                    {
                        case "login":
                            response = Process_Login(data, currentPlayer);
                            break;

                        case "register":
                            response = Process_Register(data);
                            break;

                        case "forgot_password":
                            response = Process_ForgotPassword(data);
                            break;

                        case "change_password":
                            response = Process_ChangePassword(data);
                            break;

                        case "get_info":
                            response = Process_GetInfo(data);
                            break;

                        case "create_room":
                            response = Process_CreateRoom(data, client, currentPlayer);
                            break;

                        case "join_room":
                            response = Process_JoinRoom(data, client, currentPlayer);
                            break;

                        case "get_rooms":
                            response = Process_GetListRooms();
                            break;

                        case "move":
                        case "set_ready":
                            ForwardToRoom(client, action, data);
                            break;

                        case "get_scoreboard":
                            int top = data.ContainsKey("top_count") ? data["top_count"].GetInt32() : 50;
                            response = JsonSerializer.Serialize(new { action = "scoreboard_data", data = SQL_Helper.GetTopScores(top) });
                            break;

                        case "search_player":
                            response = JsonSerializer.Serialize(new { action = "search_result", data = SQL_Helper.SearchPlayer(data["search_text"].GetString()) });
                            break;
                    }

                    if (response != null) Send_Json(stream, response);
                }
            }
            catch (Exception ex)
            {
                Log($"Lỗi kết nối với {currentPlayer.Username}: {ex.Message}");
            }
            finally
            {
                Handle_Disconnect_Cleanup(client, currentPlayer);
            }
        }

        private string Process_CreateRoom(Dictionary<string, JsonElement> data, TcpClient client, Game_Player player)
        {
            string hostName = data["username"].GetString();
            string roomID = GenerateRoomID();

            // Cập nhật thông tin người chơi hiện tại
            player.Username = hostName;

            lock (MainLock)
            {
                Game_Room newRoom = new Game_Room(roomID, hostName, player);
                Rooms.Add(newRoom);
                ClientRoom[client] = newRoom;
                ConnectedUsers[hostName] = player;
            }

            Log($"Phòng mới: {roomID} tạo bởi {hostName}");
            return JsonSerializer.Serialize(new { action = "create_success", room_id = roomID });
        }

        private string Process_JoinRoom(Dictionary<string, JsonElement> data, TcpClient client, Game_Player player)
        {
            string targetID = data["room_id"].GetString();
            string guestName = data["username"].GetString();
            player.Username = guestName;

            lock (MainLock)
            {
                Game_Room room = Rooms.FirstOrDefault(r => r.ID == targetID);
                if (room != null)
                {
                    if (room.Join(player))
                    {
                        ClientRoom[client] = room;
                        ConnectedUsers[guestName] = player;
                        Log($">> {guestName} đã vào phòng {targetID}");
                        return JsonSerializer.Serialize(new { action = "join_success", room_id = targetID });
                    }
                    else return JsonSerializer.Serialize(new { action = "join_failed", message = "Phòng đầy hoặc đang chơi" });
                }
            }
            return JsonSerializer.Serialize(new { action = "join_failed", message = "Không tìm thấy phòng" });
        }

        private string Process_GetListRooms()
        {
            lock (MainLock)
            {
                var list = Rooms.Select(r => new { id = r.ID, name = r.Name, status = r.Is_In_Game ? "Playing" : "Waiting" });
                return JsonSerializer.Serialize(new { action = "room_list", rooms = list });
            }
        }

        private void ForwardToRoom(TcpClient client, string action, Dictionary<string, JsonElement> data) 
        {
            Game_Room room = null;
            lock (MainLock)
            {
                if (ClientRoom.ContainsKey(client)) room = ClientRoom[client];
            }

            if (room != null)
            {
                // Xác định là Player 1 hay 2
                int pNum = (room.Player_1 != null && room.Player_1.Client == client) ? 1 : 2;

                string jsonString = JsonSerializer.Serialize(data);
                JsonElement element = JsonDocument.Parse(jsonString).RootElement;

                room.Handle_Player(pNum, action, element);
            }
        }

        private void Handle_Disconnect_Cleanup(TcpClient client, Game_Player player)
        {
            Log($"{player.Username} đã ngắt kết nối.");

            lock (MainLock)
            {
                if (ClientRoom.ContainsKey(client))
                {
                    Game_Room room = ClientRoom[client];

                    room.Handle_Disconnect(client);
                    ClientRoom.Remove(client);

                    if (room.IsEmpty() || room.Player_1 == null)
                    {
                        Rooms.Remove(room);
                        Log($">> Phòng {room.ID} đã giải tán.");
                    }
                }

                if (!string.IsNullOrEmpty(player.Username) && ConnectedUsers.ContainsKey(player.Username))
                {
                    ConnectedUsers.Remove(player.Username);
                }
            }

            try { client.Close(); } catch { }
        }

        private string Process_Login(Dictionary<string, JsonElement> data, Game_Player player)
        {
            string u = data["username"].GetString();
            string p = data["password"].GetString();

            // Check database
            string query = "SELECT COUNT(*) FROM Info_User WHERE Username=@u AND Password=@p";
            using (var cmd = new SQLiteCommand(query, SQL_Helper.Connection))
            {
                cmd.Parameters.AddWithValue("@u", u);
                cmd.Parameters.AddWithValue("@p", p);
                long count = (long)cmd.ExecuteScalar();

                if (count > 0)
                {
                    // Cập nhật tên cho session hiện tại
                    player.Username = u;
                    return JsonSerializer.Serialize(new { status = "success", message = "Đăng nhập thành công", username = u });
                }
            }
            return JsonSerializer.Serialize(new { status = "error", message = "Sai tài khoản hoặc mật khẩu" });
        }

        private string Process_Register(Dictionary<string, JsonElement> data)
        {
            try
            {
                string qCheck = "SELECT COUNT(*) FROM Info_User WHERE Email=@e OR Username=@u";
                using (var cmd = new SQLiteCommand(qCheck, SQL_Helper.Connection))
                {
                    cmd.Parameters.AddWithValue("@e", data["email"].GetString());
                    cmd.Parameters.AddWithValue("@u", data["username"].GetString());
                    if ((long)cmd.ExecuteScalar() > 0) return JsonSerializer.Serialize(new { status = "error", message = "Email hoặc User đã tồn tại" });
                }

                string qInsert = "INSERT INTO Info_User (Username, Email, Password, Birthday) VALUES (@u, @e, @p, @b)";
                using (var cmd = new SQLiteCommand(qInsert, SQL_Helper.Connection))
                {
                    cmd.Parameters.AddWithValue("@u", data["username"].GetString());
                    cmd.Parameters.AddWithValue("@e", data["email"].GetString());
                    cmd.Parameters.AddWithValue("@p", data["password"].GetString());
                    cmd.Parameters.AddWithValue("@b", data["birthday"].GetString());
                    cmd.ExecuteNonQuery();
                }
                return JsonSerializer.Serialize(new { status = "success", message = "Đăng ký thành công" });
            }
            catch (Exception ex) { return JsonSerializer.Serialize(new { status = "error", message = ex.Message }); }
        }

        private string Process_GetInfo(Dictionary<string, JsonElement> data)
        {
            string q = "SELECT Username, Email, Birthday FROM Info_User WHERE Username=@u";
            using (var cmd = new SQLiteCommand(q, SQL_Helper.Connection))
            {
                cmd.Parameters.AddWithValue("@u", data["username"].GetString());
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        return JsonSerializer.Serialize(new
                        {
                            status = "success",
                            username = r["Username"].ToString(),
                            email = r["Email"].ToString(),
                            birthday = r["Birthday"].ToString()
                        });
                    }
                }
            }
            return JsonSerializer.Serialize(new { status = "error", message = "User not found" });
        }

        private string Process_ForgotPassword(Dictionary<string, JsonElement> data)
        {
            string q = "SELECT Password FROM Info_User WHERE Email=@e";
            using (var cmd = new SQLiteCommand(q, SQL_Helper.Connection))
            {
                cmd.Parameters.AddWithValue("@e", data["email"].GetString());
                var res = cmd.ExecuteScalar();
                if (res != null) return JsonSerializer.Serialize(new { status = "success", message = "Password recovery sent" });
            }
            return JsonSerializer.Serialize(new { status = "error", message = "Email not found" });
        }

        private string Process_ChangePassword(Dictionary<string, JsonElement> data)
        {
            string q = "UPDATE Info_User SET Password=@p WHERE Email=@e";
            using (var cmd = new SQLiteCommand(q, SQL_Helper.Connection))
            {
                cmd.Parameters.AddWithValue("@p", data["new_password"].GetString());
                cmd.Parameters.AddWithValue("@e", data["email"].GetString());
                if (cmd.ExecuteNonQuery() > 0) return JsonSerializer.Serialize(new { status = "success", message = "Đổi mật khẩu thành công" });
            }
            return JsonSerializer.Serialize(new { status = "error", message = "Lỗi đổi mật khẩu" });
        }

        private void Log(string msg)
        {
            if (tb_hienthi.InvokeRequired)
            {
                tb_hienthi.Invoke(new Action(() => tb_hienthi.AppendText(msg + Environment.NewLine)));
            }
            else
            {
                tb_hienthi.AppendText(msg + Environment.NewLine);
            }
        }

        private void Send_Json(NetworkStream stream, object dataObj)
        {
            try
            {
                string json = dataObj is string s ? s : JsonSerializer.Serialize(dataObj);
                byte[] bytes = Encoding.UTF8.GetBytes(json + "\n");
                stream.Write(bytes, 0, bytes.Length);
            }
            catch { }
        }

        private string GenerateRoomID()
        {
            lock (MainLock)
            {
                string id;
                do { id = Server_Rand.Next(100000, 999999).ToString(); }
                while (Rooms.Any(r => r.ID == id));
                return id;
            }
        }
    }
}