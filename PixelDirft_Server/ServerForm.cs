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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PixelDirft_Server
{
    public partial class ServerForm : Form
    {
        // Đối tượng dùng để lắng nghe (listen) kết nối TCP từ các client
        private TcpListener Listener;
        // Luồng (thread) riêng để chạy server — giúp giao diện không bị đơ khi server đang chạy
        private Thread ListenThread;
        // Biến cờ (flag) dùng để kiểm tra server đang bật hay tắt
        private bool IsRunning = false;
        // Danh sách các client đã kết nối đến server
        private List<TcpClient> Clients = new List<TcpClient>();

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
                            lock (t)
                            {
                                ClientThreads.Add(t);
                            }
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

        private List<Thread> ClientThreads = new List<Thread>();

        private void HandleClient(TcpClient client)
        {
            try
            {
                //Mở luồng giao tiếp để thực hiện đọc ghi qua TCP
                NetworkStream ns = client.GetStream();
                byte[] buffer = new byte[4096];
                int bytesRead;
                //Đọc và giải mã dữ liệu nhận được từ client
                while ((bytesRead = ns.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Log($"[{client.Client.RemoteEndPoint}] gửi: {message}");

                    try
                    {
                        //Giải mã json gửi từ client để lấy được tín hiệu "action" để phục vụ cho các thao tác tiếp theo
                        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(message);
                        string action = data.ContainsKey("action") ? data["action"] : "";

                        string response;
                        //Xác định xem yêu cầu nhận được là đăng nhập, đăng kí hay lấy thông tin
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

                            default:
                                response = JsonSerializer.Serialize(new { status = "error", message = "Unknown action" });
                                break;
                        }
                        //Gửi phản hồi về lại cho client
                        byte[] respBytes = Encoding.UTF8.GetBytes(response);
                        ns.Write(respBytes, 0, respBytes.Length);
                    }
                    catch (JsonException)
                    {
                        Log("Dữ liệu từ client không phải JSON hợp lệ!");
                    }
                }
            }
            catch { }
            finally
            {
                Log($"Client {client.Client.RemoteEndPoint} đã ngắt kết nối.");
            }
        }

        private string HandleGetInfo(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("username"))
            {
                return JsonSerializer.Serialize(new { status = "error", message = "Thiếu tham số username!" });
            }

            string username = data["username"];

            try
            {
                var client = SupabaseHelper.Client;
                var result = client.From<TaiKhoanNguoiDung>().Where(u => u.Username == username).Get().Result;

                if (result.Models.Count > 0)
                {
                    var user = result.Models[0];

                    return JsonSerializer.Serialize(new
                    {
                        status = "success",
                        data = new
                        {
                            Username = user.Username,
                            Email = user.Email,
                            Birthday = user.Birthday.ToString("yyyy-MM-dd")
                        }
                    });
                }
                else
                {
                    return JsonSerializer.Serialize(new { status = "error", message = "Không tìm thấy người dùng!" });
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new { status = "error", message = "Lỗi Supabase: " + ex.Message });
            }
        }

        public ServerForm()
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

        private void btn_Send_All_Click(object sender, EventArgs e)
        {
            // Lấy nội dung
            string Message = tb_nhaptinnhan.Text;
            // Không có gì thì báo lỗi
            if (string.IsNullOrEmpty(Message))
            {
                MessageBox.Show("Không có nội dung gì để gửi");
                return;
            }

            // Chuyển nội dung qua nhị phân
            byte[] Data = Encoding.UTF8.GetBytes(Message);

            foreach (var Client in Clients)
            {
                try
                {
                    // Gửi qua stream cho từng Client
                    NetworkStream ns = Client.GetStream();
                    ns.Write(Data, 0, Data.Length);
                }
                catch
                {
                }
            }
            Log("Đã gửi broadcast: " + Message);
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

        //Xử lí nhận tín hiệu từ form đăng nhập
        private string HandleLogin(Dictionary<string, string> data)
        {
            string username = data["username"];
            string password = data["password"];

            try
            {
                //Kết nối datatabase và kiểm tra xem thông tin có trùng khớp không
                var client = SupabaseHelper.Client;
                var result = client.From<TaiKhoanNguoiDung>().Where(u => u.Email == username && u.Password == password).Get().Result;
                //Nếu trùng khớp thì thông báo thành công
                if (result.Models.Count > 0)
                {
                    var user = result.Models[0];
                    return JsonSerializer.Serialize(new
                    {
                        status = "success",
                        message = "Đăng nhập thành công!",
                        data = new
                        {
                            Username = user.Username,
                            Email = user.Email,
                            Birthday = user.Birthday.ToString("yyyy-MM-dd")
                        }
                    });
                }
                //Nếu không trùng thì báo thông tin chưa đúng
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

        private void tb_nhaptinnhan_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
