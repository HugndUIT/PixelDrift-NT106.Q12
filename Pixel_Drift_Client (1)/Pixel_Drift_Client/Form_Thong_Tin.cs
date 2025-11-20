using System;
using System.Collections.Generic;
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

        private async void Form_Thong_Tin_Load(object sender, EventArgs e)
        {
            try
            {
                var request = new
                {
                    action = "get_info",
                    username = currentUsername 
                };

                string response = await Task.Run(() => ClientManager.SendRequest(request));

                if (string.IsNullOrEmpty(response))
                {
                    MessageBox.Show("Không nhận được phản hồi từ server.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (dict.ContainsKey("Status") && dict["Status"].ToString() == "success")
                {
                    // Dùng (JsonElement) để lấy object "data"
                    var userData = (JsonElement)dict["Data"];

                    lbl_TenDangNhap.Text =  userData.GetProperty("Username").GetString();
                    lbl_Email.Text =   userData.GetProperty("Email").GetString();
                    lbl_Birthday.Text =   userData.GetProperty("Birthday").GetString();
                }
                else
                {
                    string errorMessage = dict.ContainsKey("message") ? dict["message"].ToString() : "Không thể tải thông tin.";
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (JsonException)
            {
                MessageBox.Show("Dữ liệu từ server không hợp lệ (không phải JSON).", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin: " + ex.Message, "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            Game_Window gameWindow = new Game_Window();
            gameWindow.Show();
            this.Close();
        }
    }
}