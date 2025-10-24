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

        Random Rand = new Random();

        private void Game_Window_Load(object sender, EventArgs e)
        {

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
            // Bắt đầu chạy game
            game_timer.Start();
        }

        private void game_timer_Tick(object sender, EventArgs e)
        {
            MoveRoad1();
            MoveRoad2();
            MoveBuffRoad1();
            MoveBuffRoad2();
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
