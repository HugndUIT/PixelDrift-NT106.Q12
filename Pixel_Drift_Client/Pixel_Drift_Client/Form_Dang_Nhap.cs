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
        public static string Current_Username = "";
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
                TcpClient client = new TcpClient();
                
                var result = client.BeginConnect("127.0.0.1", 1111, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5));

                if (!success)
                {
                    MessageBox.Show("Server chưa sẵn sàng", "Mất kết nối server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                client.EndConnect(result);

                NetworkStream stream = client.GetStream();
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                {
                    string hashedPassword = MaHoa(password);

                    var request = new
                    {
                        action = "login",
                        username = username,
                        password = hashedPassword
                    };

                    string json = JsonSerializer.Serialize(request);
                    writer.WriteLine(json);

                    stream.ReadTimeout = 5000; 
                    string response = reader.ReadLine();

                    if (string.IsNullOrEmpty(response))
                    {
                        MessageBox.Show("Server không phản hồi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

                    if (dict.ContainsKey("status") && dict["status"] == "success")
                    {
                        MessageBox.Show("Đăng nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form_Dang_Nhap.Current_Username = username;
                        this.Hide();
                        Form_Thong_Tin formThongTin = new Form_Thong_Tin(client,username);
                        formThongTin.ShowDialog();
                    }
                    else if (dict.ContainsKey("status") && dict["status"] == "force_logout")
                    {
                        string msg = dict.ContainsKey("message") ? dict["message"] : "Tài khoản đang được đăng nhập ở nơi khác. Vui lòng thử lại sau";
                        MessageBox.Show(msg, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string msg = dict.ContainsKey("message") ? dict["message"] : "Sai tài khoản hoặc mật khẩu!";
                        MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            
            catch (SocketException)
            {
                MessageBox.Show("Server chưa sẵn sàng", "Mất kết nối server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException)
            {
                MessageBox.Show("Dữ liệu từ server không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_quenmatkhau_Click(object sender, EventArgs e)
        {
            Form_QuenMatKhau form = new Form_QuenMatKhau();
            form.Show();
            this.Close();
        }

        private void btn_backdk_Click(object sender, EventArgs e)
        {
            Form_Dang_Ki form = new Form_Dang_Ki();
            form.Show();
            this.Close();
        }
    }
}
