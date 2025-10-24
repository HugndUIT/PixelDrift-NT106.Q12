using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void Game_Window_Load(object sender, EventArgs e)
        {
            // Ẩn toàn bộ xe khi chưa bắt đầu
            ptb_AICar1.Visible = false;
            ptb_AICar3.Visible = false;
            ptb_AICar5.Visible = false;
            ptb_AICar6.Visible = false;
        }

        private void btn_startgame_Click(object sender, EventArgs e)
        {
            // Tốc độ ban đầu bằng nhau
            LeftRoadSpeed = 10;
            RightRoadSpeed = 10;
            // Hiển thị buff
            ResetBuffPositionLeft(ptb_increasingroad1);
            ResetBuffPositionLeft(ptb_decreasingroad1);
            ResetBuffPositionRight(ptb_increasingroad2);
            ResetBuffPositionRight(ptb_decreasingroad2);

            // Đặt vị trí xuất hiện cho xe từ trên xuống
            int startY = -200;
            ptb_AICar1.Location = new Point(20, startY - 100);
            ptb_AICar5.Location = new Point(160, startY - 200);

            ptb_AICar3.Location = new Point(360, startY - 150);
            ptb_AICar6.Location = new Point(460, startY - 250);

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

        private void KiemTraVaCham()
        {
            // ====== XE 1 ======
            //Kiểm tra xem xe 1 có ăn buff tăng tốc không
            if (ptb_player1.Bounds.IntersectsWith(ptb_increasingroad1.Bounds))
            {
                LeftRoadSpeed += 3;

                //Sau khi ăn buff xong thì sẽ cho buff reset về spawn ngẫu nhiên để tránh bị kẹt buff
                ResetBuffPositionLeft(ptb_increasingroad1);
            }

            //Kiểm tra xem xe 1 có ăn buff giảm tốc không
            if (ptb_player1.Bounds.IntersectsWith(ptb_decreasingroad1.Bounds))
            {
                LeftRoadSpeed -= 3;

                //Sau khi ăn buff xong thì sẽ cho buff reset về spawn ngẫu nhiên để tránh bị kẹt buff
                ResetBuffPositionLeft(ptb_decreasingroad1);
            }

            //Kiểm tra xem xe 1 có va chạm không
            if (ptb_player1.Bounds.IntersectsWith(ptb_AICar1.Bounds))
            {
                LeftRoadSpeed -= 4;

                //Sau khi ăn buff xong thì sẽ cho buff reset về spawn ngẫu nhiên để tránh bị kẹt buff
                ResetBuffPositionLeft(ptb_AICar1);
            }

            //Kiểm tra xem xe 1 có va chạm không
            if (ptb_player1.Bounds.IntersectsWith(ptb_AICar5.Bounds))
            {
                LeftRoadSpeed -= 4;

                //Sau khi ăn buff xong thì sẽ cho buff reset về spawn ngẫu nhiên để tránh bị kẹt buff
                ResetBuffPositionLeft(ptb_AICar5);
            }


            // ====== XE 2 ======
            //Kiểm tra xem xe 2 có ăn buff tăng tốc không
            if (ptb_player2.Bounds.IntersectsWith(ptb_increasingroad2.Bounds))
            {
                RightRoadSpeed += 3;

                //Sau khi ăn buff xong thì sẽ cho buff reset về spawn ngẫu nhiên để tránh bị kẹt buff
                ResetBuffPositionRight(ptb_increasingroad2);
            }

            //Kiểm tra xem xe 1 có ăn buff giảm tốc không
            if (ptb_player2.Bounds.IntersectsWith(ptb_decreasingroad2.Bounds))
            {
                RightRoadSpeed -= 3;

                //Sau khi ăn buff xong thì sẽ cho buff reset về spawn ngẫu nhiên để tránh bị kẹt buff
                ResetBuffPositionRight(ptb_decreasingroad2);
            }

            //Kiểm tra xem xe 1 có va chạm không
            if (ptb_player2.Bounds.IntersectsWith(ptb_AICar6.Bounds))
            {
                RightRoadSpeed -= 4;

                //Sau khi ăn buff xong thì sẽ cho buff reset về spawn ngẫu nhiên để tránh bị kẹt buff
                ResetBuffPositionRight(ptb_AICar6);
            }

            //Kiểm tra xem xe 2 có va chạm không
            if (ptb_player2.Bounds.IntersectsWith(ptb_AICar3.Bounds))
            {
                RightRoadSpeed -= 4;

                //Sau khi ăn buff xong thì sẽ cho buff reset về spawn ngẫu nhiên để tránh bị kẹt buff
                ResetBuffPositionRight(ptb_AICar3);
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

            //Thêm giới hạn di chuyển cho xe 2
            int p2_minX = 40;
            int p2_maxX = 415 - ptb_player2.Width;
            ptb_player2.Left = Math.Max(ptb_player2.Left, p2_minX);
            ptb_player2.Left = Math.Min(ptb_player2.Left, p2_maxX);

            KiemTraVaCham();

            int minSpeed = 4;

            LeftRoadSpeed = Math.Max(LeftRoadSpeed, minSpeed);
            RightRoadSpeed = Math.Max(RightRoadSpeed, minSpeed);
        }


        // Hiển thị buff ở làn bên trái
        private void ResetBuffPositionLeft(PictureBox buff)
        {
            int minX = 40;  
            int maxX = 420;  
            int randomX = Rand.Next(minX, maxX);

            int randomY = Rand.Next(-400, -150); 
            if (buff == ptb_increasingroad1 && Math.Abs(randomY - ptb_decreasingroad1.Top) < 100)
            {
                randomY -= 150;
            }
            else if (buff == ptb_decreasingroad1 && Math.Abs(randomY - ptb_increasingroad1.Top) < 100)
            {
                randomY += 150;
            }

            buff.Location = new Point(randomX, randomY);
        }

        // Hiển thị buff ở làn bên phải
        private void ResetBuffPositionRight(PictureBox buff)
        {
            int minX = 40;  
            int maxX = 420;
            int randomX = Rand.Next(minX, maxX);

            int randomY = Rand.Next(-400, -150); 
            if (buff == ptb_increasingroad2 && Math.Abs(randomY - ptb_decreasingroad2.Top) < 100)
            {
                randomY -= 150;
            }
            else if (buff == ptb_decreasingroad2 && Math.Abs(randomY - ptb_increasingroad2.Top) < 100)
            {
                randomY += 150;
            }

            buff.Location = new Point(randomX, randomY);
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

        // Buff ở làn đường bên trái chạy
        private void MoveBuffRoad1()
        {
            ptb_increasingroad1.Top += LeftRoadSpeed;
            ptb_decreasingroad1.Top += LeftRoadSpeed;

            if (ptb_increasingroad1.Top > this.Height)
                ResetBuffPositionLeft(ptb_increasingroad1);

            if (ptb_decreasingroad1.Top > this.Height)
                ResetBuffPositionLeft(ptb_decreasingroad1);
        }

        // Buff ở làn đường bên phải chạy
        private void MoveBuffRoad2()
        {
            ptb_increasingroad2.Top += RightRoadSpeed;
            ptb_decreasingroad2.Top += RightRoadSpeed;

            if (ptb_increasingroad2.Top > this.Height)
                ResetBuffPositionRight(ptb_increasingroad2);

            if (ptb_decreasingroad2.Top > this.Height)
                ResetBuffPositionRight(ptb_decreasingroad2);
        }

        // ===== DANH SÁCH ẢNH XE =====
        private List<Image> carImages = new List<Image>()
{
        Properties.Resources.BuickerB,
        Properties.Resources.GalardB,
        Properties.Resources.JeepB,
        Properties.Resources.RamB,
        Properties.Resources.SuperB
};
        // ====== LÀN TRÁI ======
        private void MoveAICar1()
        {
            ptb_AICar1.Top += LeftRoadSpeed;
            if (ptb_AICar1.Top > this.Height)
            {
                int randomY = Rand.Next(-300, -80); // xuất hiện gần hơn
                int randomX = Rand.Next(20, 120);
                ptb_AICar1.Location = new Point(randomX, randomY);
                ptb_AICar1.Image = carImages[Rand.Next(carImages.Count)];

            }
        }

        private void MoveAICar5()
        {
            ptb_AICar5.Top += LeftRoadSpeed;
            if (ptb_AICar5.Top > this.Height)
            {
                int randomY = Rand.Next(-300, -80);
                int randomX = Rand.Next(140, 240);
                ptb_AICar5.Location = new Point(randomX, randomY);
                ptb_AICar5.Image = carImages[Rand.Next(carImages.Count)];

            }
        }

        // ====== LÀN PHẢI ======
        private void MoveAICar3()
        {
            ptb_AICar3.Top += RightRoadSpeed;
            if (ptb_AICar3.Top > this.Height)
            {
                int randomY = Rand.Next(-300, -80);
                int randomX = Rand.Next(300, 400);
                ptb_AICar3.Location = new Point(randomX, randomY);
                ptb_AICar3.Image = carImages[Rand.Next(carImages.Count)];

            }
        }

        private void MoveAICar6()
        {
            ptb_AICar6.Top += RightRoadSpeed;
            if (ptb_AICar6.Top > this.Height)
            {
                int randomY = Rand.Next(-300, -80);
                int randomX = Rand.Next(400, 480);
                ptb_AICar6.Location = new Point(randomX, randomY);
                ptb_AICar6.Image = carImages[Rand.Next(carImages.Count)];

            }
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

    }
}
