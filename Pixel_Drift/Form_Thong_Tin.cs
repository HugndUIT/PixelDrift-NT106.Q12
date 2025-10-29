using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;

namespace Pixel_Drift
{
    public partial class Form_Thong_Tin : Form
    {
        private string serverIP = "172.16.16.34";
        private int serverPort = 1111;
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

                string response = SendRequest(request);

                var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (dict.ContainsKey("status") && dict["status"].ToString() == "success")
                {
                    var dataJson = dict["data"].ToString();
                    var userData = JsonSerializer.Deserialize<Dictionary<string, string>>(dataJson);

                    lbl_TenDangNhap.Text += userData["username"];
                    lbl_Email.Text += userData["email"];
                    lbl_Birthday.Text += userData["birthday"];
                }
                else
                {
                    MessageBox.Show("Không thể tải thông tin người dùng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin: " + ex.Message);
            }
        }

        private string SendRequest(object data)
        {
            string json = JsonSerializer.Serialize(data);
            TcpClient client = new TcpClient();
            File.WriteAllText("GuiDi.json", json);

            client.Connect(serverIP, serverPort);

            using (NetworkStream ns = client.GetStream())
            {
                byte[] sendBytes = Encoding.UTF8.GetBytes(json);
                ns.Write(sendBytes, 0, sendBytes.Length);

                byte[] buffer = new byte[4096];
                int len = ns.Read(buffer, 0, buffer.Length);
                File.WriteAllText("Output.text", Encoding.UTF8.GetString(buffer, 0, len)); 
                return Encoding.UTF8.GetString(buffer, 0, len);
            }
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            Game_Window game = new Game_Window();
            game.Show();
        }
    }
}
