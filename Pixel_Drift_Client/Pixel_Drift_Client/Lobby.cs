using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Lobby : Form
    {
        private string MyUsername;

        public Lobby(string username)
        {
            InitializeComponent();
            MyUsername = username;

            ClientManager.StartGlobalListening();

            ClientManager.OnMessageReceived += HandleServerMessage;
        }

        private void HandleServerMessage(string message)
        {
            if (this.Disposing || this.IsDisposed || !this.IsHandleCreated) return;

            this.Invoke(new Action(() =>
            {
                try
                {
                    var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(message);
                    string status = "";
                    if (data.ContainsKey("status")) status = data["status"].GetString();
                    else if (data.ContainsKey("action")) status = data["action"].GetString();

                    if (status == "create_room_success" || status == "join_room_success")
                    {
                        string roomID = data["room_id"].GetString();
                        int playerNum = data["player_number"].GetInt32();

                        Game_Window gameForm = new Game_Window(MyUsername, playerNum, roomID);

                        gameForm.FormClosed += (s, args) =>
                        {
                            if (!this.IsDisposed) this.Show();
                        };

                        this.Hide();
                        gameForm.Show();
                    }
                    else if (status == "force_logout")
                    {
                        string msg = data.ContainsKey("message") ? data["message"].GetString() : "Tài khoản đã đăng nhập nơi khác!";

                        MessageBox.Show(msg, "Cảnh báo Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        Application.Exit();
                    }
                    else if (status == "error")
                    {
                        MessageBox.Show(data["message"].GetString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch { }
            }));
        }

        private void btn_CreateRoom_Click(object sender, EventArgs e)
        {
            ClientManager.Send_And_Forget(new { action = "create_room", username = MyUsername });
        }

        private void btn_JoinRoom_Click(object sender, EventArgs e)
        {
            using (Form_ID inputForm = new Form_ID())
            {
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    ClientManager.Send_And_Forget(new { action = "join_room", room_id = inputForm.RoomID, username = MyUsername });
                }
            }
        }

        private void Lobby_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClientManager.OnMessageReceived -= HandleServerMessage; 
            Application.Exit();
        }

        private void btn_Scoreboard_Click(object sender, EventArgs e)
        {
            var sb = Application.OpenForms.OfType<Form_ScoreBoard>().FirstOrDefault();
            if (sb != null) sb.Show();
            else { new Form_ScoreBoard(ClientManager.GetClient()).Show(); }
        }
    }
}