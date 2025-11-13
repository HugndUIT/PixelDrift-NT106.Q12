using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Pixel_Drift_Server;
using System.Windows.Forms;

namespace Pixel_Drift_Server
{
    public partial class ServerForm : Form
    {
        TcpListener TCP_Server;

        Dictionary<TcpClient, string> Client_List = new Dictionary<TcpClient, string>();

        private async void ServerForm_Load(object sender, EventArgs e)
        {
            try
            {
                SQL_Helper.Initialize();
                Log("SQLite đã sẵn sàng!");
            }
            catch (Exception ex)
            {
                Log("Lỗi SQLite: " + ex.Message);
            }
        }

        private void Log(string message)
        {
            // Nếu hàm này được gọi từ luồng khác với luồng giao diện (UI)
            if (tb_hienthi.InvokeRequired)
            {
                // Dùng Invoke để an toàn cập nhật textbox từ luồng khác
                tb_hienthi.Invoke(new Action(() => tb_hienthi.AppendText(message + Environment.NewLine)));
            }
            else
            {
                // Nếu đang ở luồng giao diện thì ghi trực tiếp
                tb_hienthi.AppendText(message + Environment.NewLine);
            }
        }

        private void StartServer()
        {
            try
            {
                TCP_Server = new TcpListener(IPAddress.Any, 1111);
                TCP_Server.Start();
                Log("Server is listening on port 1111...");

                while (true)
                {
                    TcpClient TCP_Client = TCP_Server.AcceptTcpClient();
                    if (!Client_List.ContainsKey(TCP_Client))
                        Client_List.Add(TCP_Client, "");
                    Thread Client_Thread = new Thread(() => HandleClient(TCP_Client));
                    Client_Thread.IsBackground = true;
                    Client_Thread.Start();
                }
            }
            catch (Exception Ex)
            {
                Log($"Server Error: {Ex.Message}");
            }
        }

        private void HandleClient(TcpClient Client)
        {
            try
            {
                NetworkStream Stream = Client.GetStream();
                byte[] Buffer = new byte[4096];
                int Bytes_Read;

                while ((Bytes_Read = Stream.Read(Buffer, 0, Buffer.Length)) > 0)
                {
                    string message = Encoding.UTF8.GetString(Buffer, 0, Bytes_Read);
                    Log($"[{Client.Client.RemoteEndPoint}] gửi: {message}");

                    try
                    {
                        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(message);
                        string action = data.ContainsKey("action") ? data["action"] : "";

                        string response;

                        switch (action)
                        {
                            case "login":
                                response = HandleLogin(data);
                                break;

                            case "register":
                                response = HandleRegister(data);
                                break;

                            case "get_info":
                                response = HandleGetInfo(data);
                                break;

                            case "forgot_password":
                                response = HandleForgotPassword(data);
                                break;

                            case "change_password":
                                response = HandleChangePassword(data);
                                break;

                            case "left_move":
                                response = HandleLeftMove(data);
                                break;

                            case "right_move":
                                response = HandleRightMove(data);
                                break;

                            case "crash":
                                response = HandleCrash(data);
                                break;

                            case "time":
                                response = HandleTime(data);
                                break;

                            case "positon_object":
                                response = HandlePosition(data);
                                break;

                            case "scoreboard":
                                response = HandleScoreboard(data);
                                break;

                            default:
                                response = JsonSerializer.Serialize(new { status = "error", message = "Unknown action" });
                                break;
                        }

                        byte[] Response_Bytes = Encoding.UTF8.GetBytes(response);
                        Stream.Write(Response_Bytes, 0, Response_Bytes.Length);
                    }
                    catch (JsonException)
                    {
                        Log(" Dữ liệu từ client không phải JSON hợp lệ!");
                    }
                }
            }
            catch
            {
                // Xử lý mất kết nối người dùng
            }
            finally
            {
                Log($"Client {Client.Client.RemoteEndPoint} đã ngắt kết nối.");
            }
        }

        // Xử lý đăng nhập
        private string HandleLogin(Dictionary<string, string> data)
        {
            string username = data["username"];
            string password = data["password"];

            try
            {
                using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Info_User WHERE Email=@u AND Password=@p", SQL_Helper.Connection))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);
                    long count = (long)cmd.ExecuteScalar();

                    if (count > 0)
                        return JsonSerializer.Serialize(new { status = "success", message = "Đăng nhập thành công!" });
                    else
                        return JsonSerializer.Serialize(new { status = "error", message = "Sai tài khoản hoặc mật khẩu!" });
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new { status = "error", message = "Lỗi SQLite: " + ex.Message });
            }
        }

        // Xử lý đăng ký
        private string HandleRegister(Dictionary<string, string> data)
        {
            string email = data["email"];
            string username = data["username"];
            string password = data["password"];
            string birthday = data["birthday"];

            try
            {
                using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Info_User WHERE Username=@u OR Email=@e", SQL_Helper.Connection))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@e", email);
                    long count = (long)cmd.ExecuteScalar();

                    if (count > 0)
                        return JsonSerializer.Serialize(new { status = "error", message = "Tên người dùng hoặc email đã tồn tại!" });
                }

                using (var cmd = new SQLiteCommand("INSERT INTO Info_User (Username, Email, Password, Birthday) VALUES (@u, @e, @p, @b)", SQL_Helper.Connection))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.Parameters.AddWithValue("@p", password);
                    cmd.Parameters.AddWithValue("@b", birthday);
                    cmd.ExecuteNonQuery();
                }

                return JsonSerializer.Serialize(new { status = "success", message = "Đăng ký thành công!" });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new { status = "error", message = "Lỗi SQLite: " + ex.Message });
            }
        }

        // Xử lý lấy thông tin
        private string HandleGetInfo(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("username"))
                return JsonSerializer.Serialize(new { status = "error", message = "Thiếu tham số 'username'." });

            string username = data["username"];

            try
            {
                using (var cmd = new SQLiteCommand("SELECT Username, Email, Birthday FROM Info_User WHERE Email=@u", SQL_Helper.Connection))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return JsonSerializer.Serialize(new { status = "error", message = "Không tìm thấy người dùng!" });

                        return JsonSerializer.Serialize(new
                        {
                            status = "success",
                            data = new
                            {
                                username = reader["Username"].ToString(),
                                email = reader["Email"].ToString(),
                                birthday = reader["Birthday"].ToString()
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new { status = "error", message = "Lỗi SQLite: " + ex.Message });
            }
        }

        // Xử lý quên mật khẩu
        private string HandleForgotPassword(Dictionary<string, string> data)
        {
            try
            {
                if (!data.ContainsKey("email"))
                    return JsonSerializer.Serialize(new { status = "error", message = "Thiếu email!" });

                string email = data["email"];
                string password = null;
                string username = null;

                using (var cmd = new SQLiteCommand("SELECT Username, Password FROM Info_User WHERE Email=@e", SQL_Helper.Connection))
                {
                    cmd.Parameters.AddWithValue("@e", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return JsonSerializer.Serialize(new { status = "error", message = "Email không tồn tại!" });

                        username = reader["Username"].ToString();
                        password = reader["Password"].ToString();
                    }
                }

                bool sent = SendEmail(email, "Khôi phục mật khẩu", $"Xin chào {username}, mật khẩu của bạn là: {password}");
                if (sent)
                    return JsonSerializer.Serialize(new { status = "success", message = "Mật khẩu đã được gửi đến email của bạn!" });
                else
                    return JsonSerializer.Serialize(new { status = "error", message = "Không thể gửi email!" });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new { status = "error", message = "Lỗi xử lý forgot_password: " + ex.Message });
            }
        }

        // Xử lý đổi mật khẩu
        private string HandleChangePassword(Dictionary<string, string> data)
        {
            try
            {
                string email = data["email"];
                string token = data["token"];
                string newPassword = data["new_password"];

                using (var cmd = new SQLiteCommand("SELECT Password FROM Info_User WHERE Email=@e", SQL_Helper.Connection))
                {
                    cmd.Parameters.AddWithValue("@e", email);
                    object result = cmd.ExecuteScalar();

                    if (result == null)
                        return JsonSerializer.Serialize(new { status = "error", message = "Email không tồn tại!" });

                    string oldPassword = result.ToString();

                    if (oldPassword != token)
                        return JsonSerializer.Serialize(new { status = "error", message = "Mã xác thực không đúng!" });
                }

                using (var cmd = new SQLiteCommand("UPDATE Info_User SET Password=@p WHERE Email=@e", SQL_Helper.Connection))
                {
                    cmd.Parameters.AddWithValue("@p", MaHoa(newPassword));
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.ExecuteNonQuery();
                }

                return JsonSerializer.Serialize(new { status = "success", message = "Đổi mật khẩu thành công!" });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new { status = "error", message = "Lỗi khi đổi mật khẩu: " + ex.Message });
            }
        }

        private bool SendEmail(string to, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("hoangphihung200706@gmail.com");
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new System.Net.NetworkCredential("hoangphihung200706@gmail.com", "jhtp vhhn bavf bqeo");
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Log("Lỗi gửi mail: " + ex.Message);
                return false;
            }
        }

        // Xử lý di chuyển
        private string HandleLeftMove(Dictionary<string, string> data)
        {
            return "";
        }

        private string HandleRightMove(Dictionary<string, string> data)
        {
            return "";
        }

        // Xử lý va chạm
        private string HandleCrash(Dictionary<string, string> data)
        {
            return "";
        }

        // Xử lý thời gian
        private string HandleTime(Dictionary<string, string> data)
        {
            return "";
        }

        // Xử lý vị trí vật thể
        private string HandlePosition(Dictionary<string, string> data)
        {
            return "";
        }

        // Xử lý scoreboard
        private string HandleScoreboard(Dictionary<string, string> data)
        {
            return "";
        }

        // Hàm mã hóa SHA256 
        private string MaHoa(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        public ServerForm()
        {
            InitializeComponent();
        }

        private void btn_Start_Server_Click(object sender, EventArgs e)
        {
            Thread Server_Thread = new Thread(StartServer);
            Server_Thread.IsBackground = true;
            Server_Thread.Start();
        }
    }
}