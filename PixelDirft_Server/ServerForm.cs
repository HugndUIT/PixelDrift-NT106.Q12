using Supabase;
using Supabase.Postgrest;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PixelDirft_Server
{
    public partial class frm_Server : Form
    {
        // Đối tượng dùng để lắng nghe (listen) kết nối TCP từ các client
        private TcpListener Listener;

        // Luồng (thread) riêng để chạy server — giúp giao diện không bị đơ khi server đang chạy
        private Thread ListenThread;

        // Biến cờ (flag) dùng để kiểm tra server đang bật hay tắt
        private bool IsRunning = false;

        // Danh sách các client đã kết nối đến server
        private List<TcpClient> Clients = new List<TcpClient>();

        //Ghi trạng thái hoạt động vào text box hiển thị 
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

                // 1. Khởi tạo TcpListener — lắng nghe tất cả IP (IPAddress.Any) tại cổng 1111
                Listener = new TcpListener(IPAddress.Any, 1111);

                // 2. Bắt đầu lắng nghe kết nối đến
                Listener.Start();
                Log("Listening on port 1111...");

                // 3. Vòng lặp chính — server sẽ chạy mãi cho đến khi IsRunning = false
                while (IsRunning)
                {
                    try
                    {
                        // Kiểm tra xem có client nào đang yêu cầu kết nối hay không
                        if (Listener.Pending())
                        {
                            // Nếu có client đang kết nối thì chấp nhận kết nối
                            TcpClient Client = Listener.AcceptTcpClient();

                            Log("Client connected: " + Client.Client.RemoteEndPoint.ToString());

                            // Lưu client vào danh sách quản lý
                            Clients.Add(Client);

                            // Tạo thread riêng cho client này
                            Thread t = new Thread(() => HandleClient(Client));
                            t.Start();

                        }
                        else
                        {
                            // Nếu chưa có client kết nối, tạm dừng 100ms để không tốn CPU
                            Thread.Sleep(100);
                        }
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        Log("Error: " + ex.Message);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log("Lỗi: " + ex.Message);
            }
        }




        private void HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream ns = client.GetStream();
                byte[] buffer = new byte[4096];
                int bytesRead;

                while ((bytesRead = ns.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Log($"[{client.Client.RemoteEndPoint}] gửi: {message}");

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
                            default:
                                response = JsonSerializer.Serialize(new { status = "error", message = "Unknown action" });
                                break;
                        }

                        byte[] respBytes = Encoding.UTF8.GetBytes(response);
                        ns.Write(respBytes, 0, respBytes.Length);
                    }
                    catch (JsonException)
                    {
                        Log(" Dữ liệu từ client không phải JSON hợp lệ!");
                    }
                }
            }
            catch { }
            finally
            {
                Log($"Client {client.Client.RemoteEndPoint} đã ngắt kết nối.");
            }
        }

        private string HandleForgotPassword(Dictionary<string, string> data)
        {
            try
            {
                if (!data.ContainsKey("email"))
                {
                    return JsonSerializer.Serialize(new
                    {
                        status = "error",
                        message = "Thiếu email để khôi phục mật khẩu!"
                    });
                }

                string email = data["email"];
                var client = SupabaseHelper.Client;

                // Tìm người dùng có email khớp
                var result = client
                    .From<TaiKhoanNguoiDung>()
                    .Where(u => u.Email == email)
                    .Get()
                    .Result;

                if (result.Models.Count == 0)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status = "error",
                        message = "Email không tồn tại trong hệ thống!"
                    });
                }

                // Lấy thông tin người dùng
                var user = result.Models.First();
                string password = user.Password; // Lấy mật khẩu gốc từ DB

                // Gửi email
                bool sent = SendEmail(email, "Khôi phục mật khẩu",
                                      $"Xin chào {user.Username},\n\nMật khẩu của bạn là: {password}\n\n Vui lòng đổi mật khẩu mới!");

                if (sent)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status = "success",
                        message = "Mật khẩu đã được gửi đến email của bạn!"
                    });
                }
                else
                {
                    return JsonSerializer.Serialize(new
                    {
                        status = "error",
                        message = "Không thể gửi email. Vui lòng thử lại!"
                    });
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Lỗi xử lý forgot_password: " + ex.Message
                });
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
        private string HandleChangePassword(Dictionary<string, string> data)
        {
            try
            {
                string email = data["email"];
                string token = data["token"];
                string newPassword = data["new_password"];

                var client = SupabaseHelper.Client;
                var result = client.From<TaiKhoanNguoiDung>()
                                   .Where(u => u.Email == email)
                                   .Get().Result;

                if (result.Models.Count == 0)
                {
                    return JsonSerializer.Serialize(new { status = "error", message = "Email không tồn tại!" });
                }

                var user = result.Models.First();

                // Kiểm tra token có trùng với password (mã SHA256 đang lưu)
                if (user.Password != token)
                {
                    return JsonSerializer.Serialize(new { status = "error", message = "Mã xác thực không đúng!" });
                }

                // Nếu đúng, mã hóa mật khẩu mới rồi cập nhật
                string hashedNewPassword = MaHoa(newPassword);
                user.Password = hashedNewPassword;

                client.From<TaiKhoanNguoiDung>().Update(user).Wait();

                return JsonSerializer.Serialize(new
                {
                    status = "success",
                    message = "Đổi mật khẩu thành công!"
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Lỗi khi đổi mật khẩu: " + ex.Message
                });
            }
        }

        // Hàm mã hóa SHA256 (đặt cùng file)
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


        public frm_Server()
        {
            InitializeComponent();
        }

        private void btn_Start_Server_Click(object sender, EventArgs e)
        {
            if (!IsRunning)
            {
                // Bật cờ lên để server bắt đầu chạy
                IsRunning = true;

                // Tạo luồng riêng để chạy StartServer (nếu không, form sẽ bị đứng)
                ListenThread = new Thread(StartServer);

                // Bắt đầu luồng
                ListenThread.Start();
            }
            else
            {
                Log("Server is already running.");
            }
        }

        private void StopServer()
        {
            try
            {
                // Đặt cờ dừng lại → vòng while trong StartServer sẽ thoát
                IsRunning = false;
                if (Listener != null)
                {
                    // Dừng lắng nghe và đóng port
                    Listener.Stop();
                    Listener = null;
                }

                if (ListenThread != null && ListenThread.IsAlive)
                {
                    // Chờ tối đa 0.5 giây cho thread dừng hẳn
                    ListenThread.Join(500);
                    ListenThread = null;
                }
                Log("Server stopped.");
            }
            catch (Exception ex)
            {
                Log("Error stopping server: " + ex.Message);
            }
        }

        private void btn_End_Server_Click(object sender, EventArgs e)
        {
            if (IsRunning)
            {
                StopServer();
            }
            else
            {
                Log("Server is not running.");
            }
        }

        private async void ServerForm_Load(object sender, EventArgs e)
        {
            try
            {
                await SupabaseHelper.Initialize();
                Log("Supabase đã sẵn sàng!");
            }
            catch (Exception ex)
            {
                Log("Lỗi Supabase: " + ex.Message);
            }
        }


        //Xử lí nhận tín hiệu từ form đăng kí
        private string HandleRegister(Dictionary<string, string> data)
        {
            string username = data["username"];
            string email = data["email"];
            string password = data["password"];

            try
            {
                //Kết nối với database và kiểm tra username đã có chưa
                var client = SupabaseHelper.Client;
                var existing = client.From<TaiKhoanNguoiDung>().Where(u => u.Username == username).Get().Result;

                if (existing.Models.Count > 0)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status = "error",
                        message = "Tên người dùng đã tồn tại!"
                    });
                }

                var newUser = new TaiKhoanNguoiDung
                {
                    Username = username,
                    Email = email,
                    Password = password,
                    Birthday = DateTime.Now
                };

                client.From<TaiKhoanNguoiDung>().Insert(newUser).Wait();

                return JsonSerializer.Serialize(new
                {
                    status = "success",
                    message = "Đăng ký thành công!"
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Lỗi Supabase: " + ex.Message
                });
            }
        }

        //Xử lí tín hiệu từ form thông tin
        private string HandleGetInfo(Dictionary<string, string> data)
        {
            try
            {
                // Lấy username từ dữ liệu client gửi lên
                if (!data.ContainsKey("username"))
                {
                    return JsonSerializer.Serialize(new
                    {
                        status = "error",
                        message = "Thiếu tham số 'username'."
                    });
                }

                string username = data["username"];


                // Kết nối đến Supabase
                var client = SupabaseHelper.Client;

                // Truy vấn người dùng có username trùng khớp
                var result = client
                    .From<TaiKhoanNguoiDung>()
                    .Where(u => u.Email == username)
                    .Get()
                    .Result;

                // Nếu không tìm thấy người dùng
                if (result.Models.Count == 0)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status = "error",
                        message = "Không tìm thấy người dùng!"
                    });
                }

                // Lấy bản ghi đầu tiên
                var user = result.Models.First();

                // Trả về thông tin cơ bản
                return JsonSerializer.Serialize(new
                {
                    status = "success",
                    data = new
                    {
                        username = user.Username,
                        email = user.Email,
                        birthday = user.Birthday.ToString("yyyy-MM-dd")
                    }
                });
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Lỗi Supabase: " + ex.Message
                });
            }
        }

        //Xử lí nhận tín hiệu từ form đăng nhập
        private string HandleLogin(Dictionary<string, string> data)
        {
            string username = data["username"];
            string password = data["password"];

            try
            {
                //Kết nốidatatabase à kiểm tra xem thông tin có trùng khớp không
                var client = SupabaseHelper.Client;
                var result = client.From<TaiKhoanNguoiDung>().Where(u => u.Email == username && u.Password == password).Get().Result;

                if (result.Models.Count > 0)
                {
                    return JsonSerializer.Serialize(new
                    {
                        status = "success",
                        message = "Đăng nhập thành công!"
                    });
                }
                else
                {
                    return JsonSerializer.Serialize(new
                    {
                        status = "error",
                        message = "Sai tài khoản hoặc mật khẩu!"
                    });
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Lỗi Supabase: " + ex.Message
                });
            }
        }
    }
}
