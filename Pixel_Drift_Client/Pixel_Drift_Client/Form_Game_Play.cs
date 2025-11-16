using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Pixel_Drift
{
    public partial class Game_Window : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private int myPlayerNumber = 0;
        private string myUsername = "Player";

        public Game_Window()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Game_Window_Load(object sender, EventArgs e)
        {
            // Thiết lập giao diện phòng chờ
            ResetToLobby();
            // Kết nối và lắng nghe
            try
            {
                client = new TcpClient();
                client.Connect("127.0.0.1", 1111);
                stream = client.GetStream();
                reader = new StreamReader(stream, Encoding.UTF8);

                Task.Run(() => ListenForServerMessages());

                var joinRequest = new { action = "join_lobby", username = myUsername };
                Send(JsonSerializer.Serialize(joinRequest));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể kết nối đến server: {ex.Message}");
                this.Close();
            }
        }

        // Gửi tin nhắn (Tự thêm \n)
        private void Send(string message)
        {
            if (stream == null || !stream.CanWrite) return;
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message + "\n");
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi gửi: {ex.Message}");
            }
        }

        // Luồng lắng nghe (Dùng StreamReader)
        private async Task ListenForServerMessages()
        {
            string message;
            try
            {
                while ((message = await reader.ReadLineAsync()) != null)
                {
                    this.Invoke(new Action(() => ProcessServerMessage(message)));
                }
            }
            catch (Exception)
            {
                if (this.IsHandleCreated)
                    this.Invoke(new Action(() => {
                        MessageBox.Show("Mất kết nối server.");
                        ResetToLobby();
                    }));
            }
        }

        private void ProcessServerMessage(string message)
        {
            try
            {
                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(message);
                if (!data.ContainsKey("action")) return;

                string action = data["action"].GetString();
                switch (action)
                {
                    case "assign_player":
                        myPlayerNumber = data["player_number"].GetInt32();
                        this.Text = $"Pixel Drift - PLAYER {myPlayerNumber} ({(myPlayerNumber == 1 ? "Xe Đỏ" : "Xe Xanh")})";
                        break;

                    case "update_ready_status":
                        lbl_P1_Status.Text = $"Player 1 ({data["player1_name"].GetString()}): {(data["player1_ready"].GetBoolean() ? "Sẵn sàng" : "Chưa sẵn sàng")}";
                        lbl_P2_Status.Text = $"Player 2 ({data["player2_name"].GetString()}): {(data["player2_ready"].GetBoolean() ? "Sẵn sàng" : "Chưa sẵn sàng")}";
                        break;

                    case "countdown":
                        lbl_Countdown.Visible = true;
                        lbl_Countdown.Text = data["time"].GetInt32().ToString();
                        break;

                    case "start_game":
                        StartGame();
                        break;

                    case "update_time":
                        lbl_GameTimer.Text = "Time: " + data["time"].GetInt32().ToString();
                        break;

                    case "update_score":
                        if (lbl_Score1 != null)
                            lbl_Score1.Text = "Score: " + data["p1_score"].GetInt64().ToString();
                        if (lbl_Score2 != null)
                            lbl_Score2.Text = "Score: " + data["p2_score"].GetInt64().ToString();
                        break;
                    case "game_over":
                        MessageBox.Show("Hết giờ!", "Trò chơi kết thúc");
                        ResetToLobby();
                        break;

                    case "player_disconnected":
                        MessageBox.Show($"Người chơi {data["name"].GetString()} đã ngắt kết nối.");
                        ResetToLobby();
                        break;

                    case "lobby_full":
                        MessageBox.Show("Phòng đã đầy.");
                        break;

                    case "update_game_state":
                        if (data.ContainsKey("ptb_player1"))
                        {
                            JsonElement el = data["ptb_player1"];
                            ptb_player1.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }
                        if (data.ContainsKey("ptb_player2"))
                        {
                            JsonElement el = data["ptb_player2"];
                            ptb_player2.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }

                        if (data.ContainsKey("ptb_roadtrack1"))
                        {
                            JsonElement el = data["ptb_roadtrack1"];
                            ptb_roadtrack1.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }
                        if (data.ContainsKey("ptb_roadtrack1dup"))
                        {
                            JsonElement el = data["ptb_roadtrack1dup"];
                            ptb_roadtrack1dup.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }
                        if (data.ContainsKey("ptb_roadtrack2"))
                        {
                            JsonElement el = data["ptb_roadtrack2"];
                            ptb_roadtrack2.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }
                        if (data.ContainsKey("ptb_roadtrack2dup"))
                        {
                            JsonElement el = data["ptb_roadtrack2dup"];
                            ptb_roadtrack2dup.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }

                        if (data.ContainsKey("ptb_AICar1"))
                        {
                            JsonElement el = data["ptb_AICar1"];
                            ptb_AICar1.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }
                        if (data.ContainsKey("ptb_AICar3"))
                        {
                            JsonElement el = data["ptb_AICar3"];
                            ptb_AICar3.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }
                        if (data.ContainsKey("ptb_AICar5"))
                        {
                            JsonElement el = data["ptb_AICar5"];
                            ptb_AICar5.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }
                        if (data.ContainsKey("ptb_AICar6"))
                        {
                            JsonElement el = data["ptb_AICar6"];
                            ptb_AICar6.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }

                        if (data.ContainsKey("ptb_increasingroad1"))
                        {
                            JsonElement el = data["ptb_increasingroad1"];
                            ptb_increasingroad1.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }
                        if (data.ContainsKey("ptb_decreasingroad1"))
                        {
                            JsonElement el = data["ptb_decreasingroad1"];
                            ptb_decreasingroad1.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }
                        if (data.ContainsKey("ptb_increasingroad2"))
                        {
                            JsonElement el = data["ptb_increasingroad2"];
                            ptb_increasingroad2.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }
                        if (data.ContainsKey("ptb_decreasingroad2"))
                        {
                            JsonElement el = data["ptb_decreasingroad2"];
                            ptb_decreasingroad2.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi xử lý tin nhắn: {ex.Message} (Data: {message})");
            }
        }

        // Ẩn/hiện các đối tượng game
        private void ToggleGameObjects(bool show)
        {
            ptb_roadtrack1.Visible = show;
            ptb_roadtrack1dup.Visible = show;
            ptb_roadtrack2.Visible = show;
            ptb_roadtrack2dup.Visible = show;
            ptb_player1.Visible = show;
            ptb_player2.Visible = show;

            ptb_AICar1.Visible = show;
            ptb_AICar3.Visible = show;
            ptb_AICar5.Visible = show;
            ptb_AICar6.Visible = show;
            ptb_increasingroad1.Visible = show;
            ptb_decreasingroad1.Visible = show;
            ptb_increasingroad2.Visible = show;
            ptb_decreasingroad2.Visible = show;
        }

        // Bắt đầu game
        private void StartGame()
        {
            btn_Ready.Visible = false;
            lbl_P1_Status.Visible = false;
            lbl_P2_Status.Visible = false;
            lbl_Countdown.Visible = false;

            ToggleGameObjects(true);
            lbl_GameTimer.Visible = true;
            lbl_GameTimer.Text = "Time: 60";

            if (lbl_Score1 != null)
            {
                lbl_Score1.Visible = true;
                lbl_Score1.Text = "Score: 0";
            }
            if (lbl_Score2 != null)
            {
                lbl_Score2.Visible = true;
                lbl_Score2.Text = "Score: 0";
            }

            btn_Scoreboard.Enabled = false;

            // Player
            ptb_player1.BringToFront();
            ptb_player2.BringToFront();

            // Xe AI
            ptb_AICar1.BringToFront();
            ptb_AICar3.BringToFront();
            ptb_AICar5.BringToFront();
            ptb_AICar6.BringToFront();

            // Item
            ptb_increasingroad1.BringToFront();
            ptb_decreasingroad1.BringToFront();
            ptb_increasingroad2.BringToFront();
            ptb_decreasingroad2.BringToFront();

            this.Focus();
        }

        // Quay về phòng chờ
        private void ResetToLobby()
        {
            game_timer.Stop();

            btn_Ready.Visible = true;
            btn_Ready.Enabled = true;
            btn_Ready.Text = "Sẵn sàng";
            lbl_P1_Status.Visible = true;
            lbl_P2_Status.Visible = true;
            btn_Scoreboard.Enabled = true;

            ToggleGameObjects(false);
            lbl_Countdown.Visible = false;
            lbl_GameTimer.Visible = false;
            if (lbl_Score1 != null) lbl_Score1.Visible = false;
            if (lbl_Score2 != null) lbl_Score2.Visible = false;

            btn_Ready.Focus();
        }

        private void btn_Ready_Click(object sender, EventArgs e)
        {
            var readyRequest = new { action = "set_ready", ready_status = "true" };
            Send(JsonSerializer.Serialize(readyRequest));
            btn_Ready.Enabled = false;
            btn_Ready.Text = "Đang chờ...";
        }

        private void game_timer_Tick(object sender, EventArgs e) { }

        private void btn_Scoreboard_Click(object sender, EventArgs e)
        {
            Form_ScoreBoard scoreBoard = new Form_ScoreBoard();
            scoreBoard.ShowDialog();
        }

        private void Game_Window_KeyDown(object sender, KeyEventArgs e)
        {
            string direction = null;
            string state = "down";

            if (e.KeyCode == Keys.Left) { direction = "left"; }
            else if (e.KeyCode == Keys.Right) { direction = "right"; }

            if (direction != null)
            {
                var moveRequest = new { action = "move", player = myPlayerNumber, direction, state };
                Send(JsonSerializer.Serialize(moveRequest));
            }
        }

        private void Game_Window_KeyUp(object sender, KeyEventArgs e)
        {
            string direction = null;
            string state = "up";

            if (e.KeyCode == Keys.Left) { direction = "left"; }
            else if (e.KeyCode == Keys.Right) { direction = "right"; }

            if (direction != null)
            {
                var moveRequest = new { action = "move", player = myPlayerNumber, direction, state };
                Send(JsonSerializer.Serialize(moveRequest));
            }
        }

        private void Game_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            reader?.Close();
            stream?.Close();
            client?.Close();
        }
    }
}