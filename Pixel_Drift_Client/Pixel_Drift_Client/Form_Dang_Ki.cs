using Pixel_Drift;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Form_Dang_Ki : Form
    {
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
            string birthday = tb_BirthDay.Text.Trim();

            // Kiểm tra dữ liệu đầu vào
            if (username == "" || password == "" || email == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            birthday = DinhDangNgay(birthday);
            if (birthday == null)
            {
                MessageBox.Show("Nhập sai định dạng ngày sinh nhật");
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
                string response = await SendRegisterRequest(username, email, hashedPassword, birthday);

                // Phân tích phản hồi JSON
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

                if (dict.ContainsKey("status") && dict["status"] == "success")
                {
                    DialogResult result = MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (result == DialogResult.OK)
                    {
                        Pixel_Drift.Form_Dang_Nhap formDangNhap = new Pixel_Drift.Form_Dang_Nhap();
                        formDangNhap.Show();
                        this.Close();
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

        //Định dạng ngày sinh nhật
        private string DinhDangNgay(string day)
        {
            if (DateTime.TryParse(day, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime parsedDay))
                return parsedDay.ToString("yyyy-MM-dd");
            return null;
        }

        private async Task<string> SendRegisterRequest(string username, string email, string hashedPassword, string birthday)
        {
            // Dùng Task.Run để không làm treo giao diện
            return await Task.Run(() =>
            {
                // Mở kết nối
                if (!ClientManager.IsConnected)
                {
                    string ip = ClientManager.Get_Server_IP();

                    if (string.IsNullOrEmpty(ip)) ip = "127.0.0.1";

                    if (!ClientManager.Connect(ip, 1111))
                    {
                        var error = new { status = "error", message = "Không thể kết nối đến server." };
                        return JsonSerializer.Serialize(error);
                    }
                }

                var data = new
                {
                    action = "register",
                    email = email,
                    username = username,
                    password = hashedPassword,
                    birthday = birthday
                };

                return ClientManager.Send_And_Wait(data);
            });
        }

        private void btn_backdn_Click(object sender, EventArgs e)
        {
            Form_Dang_Nhap dn = Application.OpenForms.OfType<Form_Dang_Nhap>().FirstOrDefault();

            if (dn != null)
            {
                dn.Show(); 
            }
            else
            {
                dn = new Form_Dang_Nhap();
                dn.Show();
            }
            this.Hide();
        }
    }
}
