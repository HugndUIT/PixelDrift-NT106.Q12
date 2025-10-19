using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Form_Dang_Ki : Form
    {
        // Địa chỉ IP và cổng của server TCP 
        private const string SERVER_IP = "172.16.16.33";
        private const int SERVER_PORT = 1111;  // trùng với server của bạn

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
            string emailsdt = tb_emailsdt.Text.Trim();

            // Kiểm tra dữ liệu đầu vào
            if (username == "" || password == "" || emailsdt == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isEmail = Regex.IsMatch(emailsdt, @"^[a-zA-Z0-9._%+-]+@gmail\.com$");
            bool isPhone = Regex.IsMatch(emailsdt, @"^(0[0-9]{9})$");
            if (!isEmail && !isPhone)
            {
                MessageBox.Show("Email hoặc số điện thoại không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                string response = await SendRegisterRequest(username, emailsdt, hashedPassword);

                // Xử lý phản hồi
                if (response.StartsWith("SUCCESS"))
                {
                    MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(response, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối đến server: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Gửi yêu cầu đăng ký qua TCP
        private async Task<string> SendRegisterRequest(string username, string email, string hashedPassword)
        {
            return await Task.Run(() =>
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(SERVER_IP, SERVER_PORT);
                    NetworkStream stream = client.GetStream();

                    // Tạo gói tin: REGISTER|username|email|password_hash
                    string message = $"REGISTER|{username}|{email}|{hashedPassword}";
                    byte[] data = Encoding.UTF8.GetBytes(message);

                    // Gửi dữ liệu lên server
                    stream.Write(data, 0, data.Length);

                    // Đọc phản hồi
                    byte[] buffer = new byte[1024];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytes);

                    return response;
                }
            });
        }
    }
}
