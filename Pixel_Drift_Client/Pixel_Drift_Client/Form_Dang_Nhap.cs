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
                if (!ClientManager.Connect("127.0.0.1", 1111))
                {
                    MessageBox.Show("Không thể kết nối tới server. Hãy kiểm tra IP và cổng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string hashedPassword = MaHoa(password);

                var request = new
                {
                    action = "login",
                    username = username,
                    password = hashedPassword
                };

                string response = ClientManager.SendRequest(request);
                Console.WriteLine("Server response: " + response); 

                // Phân tích phản hồi JSON
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

                if (dict.ContainsKey("Status") && dict["Status"] == "success")
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    Form_Thong_Tin formThongTin = new Form_Thong_Tin(username);
                    formThongTin.ShowDialog();
                    this.Close();
                }
                else
                {
                    string msg = dict.ContainsKey("message") ? dict["message"] : "Sai tài khoản hoặc mật khẩu!";
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ClientManager.CloseConnection();
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
