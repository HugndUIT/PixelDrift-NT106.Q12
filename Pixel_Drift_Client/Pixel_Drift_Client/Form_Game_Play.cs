using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text.Json;
using System.Windows.Forms;
using WMPLib;

namespace Pixel_Drift
{
    public partial class Game_Window : Form
    {
        private int myPlayerNumber = 0;
        private string myUsername;
        private bool isReturningToLobby = false;

        private Dictionary<string, Control> gameObjects = new Dictionary<string, Control>();

        // Chống Spam phím
        private bool isLeftPressed = false;
        private bool isRightPressed = false;

        // Âm thanh
        private WindowsMediaPlayer Music;
        private SoundPlayer CountDown_5Sec, Buff, Debuff, Car_Hit;

        // Điểm số
        private long player1Score = 0;
        private long player2Score = 0;
        private int crashCount = 0;

        public Game_Window(string username, int playerNum, string roomID)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            this.myUsername = username;
            this.myPlayerNumber = playerNum;
            if (btn_ID != null) btn_ID.Text = "ID: " + roomID;

            string playerColor = (myPlayerNumber == 1) ? "Xe Đỏ" : "Xe Xanh";
            this.Text = $"Pixel Drift - PLAYER {myPlayerNumber} ({playerColor}) - {myUsername}";

            ClientManager.OnMessageReceived += HandleServerMessage;
        }

        public Game_Window() { InitializeComponent(); }

        private void Game_Window_Load(object sender, EventArgs e)
        {
            try
            {
                InitAudio();
                InitControlsCache();
                ResetToLobby();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo Game: " + ex.Message);
                this.Close();
            }
        }

        private void HandleServerMessage(string message)
        {
            if (this.Disposing || this.IsDisposed || !this.IsHandleCreated) return;

            this.Invoke(new Action(() =>
            {
                try
                {
                    var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(message);
                    if (!data.ContainsKey("action")) return;

                    string action = data["action"].GetString();

                    switch (action)
                    {
                        case "update_game_state":
                            UpdateGameObjects(data);
                            break;

                        case "update_score":
                            player1Score = data["p1_score"].GetInt64();
                            player2Score = data["p2_score"].GetInt64();
                            if (lbl_Score1 != null) lbl_Score1.Text = "Score: " + player1Score;
                            if (lbl_Score2 != null) lbl_Score2.Text = "Score: " + player2Score;
                            break;

                        case "update_time":
                            if (lbl_GameTimer != null)
                                lbl_GameTimer.Text = "Time: " + data["time"].GetInt32();
                            break;

                        case "start_game":
                            StartGame();
                            break;

                        case "countdown":
                            if (lbl_Countdown != null)
                            {
                                lbl_Countdown.Visible = true;
                                int time = data["time"].GetInt32();
                                lbl_Countdown.Text = time.ToString();
                                if (time == 5) { Music.controls.stop(); CountDown_5Sec?.Play(); }
                            }
                            break;

                        case "update_ready_status":
                            string p1Name = data["player1_name"].GetString();
                            string p2Name = data["player2_name"].GetString();
                            bool p1Ready = data["player1_ready"].GetBoolean();
                            bool p2Ready = data["player2_ready"].GetBoolean();
                            if (lbl_P1_Status != null) lbl_P1_Status.Text = $"P1 ({p1Name}): {(p1Ready ? "Sẵn sàng" : "...")}";
                            if (lbl_P2_Status != null) lbl_P2_Status.Text = $"P2 ({p2Name}): {(p2Ready ? "Sẵn sàng" : "...")}";
                            break;

                        case "game_over":
                            Music?.controls.stop();
                            MessageBox.Show("Hết giờ! Trò chơi kết thúc.", "Thông báo");
                            EndGame();
                            ResetToLobby();
                            break;

                        case "player_disconnected":
                            string name = data.ContainsKey("name") ? data["name"].GetString() : "Đối thủ";
                            Music?.controls.stop();
                            CountDown_5Sec?.Stop();
                            MessageBox.Show($"{name} đã thoát. Bạn sẽ về Lobby.", "Thông báo");

                            isReturningToLobby = true;
                            Send(new { action = "leave_room" });
                            this.Close();
                            break;

                        case "play_sound":
                            string sound = data["sound"].GetString();
                            PlaySoundEffect(sound);
                            break;

                        case "force_logout":
                            Music?.controls.stop();
                            MessageBox.Show("Tài khoản đăng nhập nơi khác!", "Cảnh báo");
                            Application.Exit();
                            break;
                    }
                }
                catch { }
            }));
        }

        private void InitControlsCache()
        {
            string[] objectNames = {
                "ptb_player1", "ptb_player2",
                "ptb_roadtrack1", "ptb_roadtrack1dup", "ptb_roadtrack2", "ptb_roadtrack2dup",
                "ptb_AICar1", "ptb_AICar3", "ptb_AICar5", "ptb_AICar6",
                "ptb_increasingroad1", "ptb_decreasingroad1", "ptb_increasingroad2", "ptb_decreasingroad2"
            };

            foreach (string name in objectNames)
            {
                Control c = this.Controls.Find(name, true).FirstOrDefault();
                if (c != null) gameObjects[name] = c;
            }
        }

        private void UpdateGameObjects(Dictionary<string, JsonElement> data)
        {
            foreach (var kvp in gameObjects)
            {
                if (data.ContainsKey(kvp.Key))
                {
                    JsonElement el = data[kvp.Key];
                    int x = el.GetProperty("X").GetInt32();
                    int y = el.GetProperty("Y").GetInt32();

                    if (kvp.Value.Location.X != x || kvp.Value.Location.Y != y)
                    {
                        kvp.Value.Location = new Point(x, y);
                    }
                }
            }
        }

        private void Send(object msg) => ClientManager.Send_And_Forget(msg);

        private void btn_Ready_Click(object sender, EventArgs e)
        {
            Send(new { action = "set_ready", ready_status = "true" });
            btn_Ready.Enabled = false;
            btn_Ready.Text = "Đang chờ...";
        }

        private void Game_Window_KeyDown(object sender, KeyEventArgs e)
        {
            string direction = null;

            if (e.KeyCode == Keys.Left)
            {
                if (isLeftPressed) return;
                isLeftPressed = true;
                direction = "left";
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (isRightPressed) return;
                isRightPressed = true;
                direction = "right";
            }

            if (direction != null)
            {
                Send(new { action = "move", player = myPlayerNumber, direction, state = "down" });
            }
        }

        private void Game_Window_KeyUp(object sender, KeyEventArgs e)
        {
            string direction = null;

            if (e.KeyCode == Keys.Left)
            {
                isLeftPressed = false;
                direction = "left";
            }
            else if (e.KeyCode == Keys.Right)
            {
                isRightPressed = false;
                direction = "right";
            }

            if (direction != null)
            {
                Send(new { action = "move", player = myPlayerNumber, direction, state = "up" });
            }
        }

        private void Game_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClientManager.OnMessageReceived -= HandleServerMessage;

            try
            {
                Music?.controls.stop(); Music?.close();
                CountDown_5Sec?.Stop(); Car_Hit?.Stop(); Buff?.Stop(); Debuff?.Stop();
            }
            catch { }

            if (!isReturningToLobby)
            {
                Send(new { action = "leave_room" });
            }
        }

        private void InitAudio()
        {
            Music = new WindowsMediaPlayer();
            Music.settings.setMode("loop", true);
            Music.settings.volume = 30;
            try
            {
                CountDown_5Sec = new SoundPlayer("dem_nguoc.wav");
                Buff = new SoundPlayer("buff.wav");
                Debuff = new SoundPlayer("debuff.wav");
                Car_Hit = new SoundPlayer("car_crash.wav");
                CountDown_5Sec.LoadAsync(); Buff.LoadAsync(); Debuff.LoadAsync(); Car_Hit.LoadAsync();
            }
            catch { }
        }

        private void PlayMusicLoop(string musicFile)
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, musicFile);
                if (File.Exists(path)) { Music.URL = path; Music.controls.play(); }
            }
            catch { }
        }

        private void PlaySoundEffect(string soundType)
        {
            if (soundType == "buff") Buff?.Play();
            else if (soundType == "debuff") Debuff?.Play();
            else if (soundType == "hit_car") { Car_Hit?.Play(); crashCount++; }
        }

        private void ToggleGameObjects(bool show)
        {
            foreach (var ctrl in gameObjects.Values) ctrl.Visible = show;
        }

        private void StartGame()
        {
            crashCount = 0; player1Score = 0; player2Score = 0;
            btn_Ready.Visible = false; lbl_P1_Status.Visible = false;
            lbl_P2_Status.Visible = false; lbl_Countdown.Visible = false;
            btn_Scoreboard.Enabled = false;

            ToggleGameObjects(true);
            lbl_GameTimer.Visible = true; lbl_GameTimer.Text = "Time: 60";
            if (lbl_Score1 != null) { lbl_Score1.Visible = true; lbl_Score1.Text = "Score: 0"; }
            if (lbl_Score2 != null) { lbl_Score2.Visible = true; lbl_Score2.Text = "Score: 0"; }

            if (gameObjects.ContainsKey("ptb_roadtrack1")) gameObjects["ptb_roadtrack1"].SendToBack();
            if (gameObjects.ContainsKey("ptb_roadtrack1dup")) gameObjects["ptb_roadtrack1dup"].SendToBack();
            if (gameObjects.ContainsKey("ptb_roadtrack2")) gameObjects["ptb_roadtrack2"].SendToBack();
            if (gameObjects.ContainsKey("ptb_roadtrack2dup")) gameObjects["ptb_roadtrack2dup"].SendToBack();

            if (gameObjects.ContainsKey("ptb_player1")) gameObjects["ptb_player1"].BringToFront();
            if (gameObjects.ContainsKey("ptb_player2")) gameObjects["ptb_player2"].BringToFront();

            if (gameObjects.ContainsKey("ptb_AICar1")) gameObjects["ptb_AICar1"].BringToFront();
            if (gameObjects.ContainsKey("ptb_AICar3")) gameObjects["ptb_AICar3"].BringToFront();
            if (gameObjects.ContainsKey("ptb_AICar5")) gameObjects["ptb_AICar5"].BringToFront();
            if (gameObjects.ContainsKey("ptb_AICar6")) gameObjects["ptb_AICar6"].BringToFront();

            if (gameObjects.ContainsKey("ptb_increasingroad1")) gameObjects["ptb_increasingroad1"].BringToFront();
            if (gameObjects.ContainsKey("ptb_decreasingroad1")) gameObjects["ptb_decreasingroad1"].BringToFront();
            if (gameObjects.ContainsKey("ptb_increasingroad2")) gameObjects["ptb_increasingroad2"].BringToFront();
            if (gameObjects.ContainsKey("ptb_decreasingroad2")) gameObjects["ptb_decreasingroad2"].BringToFront();

            CountDown_5Sec?.Stop();
            PlayMusicLoop("nhac_thi_dau.wav");

            this.Focus();
        }

        private void ResetToLobby()
        {
            CountDown_5Sec?.Stop();
            PlayMusicLoop("cho_doi.wav");
            game_timer.Stop();

            btn_Ready.Visible = true; btn_Ready.Enabled = true; btn_Ready.Text = "Sẵn sàng";
            lbl_P1_Status.Visible = true; lbl_P2_Status.Visible = true;
            btn_Scoreboard.Enabled = true;

            ToggleGameObjects(false);
            lbl_Countdown.Visible = false; lbl_GameTimer.Visible = false;
            if (lbl_Score1 != null) lbl_Score1.Visible = false;
            if (lbl_Score2 != null) lbl_Score2.Visible = false;

            btn_Ready.Focus();
        }

        private void EndGame()
        {
            try
            {
                int winCount = 0;
                if (myPlayerNumber == 1 && player1Score > player2Score) winCount = 1;
                else if (myPlayerNumber == 2 && player2Score > player1Score) winCount = 1;

                double totalScore = player1Score + (winCount * 500) - (crashCount * 50);
                if (myPlayerNumber == 2) totalScore = player2Score + (winCount * 500) - (crashCount * 50);

                var scoreData = new
                {
                    action = "add_score",
                    player_name = myUsername,
                    win_count = winCount,
                    crash_count = crashCount,
                    total_score = totalScore
                };
                Send(scoreData);
            }
            catch { }
            finally { crashCount = 0; player1Score = 0; player2Score = 0; }
        }

        private void btn_Scoreboard_Click(object sender, EventArgs e)
        {
            var sb = Application.OpenForms.OfType<Form_ScoreBoard>().FirstOrDefault();
            if (sb != null) sb.Show();
            else { new Form_ScoreBoard(ClientManager.GetClient()).Show(); }
        }
    }
}