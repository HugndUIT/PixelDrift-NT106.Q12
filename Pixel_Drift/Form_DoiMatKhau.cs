using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Form_DoiMatKhau : Form
    {
        private string serverIP = "192.168.43.174"; // IP máy chủ
        private int serverPort = 1111;              // Cổng TCP
        private string userEmail;                   // Email được truyền từ form Quên mật khẩu

        public Form_DoiMatKhau(string email)
        {
            InitializeComponent();
            userEmail = email;
        }

        private string SendRequest(object data)
        {
            string json = JsonSerializer.Serialize(data);

            using (TcpClient client = new TcpClient())
            {
                client.Connect(serverIP, serverPort);
                NetworkStream ns = client.GetStream();

                byte[] sendBytes = Encoding.UTF8.GetBytes(json);
                ns.Write(sendBytes, 0, sendBytes.Length);

                byte[] buffer = new byte[4096];
                int len = ns.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, len);

                return response;
            }
        }

        private void btn_doimk_Click(object sender, EventArgs e)
        {
            string token = txt_mkcu.Text.Trim();
            string newPass = txt_mkmoi.Text.Trim();
            string confirm = txt_xacnhanmk.Text.Trim();

            if (token == "" || newPass == "" || confirm == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPass != confirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không trùng khớp!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var request = new
                {
                    action = "change_password",
                    email = userEmail,
                    token = token,
                    new_password = newPass
                };

                string response = SendRequest(request);
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

                if (dict.ContainsKey("status") && dict["status"] == "success")
                {
                    MessageBox.Show(dict["message"], "Thành công",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Ẩn form đổi mật khẩu
                    this.Hide();

                    // Mở lại form đăng nhập
                    Form_Dang_Nhap formDangNhap = new Form_Dang_Nhap();
                    formDangNhap.ShowDialog();

                    // Sau khi form đăng nhập đóng thì đóng luôn form đổi mật khẩu
                    this.Close();
                }

                else
                {
                    string msg = dict.ContainsKey("message") ? dict["message"] : "Đổi mật khẩu thất bại!";
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Không thể kết nối đến server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form_Dang_Nhap formdangnhap = new Form_Dang_Nhap();
        }

      
    }
}
