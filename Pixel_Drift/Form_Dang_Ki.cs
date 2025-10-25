using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms;
using  Pixel_Drift;

namespace Pixel_Drift
{
    public partial class Form_Dang_Ki : Form
    {
        // Địa chỉ IP và cổng của server TCP 
        private const string SERVER_IP = "172.16.16.34";
        private const int SERVER_PORT = 1111;  // trùng với server

        public Form_Dang_Ki()
        {
            InitializeComponent();
        }

        // Hàm mã hoá mật khẩu bằng SHA256
        private string MaHoa(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        // Kiểm tra độ mạnh của mật khẩu
        private bool KiemTraDoManhMatKhau(string password)
        {
            if (password.Length < 8) return false;
            bool coChuHoa = Regex.IsMatch(password, "[A-Z]");
            bool coChuThuong = Regex.IsMatch(password, "[a-z]");
            bool coSo = Regex.IsMatch(password, "[0-9]");
            bool coKyTuDacBiet = Regex.IsMatch(password, @"[@$!%*?&#]");

            return coChuHoa && coChuThuong && coSo && coKyTuDacBiet;
        }

        private async void btn_xacnhan_Click(object sender, EventArgs e)
        {
            string username = tb_tendangnhap.Text.Trim();
            string password = tb_matkhau.Text.Trim();
            string confirmpass = tb_xacnhanmk.Text.Trim();
            string email = tb_emailsdt.Text.Trim();
        
            // Kiểm tra dữ liệu đầu vào
            if (username == "" || password == "" || email == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        
            bool isEmail = Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$");
            if (!isEmail)
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        
            if (!KiemTraDoManhMatKhau(password))
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        
            if (password != confirmpass)
            {
                MessageBox.Show("Mật khẩu không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        
            // Mã hóa mật khẩu
            string hashedPassword = MaHoa(password);
        
            try
            {
                string response = await SendRegisterRequest(username, email, hashedPassword);
        
                // Phân tích phản hồi JSON
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(response);
        
                if (dict.ContainsKey("status") && dict["status"] == "success")
                {
                    DialogResult result = MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
                    // Sau khi bấm OK, chuyển qua form đăng nhập (nằm ở namespace khác)
                    if (result == DialogResult.OK)
                    {
                        this.Hide(); // ẩn form đăng ký
                        Pixel_Drift.Form_Dang_Nhap formDangNhap = new Pixel_Drift.Form_Dang_Nhap();
                        formDangNhap.ShowDialog(); // hiển thị form đăng nhập
                        this.Close(); // đóng form đăng ký
                    }
                }
        
                else
                {
                    string msg = dict.ContainsKey("message") ? dict["message"] : "Đăng ký thất bại!";
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (JsonException)
            {
                MessageBox.Show("Dữ liệu phản hồi từ server không hợp lệ (không phải JSON).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SocketException)
            {
                MessageBox.Show("Không thể kết nối đến server. Kiểm tra IP và cổng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Gửi yêu cầu đăng ký qua TCP (định dạng JSON)
        private async Task<string> SendRegisterRequest(string username, string email, string hashedPassword)
        {
            return await Task.Run(() =>
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(SERVER_IP, SERVER_PORT);
                    NetworkStream stream = client.GetStream();

                    // Tạo JSON request
                    var data = new
                    {
                        action = "register",
                        username = username,
                        email = email,
                        password = hashedPassword
                    };

                    string json = JsonSerializer.Serialize(data);
                    byte[] sendBytes = Encoding.UTF8.GetBytes(json);
                    stream.Write(sendBytes, 0, sendBytes.Length);

                    // Nhận phản hồi từ server
                    byte[] buffer = new byte[4096];
                    int len = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, len);

                    return response;
                }
            });
        }
    }
}
