using System;
using System.Collections.Generic;
using System.Net.Sockets; 
using System.Text.Json;
using System.Windows.Forms;


namespace Pixel_Drift
{
    public partial class Form_QuenMatKhau : Form
    {
        public Form_QuenMatKhau()
        {
            InitializeComponent();
        }


        private void btn_quenmatkhau_Click(object sender, EventArgs e)
        {
            string email = txt_email.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập email của bạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                if (!ClientManager.IsConnected)
                {

                    if (!ClientManager.Connect("127.0.0.1", 1111))
                    {
                        throw new SocketException();
                    }
                }

                var request = new
                {
                    action = "forgot_password",
                    email = email
                };

                string response = ClientManager.SendRequest(request);

                if (response == null)
                {
                    throw new SocketException(); 
                }

                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

                if (dict.ContainsKey("status") && dict["status"] == "success")
                {
                    MessageBox.Show(dict["message"], "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 
                    Form_Doi_Mat_Khau formDoi = new Form_Doi_Mat_Khau(email);
                    formDoi.Show();
                    this.Close(); 
                }
                else
                {
                    string msg = dict.ContainsKey("message") ? dict["message"] : "Không thể gửi mật khẩu!";
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Không thể kết nối đến server. Kiểm tra IP và cổng!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btn_quaylai_Click(object sender, EventArgs e)
        {
            Form_Dang_Nhap form = new Form_Dang_Nhap();
            form.Show();
            this.Close();
        }
    }
}