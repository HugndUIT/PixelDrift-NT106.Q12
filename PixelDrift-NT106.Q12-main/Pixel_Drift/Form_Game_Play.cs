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
        //global variables
      
        int roadSpeed = 5;
        bool carLeft;
        bool carRight;
        int trafficSpeed = 5;
     
        Random rnd = new Random();
        public Game_Window()
        {
            InitializeComponent();
        }

        static int LeftRoadSpeed;
        static int RightRoadSpeed;

        Random Rand = new Random();

        private void Game_Window_Load(object sender, EventArgs e)
        {
            // Ẩn toàn bộ xe khi chưa bắt đầu
            ptb_AICar1.Visible = false;
            ptb_AICar2.Visible = false;
            ptb_AICar3.Visible = false;
            ptb_AICar4.Visible = false;
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
            ptb_AICar2.Location = new Point(300, startY - 300);

            ptb_AICar3.Location = new Point(360, startY - 150);
            ptb_AICar6.Location = new Point(460, startY - 250);
            ptb_AICar4.Location = new Point(540, startY - 350);

            // Hiển thị xe lại
            ptb_AICar1.Visible = true;
            ptb_AICar2.Visible = true;
            ptb_AICar3.Visible = true;
            ptb_AICar4.Visible = true;
            ptb_AICar5.Visible = true;
            ptb_AICar6.Visible = true;

            // Bắt đầu chạy game
            game_timer.Start();
        }


        private void game_timer_Tick(object sender, EventArgs e)
        {
            MoveRoad1();
            MoveRoad2();
            MoveBuffRoad1();
            MoveBuffRoad2();

            MoveAICar1();
            MoveAICar2();
            MoveAICar3();
            MoveAICar4();
            MoveAICar5();
            MoveAICar6();
        }

        // Kiểm tra hai PictureBox có đè lên nhau không
        private bool IsColliding(PictureBox a, PictureBox b)
        {
            return a.Bounds.IntersectsWith(b.Bounds);
        }

        private void ResetBuffPositionLeft(PictureBox buff)
        {
            int minX = 40;
            int maxX = 420;
            int randomX = Rand.Next(minX, maxX);

            // spawn xa hơn => tần suất ít hơn
            int randomY = Rand.Next(-700, -400);

            buff.Location = new Point(randomX, randomY);
        }

        private void ResetBuffPositionRight(PictureBox buff)
        {
            int minX = 40;
            int maxX = 420;
            int randomX = Rand.Next(minX, maxX);

            int randomY = Rand.Next(-700, -400);

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

                // tránh spawn trùng buff
                while (IsColliding(ptb_AICar1, ptb_increasingroad1) || IsColliding(ptb_AICar1, ptb_decreasingroad1))
                    ptb_AICar1.Top -= 100;
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

                while (IsColliding(ptb_AICar5, ptb_increasingroad1) || IsColliding(ptb_AICar5, ptb_decreasingroad1))
                    ptb_AICar5.Top -= 100;
            }
        }

        private void MoveAICar2()
        {
            ptb_AICar2.Top += LeftRoadSpeed;
            if (ptb_AICar2.Top > this.Height)
            {
                int randomY = Rand.Next(-300, -80);
                int randomX = Rand.Next(260, 360);
                ptb_AICar2.Location = new Point(randomX, randomY);
                ptb_AICar2.Image = carImages[Rand.Next(carImages.Count)];

                while (IsColliding(ptb_AICar2, ptb_increasingroad1) || IsColliding(ptb_AICar2, ptb_decreasingroad1))
                    ptb_AICar2.Top -= 100;
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

                while (IsColliding(ptb_AICar3, ptb_increasingroad2) || IsColliding(ptb_AICar3, ptb_decreasingroad2))
                    ptb_AICar3.Top -= 100;
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

                while (IsColliding(ptb_AICar6, ptb_increasingroad2) || IsColliding(ptb_AICar6, ptb_decreasingroad2))
                    ptb_AICar6.Top -= 100;
            }
        }

        private void MoveAICar4()
        {
            ptb_AICar4.Top += RightRoadSpeed;
            if (ptb_AICar4.Top > this.Height)
            {
                int randomY = Rand.Next(-300, -80);
                int randomX = Rand.Next(500, 560);
                ptb_AICar4.Location = new Point(randomX, randomY);
                ptb_AICar4.Image = carImages[Rand.Next(carImages.Count)];

                while (IsColliding(ptb_AICar4, ptb_increasingroad2) || IsColliding(ptb_AICar4, ptb_decreasingroad2))
                    ptb_AICar4.Top -= 100;
            }
        }


        private void ptb_increasingroad1_Click(object sender, EventArgs e)
        {

        }

        private void ptb_decreasingroad1_Click(object sender, EventArgs e)
        {

        }

        private void ptb_increasingroad2_Click(object sender, EventArgs e)
        {

        }

        private void ptb_decreasingroad2_Click(object sender, EventArgs e)
        {

        }
    }
}
