using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Pixel_Drift
{
    public partial class Game_Window : Form
    {
        public Game_Window()
        {
            InitializeComponent();
        }

        static int LeftRoadSpeed;
        static int RightRoadSpeed;

        //Tạo biến chỉ trạng thái di chuyển của người chơi
        bool Player1_left, Player1_right;

        bool Player2_left, Player2_right;

        //Tốc độ di chuyển ngang của xe
        int PlayerSpeed = 12;

        Random Rand = new Random();

        private List<Image> carImages = new List<Image>();

        private void Game_Window_Load(object sender, EventArgs e)
        {
            // Ẩn toàn bộ xe khi chưa bắt đầu
            ptb_AICar1.Visible = false;
            ptb_AICar3.Visible = false;
            ptb_AICar5.Visible = false;
            ptb_AICar6.Visible = false;

            ClientManager.Connect("127.0.0.1", 1111);
        }

        private void btn_startgame_Click(object sender, EventArgs e)
        {
            // Tốc độ ban đầu bằng nhau
            LeftRoadSpeed = 10;
            RightRoadSpeed = 10;

            // Yêu cầu vị trí ban đầu từ Server
            RequestNewPosition("ptb_increasingroad1");
            RequestNewPosition("ptb_decreasingroad1");
            RequestNewPosition("ptb_increasingroad2");
            RequestNewPosition("ptb_decreasingroad2");
            RequestNewPosition("ptb_AICar1");
            RequestNewPosition("ptb_AICar5");
            RequestNewPosition("ptb_AICar3");
            RequestNewPosition("ptb_AICar6");

            // Hiển thị xe lại
            ptb_AICar1.Visible = true;
            ptb_AICar3.Visible = true;
            ptb_AICar5.Visible = true;
            ptb_AICar6.Visible = true;


            // Bắt đầu chạy game
            game_timer.Start();

            btn_startgame.Enabled = false;
            btn_Scoreboard.Enabled = false;

        }

        //Phan kiem tra va cham cua xe
        private void KiemTraVaCham()
        {
            // ====== XE 1 ======
            //Kiểm tra xem xe 1 có ăn buff tăng tốc không
            if (ptb_player1.Bounds.IntersectsWith(ptb_increasingroad1.Bounds))
            {
                LeftRoadSpeed += 3;
                RequestNewPosition("ptb_increasingroad1");
            }

            //Kiểm tra xem xe 1 có ăn buff giảm tốc không
            if (ptb_player1.Bounds.IntersectsWith(ptb_decreasingroad1.Bounds))
            {
                LeftRoadSpeed -= 3;
                RequestNewPosition("ptb_decreasingroad1");
            }

            //Kiểm tra xem xe 1 có va chạm không
            if (ptb_player1.Bounds.IntersectsWith(ptb_AICar1.Bounds))
            {
                LeftRoadSpeed -= 4;
                RequestNewPosition("ptb_AICar1");
            }

            //Kiểm tra xem xe 1 có va chạm không
            if (ptb_player1.Bounds.IntersectsWith(ptb_AICar5.Bounds))
            {
                LeftRoadSpeed -= 4;
                RequestNewPosition("ptb_AICar5");
            }


            // ====== XE 2 ======
            //Kiểm tra xem xe 2 có ăn buff tăng tốc không
            if (ptb_player2.Bounds.IntersectsWith(ptb_increasingroad2.Bounds))
            {
                RightRoadSpeed += 3;
                RequestNewPosition("ptb_increasingroad2");
            }

            //Kiểm tra xem xe 1 có ăn buff giảm tốc không
            if (ptb_player2.Bounds.IntersectsWith(ptb_decreasingroad2.Bounds))
            {
                RightRoadSpeed -= 3;
                RequestNewPosition("ptb_decreasingroad2");
            }

            //Kiểm tra xem xe 1 có va chạm không
            if (ptb_player2.Bounds.IntersectsWith(ptb_AICar6.Bounds))
            {
                RightRoadSpeed -= 4;
                RequestNewPosition("ptb_AICar6");
            }

            //Kiểm tra xem xe 2 có va chạm không
            if (ptb_player2.Bounds.IntersectsWith(ptb_AICar3.Bounds))
            {
                RightRoadSpeed -= 4;
                RequestNewPosition("ptb_AICar3");
            }
        }

        private void game_timer_Tick(object sender, EventArgs e)
        {
            MoveRoad1();
            MoveRoad2();
            MoveBuffRoad1();
            MoveBuffRoad2();

            MoveAICar1();
            MoveAICar3();
            MoveAICar5();
            MoveAICar6();

            // Di chuyển xe 1 (người chơi này)
            if (Player1_left)
            {
                ptb_player1.Left -= PlayerSpeed;
            }
            if (Player1_right)
            {
                ptb_player1.Left += PlayerSpeed;
            }

            // Di chuyển xe 2 (người chơi này)
            if (Player2_left)
            {
                ptb_player2.Left -= PlayerSpeed;
            }
            if (Player2_right)
            {
                ptb_player2.Left += PlayerSpeed;
            }

            // Thêm giới hạn di chuyển cho xe 1
            int p1_minX = 40;
            int p1_maxX = 420 - ptb_player1.Width;
            ptb_player1.Left = Math.Max(ptb_player1.Left, p1_minX);
            ptb_player1.Left = Math.Min(ptb_player1.Left, p1_maxX);

            // Thêm giới hạn di chuyển cho xe 2
            int p2_minX = 40;
            int p2_maxX = 415 - ptb_player2.Width;
            ptb_player2.Left = Math.Max(ptb_player2.Left, p2_minX);
            ptb_player2.Left = Math.Min(ptb_player2.Left, p2_maxX);

            KiemTraVaCham();

            int minSpeed = 4;

            LeftRoadSpeed = Math.Max(LeftRoadSpeed, minSpeed);
            RightRoadSpeed = Math.Max(RightRoadSpeed, minSpeed);
        }

        private void RequestNewPosition(string objectName)
        {
            var request = new Dictionary<string, string>
            {
                { "action", "position_object" },
                { "object_name", objectName }
            };

            string response = ClientManager.SendRequest(request);
            HandlePositionResponse(response);
        }

        // Hàm xử lý phản hồi từ Server
        private void HandlePositionResponse(string response)
        {
            if (string.IsNullOrEmpty(response)) return;

            try
            {
                var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(response);

                if (data != null && data.ContainsKey("status") && data["status"].GetString() == "success" && data.ContainsKey("action") && data["action"].GetString() == "update_position")
                {
                    string name = data["name"].GetString();
                    int x = data["x"].GetInt32();
                    int y = data["y"].GetInt32();

                    Control[] controls = this.Controls.Find(name, true);
                    if (controls.Length > 0 && controls[0] is PictureBox ptb)
                    {
                        ptb.Location = new Point(x, y);

                        if (name.Contains("AICar") && carImages.Count > 0)
                        {
                            ptb.Image = carImages[Rand.Next(carImages.Count)];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi xử lý phản hồi vị trí: {ex.Message}");
            }
        }

        // Đường bên trái chạy
        private void MoveRoad1()
        {
            ptb_roadtrack1.Top += LeftRoadSpeed;
            ptb_roadtrack1dup.Top += LeftRoadSpeed;

            if (ptb_roadtrack1.Top >= this.Height)
            {
                ptb_roadtrack1.Top = ptb_roadtrack1dup.Top - ptb_roadtrack1.Height;
            }
            if (ptb_roadtrack1dup.Top >= this.Height)
            {
                ptb_roadtrack1dup.Top = ptb_roadtrack1.Top - ptb_roadtrack1dup.Height;
            }
        }

        // Đường bên phải chạy
        private void MoveRoad2()
        {
            ptb_roadtrack2.Top += RightRoadSpeed;
            ptb_roadtrack2dup.Top += RightRoadSpeed;

            if (ptb_roadtrack2.Top >= this.Height)
            {
                ptb_roadtrack2.Top = ptb_roadtrack2dup.Top - ptb_roadtrack2.Height;
            }
            if (ptb_roadtrack2dup.Top >= this.Height)
            {
                ptb_roadtrack2dup.Top = ptb_roadtrack2.Top - ptb_roadtrack2dup.Height;
            }
        }

        // ====== LÀN TRÁI - AI CARS ======
        private void MoveAICar1()
        {
            ptb_AICar1.Top += LeftRoadSpeed;
            if (ptb_AICar1.Top > this.Height)
            {
                RequestNewPosition("ptb_AICar1");
            }
        }

        private void MoveAICar5()
        {
            ptb_AICar5.Top += LeftRoadSpeed;
            if (ptb_AICar5.Top > this.Height)
            {
                RequestNewPosition("ptb_AICar5");
            }
        }

        // ====== LÀN PHẢI - AI CARS ======
        private void MoveAICar3()
        {
            ptb_AICar3.Top += RightRoadSpeed;
            if (ptb_AICar3.Top > this.Height)
            {
                RequestNewPosition("ptb_AICar3");
            }
        }

        private void MoveAICar6()
        {
            ptb_AICar6.Top += RightRoadSpeed;
            if (ptb_AICar6.Top > this.Height)
            {
                RequestNewPosition("ptb_AICar6");
            }
        }

        // Buff ở làn đường bên trái chạy
        private void MoveBuffRoad1()
        {
            ptb_increasingroad1.Top += LeftRoadSpeed;
            ptb_decreasingroad1.Top += LeftRoadSpeed;

            if (ptb_increasingroad1.Top > this.Height)
                RequestNewPosition("ptb_increasingroad1");

            if (ptb_decreasingroad1.Top > this.Height)
                RequestNewPosition("ptb_decreasingroad1");
        }

        // Buff ở làn đường bên phải chạy
        private void MoveBuffRoad2()
        {
            ptb_increasingroad2.Top += RightRoadSpeed;
            ptb_decreasingroad2.Top += RightRoadSpeed;

            if (ptb_increasingroad2.Top > this.Height)
                RequestNewPosition("ptb_increasingroad2");

            if (ptb_decreasingroad2.Top > this.Height)
                RequestNewPosition("ptb_decreasingroad2");
        }

        private void btn_Scoreboard_Click(object sender, EventArgs e)
        {
            Form_ScoreBoard scoreBoard = new Form_ScoreBoard();
            scoreBoard.ShowDialog();
        }

        private void Game_Window_KeyDown(object sender, KeyEventArgs e)
        {
            //Kiểm soát di chuyển của player1
            if (e.KeyCode == Keys.Left)
            {
                Player1_left = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                Player1_right = true;
            }

            //Kiểm soát di chuyển của player2
            if (e.KeyCode == Keys.A)
            {
                Player2_left = true;
            }
            if (e.KeyCode == Keys.D)
            {
                Player2_right = true;
            }


        }

        private void Game_Window_KeyUp(object sender, KeyEventArgs e)
        {
            //Kiểm soát di chuyển của player1
            if (e.KeyCode == Keys.Left)
            {
                Player1_left = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                Player1_right = false;
            }

            //Kiểm soát di chuyển của player2
            if (e.KeyCode == Keys.A)
            {
                Player2_left = false;
            }
            if (e.KeyCode == Keys.D)
            {
                Player2_right = false;
            }
        }

        private void Game_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClientManager.CloseConnection();
        }
    }
}