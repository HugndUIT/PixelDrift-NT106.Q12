using Microsoft.Office.SharePoint.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Form_Thong_Tin : Form
    {
        private string currentUsername;

        public Form_Thong_Tin(string username)
        {
            InitializeComponent();
            currentUsername = username;
        }

        private void Form_Thong_Tin_Load(object sender, EventArgs e)
        {
            try
            {
                var request = new
                {
                    action = "get_info",
                    username = currentUsername
                };

                string response = ClientManager.Send_And_Wait(request);

                if (string.IsNullOrEmpty(response))
                {
                    MessageBox.Show("Server không phản hồi!");
                    return;
                }

                if (string.IsNullOrEmpty(response))
                {
                    MessageBox.Show("Server không phản hồi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(response);

                if (dict.ContainsKey("status"))
                {
                    string status = dict["status"].GetString();
                    if (status == "success")
                    {
                        lbl_TenDangNhap.Text = dict.ContainsKey("username") ? dict["username"].GetString() : "N/A";
                        lbl_Email.Text = dict.ContainsKey("email") ? dict["email"].GetString() : "N/A";
                        lbl_Birthday.Text = dict.ContainsKey("birthday") ? dict["birthday"].GetString() : "N/A";
                    }
                    else
                    {
                        MessageBox.Show("Không thể tải thông tin.");
                    }
                }
                else
                {
                    MessageBox.Show("Phản hồi từ server không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Server chưa sẵn sàng", "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Dữ liệu từ server không hợp lệ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVaoGame_Click(object sender, EventArgs e)
        {
            Lobby lobby = new Lobby(currentUsername);
            lobby.Show();
            this.Hide();
        }

        private void Form_Thong_Tin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
