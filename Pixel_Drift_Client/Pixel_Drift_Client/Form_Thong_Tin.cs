using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Pixel_Drift
{
    public partial class Form_Thong_Tin : Form
    {
        private TcpClient mainClient;
        private string currentUsername;

        public Form_Thong_Tin(TcpClient clientFromLogin,string username)
        {
            InitializeComponent();
            currentUsername = username;
            mainClient = clientFromLogin;
        }

        private void Form_Thong_Tin_Load(object sender, EventArgs e)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    var result = client.BeginConnect("172.16.16.187", 1111, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5));

                    if (!success)
                    {
                        MessageBox.Show("Không thể kết nối tới server để lấy thông tin!", "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    client.EndConnect(result);

                    using (NetworkStream stream = client.GetStream())
                    using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        stream.ReadTimeout = 5000; 

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

        private void btnVaoGame_Click(object sender, EventArgs e)
        {
            this.Hide();
            Lobby lobby = new Lobby(mainClient, currentUsername);
            lobby.ShowDialog();
        }
    }
}
