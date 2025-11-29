using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Pixel_Drift
{
    public partial class Game_Window : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private int myPlayerNumber = 0;
        private string myUsername = Form_Dang_Nhap.Current_Username ?? "Player";
        // File Âm thanh
        private WindowsMediaPlayer Music;
        private SoundPlayer CountDown_5Sec;
        private SoundPlayer Buff;
        private SoundPlayer Debuff;
        private SoundPlayer Car_Hit;

        // TÍNH NĂNG SCOREBOARD
        private long player1Score = 0;
        private long player2Score = 0;
        private int crashCount = 0;

        public Game_Window()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public Game_Window(TcpClient client,string myUsername,int playerNum,string roomID)
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            this.client = client;
            this.myUsername = myUsername;
            this.myPlayerNumber = playerNum;
            btn_ID.Text = "ID: " + roomID;

            this.stream = client.GetStream();
            this.reader = new StreamReader(stream, Encoding.UTF8);

            string playerColor = (myPlayerNumber == 1) ? "Xe Đỏ" : "Xe Xanh";
            this.Text = $"Pixel Drift - PLAYER {myPlayerNumber} ({playerColor}) - {myUsername}";
        }

        private void Game_Window_Load(object sender, EventArgs e)
        {
            try
            {
                // Gán file âm thanh
                Music = new WindowsMediaPlayer();
                Music.settings.setMode("loop", true);
                Music.settings.volume = 30;

                try
                {
                    CountDown_5Sec = new SoundPlayer("dem_nguoc.wav");
                    Buff = new SoundPlayer("buff.wav");
                    Debuff = new SoundPlayer("debuff.wav");
                    Car_Hit = new SoundPlayer("car_crash.wav");

                    CountDown_5Sec.LoadAsync();
                    Buff.LoadAsync();
                    Debuff.LoadAsync();
                    Car_Hit.LoadAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không tải được file âm thanh: {ex.Message}", "Cảnh báo");
                }

                ResetToLobby();

                Task.Run(() => ListenForServerMessages());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi Game_Window_Load: {ex.Message}\n\nStackTrace: {ex.StackTrace}",
                    "Lỗi Nghiêm Trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void PlayMusicLoop(string musicFile)
        {
            try
            {
                string musicPath = Path.Combine(Application.StartupPath, musicFile);
                if (File.Exists(musicPath))
                {
                    Music.URL = musicPath;
                    Music.controls.play();
                }
            }
            catch { }
        }

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

        // Luồng lắng nghe
        private async Task ListenForServerMessages()
        {
            string message;
            try
            {
                while ((message = await reader.ReadLineAsync()) != null)
                {
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        this.Invoke(new Action(() => ProcessServerMessage(message)));
                    }
                }
            }
            catch (System.IO.IOException)
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    this.Invoke(new Action(() => {
                        ResetToLobby();
                    }));
                }
            }
            catch (ObjectDisposedException)
            {
                
            }
            catch (Exception ex)
            {
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    this.Invoke(new Action(() => {
                        MessageBox.Show($"Mất kết nối server: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ResetToLobby();
                    }));
                }
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
                        string playerColor = (myPlayerNumber == 1) ? "Xe Đỏ" : "Xe Xanh";
                        this.Text = $"Pixel Drift - PLAYER {myPlayerNumber} ({playerColor})";
                        break;

                    case "update_ready_status":
                        string p1Name = data["player1_name"].GetString();
                        string p2Name = data["player2_name"].GetString();
                        bool p1Ready = data["player1_ready"].GetBoolean();
                        bool p2Ready = data["player2_ready"].GetBoolean();

                        lbl_P1_Status.Text = $"Player 1 ({p1Name} - Xe Đỏ): {(p1Ready ? "Sẵn sàng" : "Chưa sẵn sàng")}";
                        lbl_P2_Status.Text = $"Player 2 ({p2Name} - Xe Xanh): {(p2Ready ? "Sẵn sàng" : "Chưa sẵn sàng")}";
                        break;

                    case "countdown":
                        lbl_Countdown.Visible = true;
                        lbl_Countdown.Text = data["time"].GetInt32().ToString();
                        if (data["time"].GetInt32() == 5)
                        {
                            Music.controls.stop();
                            CountDown_5Sec?.Play();
                        }
                        break;

                    case "start_game":
                        StartGame();
                        break;

                    case "update_time":
                        lbl_GameTimer.Text = "Time: " + data["time"].GetInt32().ToString();
                        break;

                    case "update_score":
                        player1Score = data["p1_score"].GetInt64();
                        player2Score = data["p2_score"].GetInt64();

                        if (lbl_Score1 != null)
                            lbl_Score1.Text = "Score: " + player1Score.ToString();
                        if (lbl_Score2 != null)
                            lbl_Score2.Text = "Score: " + player2Score.ToString();
                        break;

                    case "game_over":
                        Music?.controls.stop();
                        MessageBox.Show("Hết giờ!", "Trò chơi kết thúc");
                        EndGame();
                        ResetToLobby();
                        break;

                    case "player_disconnected":
                        string name = "";
                        if (data.ContainsKey("name") && data["name"].ValueKind == JsonValueKind.String)
                        {
                            name = data["name"].GetString();
                        }

                        if (string.IsNullOrEmpty(name) || name == "Unknown" || name.Contains("Unknown"))
                        {
                            break;
                        }

                        MessageBox.Show($"{name} đã ngắt kết nối. Trở về sảnh chờ.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ResetToLobby();
                        break;

                    case "lobby_full":
                        MessageBox.Show("Phòng đã đầy.");
                        break;

                    case "update_game_state":
                        UpdateGameObjects(data);
                        break;

                    case "play_sound":
                        string soundType = data["sound"].GetString();
                        PlaySoundEffect(soundType);
                        break;

                    case "force_logout":
                        Music?.controls.stop();
                        string logoutMsg = "Tài khoản của bạn đã được đăng nhập từ nơi khác.";
                        if (data.ContainsKey("Message"))
                        {
                            logoutMsg = data["Message"].GetString();
                        }
                        MessageBox.Show(logoutMsg, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        Application.Exit();
                        break;

                    case "scoreboard_data":
                        string scoreData = data["data"].GetString();
                        if (Application.OpenForms.OfType<Form_ScoreBoard>().Any())
                        {
                            var scoreboard = Application.OpenForms.OfType<Form_ScoreBoard>().First();
                            scoreboard.DisplayScoreBoard(scoreData);
                        }
                        break;

                    case "search_result":
                        string searchData = data["data"].GetString();
                        if (Application.OpenForms.OfType<Form_ScoreBoard>().Any())
                        {
                            var scoreboard = Application.OpenForms.OfType<Form_ScoreBoard>().First();
                            scoreboard.DisplaySearchResults(searchData);
                        }
                        break;

                    case "add_score_result":
                        bool success = data["success"].GetBoolean();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi xử lý tin nhắn: {ex.Message} (Data: {message})");
            }
        }

        private void EndGame()
        {
            try
            {
                string playerName = myUsername;
                int winCount = 0;

                // Xác định người thắng dựa trên điểm số
                if (myPlayerNumber == 1 && player1Score > player2Score)
                    winCount = 1;
                else if (myPlayerNumber == 2 && player2Score > player1Score)
                    winCount = 1;

                // Tính tổng điểm
                double totalScore = CalculateTotalScore(winCount, crashCount);

                // Gửi điểm lên server
                var scoreData = new
                {
                    action = "add_score",
                    player_name = playerName,
                    win_count = winCount,
                    crash_count = crashCount,
                    total_score = totalScore
                };

                Send(JsonSerializer.Serialize(scoreData));
                Console.WriteLine($"Đã gửi điểm: {playerName} - Thắng: {winCount} - Đâm: {crashCount} - Tổng: {totalScore}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi điểm: {ex.Message}");
            }
            finally
            {
                // Reset biến đếm cho game tiếp theo
                crashCount = 0;
                player1Score = 0;
                player2Score = 0;
            }
        }

        private double CalculateTotalScore(int winCount, int crashCount)
        {
            long currentPlayerScore = (myPlayerNumber == 1) ? player1Score : player2Score;
            double baseScore = currentPlayerScore;
            double winBonus = winCount * 500;
            double crashPenalty = crashCount * 50;

            return baseScore + winBonus - crashPenalty;
        }

        private void UpdateGameObjects(Dictionary<string, JsonElement> data)
        {
            // Cập nhật tất cả game objects
            string[] objects = {
                "ptb_player1", "ptb_player2",
                "ptb_roadtrack1", "ptb_roadtrack1dup", "ptb_roadtrack2", "ptb_roadtrack2dup",
                "ptb_AICar1", "ptb_AICar3", "ptb_AICar5", "ptb_AICar6",
                "ptb_increasingroad1", "ptb_decreasingroad1", "ptb_increasingroad2", "ptb_decreasingroad2"
            };

            foreach (string objName in objects)
            {
                if (data.ContainsKey(objName))
                {
                    JsonElement el = data[objName];
                    Control control = this.Controls.Find(objName, true).FirstOrDefault();
                    if (control != null)
                    {
                        control.Location = new Point(el.GetProperty("X").GetInt32(), el.GetProperty("Y").GetInt32());
                    }
                }
            }
        }

        private void PlaySoundEffect(string soundType)
        {
            DateTime now = DateTime.Now;

            if (soundType == "buff")
                Buff?.Play();
            else if (soundType == "debuff")
                Debuff?.Play();
            else if (soundType == "hit_car")
            {
                Car_Hit?.Play();
                crashCount++; // Đếm số lần va chạm
            }
        }

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

        private void StartGame()
        {
            crashCount = 0;
            player1Score = 0;
            player2Score = 0;

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

            CountDown_5Sec?.Stop();
            PlayMusicLoop("nhac_thi_dau.wav");

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

        private void ResetToLobby()
        {
            CountDown_5Sec?.Stop();
            PlayMusicLoop("cho_doi.wav");
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
            var readyRequest = new
            {
                action = "set_ready",
                ready_status = "true"
            };
            Send(JsonSerializer.Serialize(readyRequest));
            btn_Ready.Enabled = false;
            btn_Ready.Text = "Đang chờ...";
        }

        private void btn_Scoreboard_Click(object sender, EventArgs e)
        {
            Form_ScoreBoard scoreboardForm = new Form_ScoreBoard(client);
            scoreboardForm.ShowDialog();
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
            // Tắt hết các file nhạc
            Music?.controls.stop();
            Music?.close();
            CountDown_5Sec?.Stop();
            Car_Hit?.Stop();
            Buff?.Stop();
            Debuff?.Stop();

            reader?.Close();
            stream?.Close();
            client?.Close();
        }
        private void game_timer_Tick(object sender, EventArgs e)
        {
        }
    }
}