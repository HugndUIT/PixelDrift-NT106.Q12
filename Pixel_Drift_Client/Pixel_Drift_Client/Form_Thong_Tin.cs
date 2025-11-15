using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks; // Vẫn cần cho Task.Run
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Form_Thong_Tin : Form
    {
        // XÓA serverIP và serverPort
        private string currentUsername; // Đây là email/username dùng để đăng nhập

        public Form_Thong_Tin(string username)
        {
            InitializeComponent();
            currentUsername = username;
        }

        // 1. Giữ "async void" để không làm treo UI
        private async void Form_Thong_Tin_Load(object sender, EventArgs e)
        {
            try
            {
                var request = new
                {
                    action = "get_info",
                    username = currentUsername // Gửi email đã đăng nhập thành công
                };

                // 2. Gọi ClientManager.SendRequest
                // Dùng "Task.Run" để chạy hàm SendRequest (đồng bộ)
                // trên một luồng khác (bất đồng bộ), không làm treo UI.
                string response = await Task.Run(() => ClientManager.SendRequest(request));

                // 3. Khi có kết quả, code chạy tiếp TỰ ĐỘNG TRÊN LUỒNG UI
                if (string.IsNullOrEmpty(response))
                {
                    MessageBox.Show("Không nhận được phản hồi từ server.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Parse JSON
                // Sửa Deserialize: Server của bạn trả về Dictionary<string, string> cho lỗi
                // nhưng trả về Dictionary<string, object> cho success (vì có "data" là object)
                var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (dict.ContainsKey("status") && dict["status"].ToString() == "success")
                {
                    // Dùng (JsonElement) để lấy object "data"
                    var userData = (JsonElement)dict["data"];

                    // Cập nhật UI an toàn, không cần Invoke
                    lbl_TenDangNhap.Text = "Username: " + userData.GetProperty("username").GetString();
                    lbl_Email.Text = "Email: " + userData.GetProperty("email").GetString();
                    lbl_Birthday.Text = "Birthday: " + userData.GetProperty("birthday").GetString();
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
                // Bắt các lỗi khác, ví dụ ClientManager báo "Mất kết nối tới server."
                MessageBox.Show("Lỗi khi tải thông tin: " + ex.Message, "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 4. XÓA BỎ HOÀN TOÀN HÀM "SendRequestAsync"
        // private async Task<string> SendRequestAsync(object data) { ... }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            // Nơi này để mở Form Game chính
            // Viết code mở Form Game ở đây...

            // Sau đó đóng Form thông tin
            this.Close();
        }
    }
}