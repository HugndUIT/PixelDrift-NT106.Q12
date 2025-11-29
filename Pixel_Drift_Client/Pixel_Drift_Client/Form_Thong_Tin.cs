using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Text;

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
                using (TcpClient client = new TcpClient())
                {
                    // Timeout 5 giây khi kết nối
                    var result = client.BeginConnect("127.0.0.1", 1111, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5));

                    if (!success)
                    {
                        MessageBox.Show("Không thể kết nối tới server để lấy thông tin!",
                            "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    client.EndConnect(result);

                    using (NetworkStream stream = client.GetStream())
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        stream.ReadTimeout = 5000; // 5 giây timeout

                        var request = new
                        {
                            action = "get_info",
                            username = currentUsername
                        };

                        string json = JsonSerializer.Serialize(request);
                        writer.WriteLine(json);

                        string response = reader.ReadLine();

                        if (string.IsNullOrEmpty(response))
                        {
                            MessageBox.Show("Server không phản hồi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(response);

                        if (dict.ContainsKey("Status"))
                        {
                            string status = dict["Status"].GetString();
                            if (status == "success")
                            {
                                // Lấy dữ liệu trực tiếp từ JSON, không qua property "Data"
                                lbl_TenDangNhap.Text = dict.ContainsKey("username") ? dict["username"].GetString() : "N/A";
                                lbl_Email.Text = dict.ContainsKey("email") ? dict["email"].GetString() : "N/A";
                                lbl_Birthday.Text = dict.ContainsKey("birthday") ? dict["birthday"].GetString() : "N/A";
                            }
                            else
                            {
                                string errorMessage = dict.ContainsKey("Message") ? dict["Message"].GetString() : "Không thể tải thông tin.";
                                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Phản hồi từ server không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Server chưa sẵn sàng",
                    "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            Game_Window gameWindow = new Game_Window();
            gameWindow.Show();
            this.Close();
        }
    }
}
