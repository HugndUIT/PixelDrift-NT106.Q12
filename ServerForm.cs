using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Thêm thư viện để làm việc với mạng và luồng
using System.Net;
using System.Net.Sockets;
using System.Threading;

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

        private void tb_hienthi_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_nhaptinnhan_TextChanged(object sender, EventArgs e)
        {

        }

        private void lb_danhsachclient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
