using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Pixel_Drift
{
    public partial class Lobby : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private string MyUsername;

        public Lobby(TcpClient existingClient, string username)
        {
            InitializeComponent();
            client = existingClient;
            MyUsername = username;
            stream = client.GetStream();
            reader = new StreamReader(stream,Encoding.UTF8);

            Task.Run(() => ListenForMessage());
        }

        private async Task ListenForMessage()
        {
            try
            {
                string message;
                while ((message = await reader.ReadLineAsync()) != null)
                {
                    this.Invoke(new Action(() => ProcessMessage(message)));
                }
            }
            catch { }
        }

        private void ProcessMessage(string message)
        {
            try
            {
                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(message);
                if (!data.ContainsKey("action")) return;

                string action = data["action"].GetString();

                if (action == "create_room_succes" || action == "join_room_success")
                {
                    string roomID = data["room_id"].GetString();
                    int playerNum = data["player_number"].GetInt32();

                    Game_Window gameForm = new Game_Window(client, MyUsername, playerNum, roomID);

                    this.Hide();

                    gameForm.Show();
                }
                else if (action == "force_logout") 
                {
                    string logoutMsg = data["message"].GetString();
                    MessageBox.Show(logoutMsg, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
                else if (data.ContainsKey("status") && data["status"].GetString() == "error")
                {
                    MessageBox.Show(data["message"].GetString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) { Console.WriteLine("Lỗi: " + ex.Message); }
        }

        private void btn_CreateRoom_Click(object sender, EventArgs e)
        {
            var req = new { action = "create_room", username = MyUsername };
            Send(JsonSerializer.Serialize(req));
        }

        private void Send(string v)
        {
            if (client == null || !client.Connected) return;
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(v + "\n");
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
            catch { MessageBox.Show("Mất kết nối!"); }
        }

        private void btn_JoinRoom_Click(object sender, EventArgs e)
        {
            using (Form_ID inputForm = new Form_ID())
            {
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    string roomID = inputForm.RoomID;
                    if (string.IsNullOrEmpty(roomID)) return;

                    var req = new { action = "join_room", room_id = roomID, username = MyUsername };
                    Send(JsonSerializer.Serialize(req));
                }
            }
        }
    }
}
