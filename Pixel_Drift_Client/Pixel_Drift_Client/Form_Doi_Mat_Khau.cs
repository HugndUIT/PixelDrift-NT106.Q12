using System;
using System.Collections.Generic;
using System.Net.Sockets; 
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Pixel_Drift
{
    public partial class Form_Doi_Mat_Khau : Form
    {
        private string userEmail;

        public Form_Doi_Mat_Khau(string email)
        {
            InitializeComponent();
            userEmail = email;
        }


        private void btn_doimk_Click(object sender, EventArgs e)
        {
            string token = txt_mkcu.Text.Trim();
            string newPass = txt_mkmoi.Text.Trim();
            string confirm = txt_xacnhanmk.Text.Trim();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirm))
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

            if (!ClientManager.IsConnected)
            {
                MessageBox.Show("Mất kết nối đến server! Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Mã hóa mật khẩu mới trước khi gửi 
                string encryptedNewPassword = MaHoa(newPass);

                var request = new
                {
                    action = "change_password",
                    email = userEmail,
                    token = token,
                    new_password = encryptedNewPassword // Gửi mật khẩu đã mã hóa
                };

                string response = ClientManager.SendRequest(request);

                if (response == null)
                {
                    throw new SocketException();
                }

                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

                if (dict.ContainsKey("Status") && dict["Status"] == "success")
                {
                    MessageBox.Show(dict["Message"], "Thành công",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    Form_Dang_Nhap formDangNhap = new Form_Dang_Nhap();
                    formDangNhap.ShowDialog();
                    this.Close();
                }
                else
                {
                    string msg = dict.ContainsKey("Message") ? dict["Message"] : "Đổi mật khẩu thất bại!";
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Không thể kết nối hoặc mất kết nối đến server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException)
            {
                MessageBox.Show("Phản hồi từ server không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form_Dang_Nhap formdangnhap = new Form_Dang_Nhap();
            // Đã sửa: Phải ShowDialog()
            formdangnhap.ShowDialog();
            this.Close();
        }
    }
}