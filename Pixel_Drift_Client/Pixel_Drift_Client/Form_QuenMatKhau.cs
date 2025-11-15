using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
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

        private string SendRequest(object data)
        {
            string json = JsonSerializer.Serialize(data);

            using (TcpClient client = new TcpClient())
            {
                client.Connect("127.0.0.1", 1111);
                NetworkStream ns = client.GetStream();

                byte[] sendBytes = Encoding.UTF8.GetBytes(json);
                ns.Write(sendBytes, 0, sendBytes.Length);

                byte[] buffer = new byte[4096];
                int len = ns.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, len);

                return response;
            }
        }

        private void btn_quenmatkhau_Click(object sender, EventArgs e)
        {
            string email = txt_email.Text.Trim();

            if (email == "")
            {
                MessageBox.Show("Vui lòng nhập email của bạn!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var request = new
                {
                    action = "forgot_password",
                    email = email
                };

                string response = SendRequest(request);
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

                if (dict.ContainsKey("status") && dict["status"] == "success")
                {
                    MessageBox.Show(dict["message"], "Thành công",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    Form_Doi_Mat_Khau formDoi = new Form_Doi_Mat_Khau(txt_email.Text.Trim());
                    formDoi.Show();
                    this.Hide();
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_quaylai_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
