using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Windows.Forms;
using Pixel_Drift;
using System.IO;

namespace Pixel_Drift
{
    public partial class Form_Dang_Nhap : Form
    {
        private string serverIP = "127.0.0.1";   // IP máy chủ
        private int serverPort = 1111;              // Cổng TCP

        public Form_Dang_Nhap()
        {
            InitializeComponent();
        }

        // Hàm mã hóa SHA-256
        private string MaHoa(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        // Hàm gửi JSON đến server và nhận phản hồi
        private string SendRequest(object data)
        {
            string json = JsonSerializer.Serialize(data);
            File.WriteAllText("DangNhap.json", json); //Test xem nó gửi đi cái gì 

            using (TcpClient client = new TcpClient())
            {

                client.Connect(serverIP, serverPort);
                NetworkStream ns = client.GetStream();

                // Gửi JSON request
                byte[] sendBytes = Encoding.UTF8.GetBytes(json);
                ns.Write(sendBytes, 0, sendBytes.Length);


                // Nhận phản hồi từ server
                byte[] buffer = new byte[4096];
                int len = ns.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, len);


                return response;
            }
        }

        private void btn_vaogame_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Mã hóa mật khẩu
                string hashedPassword = MaHoa(password);

                // Gửi yêu cầu login qua TCP
                var request = new
                {
                    action = "login",
                    username = username,
                    password = hashedPassword
                };

                string response = SendRequest(request);
                Console.WriteLine("Server response: " + response); // debug

                // Phân tích phản hồi JSON
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

                if (dict.ContainsKey("status") && dict["status"] == "success")
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //  Ẩn form đăng nhập hiện tại
                    this.Hide();

                    //  Tạo và mở form thông tin
                    Form_Thong_Tin formThongTin = new Form_Thong_Tin(username);

                    formThongTin.ShowDialog();

                    //  Sau khi đóng form thông tin -> thoát form đăng nhập
                    this.Close();
                }

                else
                {
                    string msg = dict.ContainsKey("message") ? dict["message"] : "Sai tài khoản hoặc mật khẩu!";
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (JsonException)
            {
                MessageBox.Show("Dữ liệu từ server không hợp lệ (không phải JSON).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SocketException)
            {
                MessageBox.Show("Không thể kết nối tới server. Hãy kiểm tra IP và cổng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_quenmatkhau_Click(object sender, EventArgs e)
        {
            
            Form_QuenMatKhau form = new Form_QuenMatKhau();
            form.ShowDialog();
            this.Hide();
        }
    }
}
