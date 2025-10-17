using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Game_Window : Form
    {
        public Game_Window()
        {
            InitializeComponent();
        }

        static int roadspeed;

        private void Game_Window_Load(object sender, EventArgs e)
        {

        }

        private void btn_startgame_Click(object sender, EventArgs e)
        {
            roadspeed = 10;
            game_timer.Start();
        }

        private void game_timer_Tick(object sender, EventArgs e)
        {
            MoveRoad();
        }

        private void MoveRoad()
        {
            ptb_roadtrack1.Top += roadspeed;
            ptb_roadtrack1dup.Top += roadspeed;

            ptb_roadtrack2.Top += roadspeed;
            ptb_roadtrack2dup.Top += roadspeed;

            if (ptb_roadtrack1.Top >= this.Height)
            {
                ptb_roadtrack1.Top = ptb_roadtrack1dup.Top - ptb_roadtrack1.Height;
            }
            if (ptb_roadtrack1dup.Top >= this.Height)
            {
                ptb_roadtrack1dup.Top = ptb_roadtrack1.Top - ptb_roadtrack1dup.Height;
            }

            if (ptb_roadtrack2.Top >= this.Height)
            {
                ptb_roadtrack2.Top = ptb_roadtrack2dup.Top - ptb_roadtrack2.Height;
            }
            if (ptb_roadtrack2dup.Top >= this.Height)
            {
                ptb_roadtrack2dup.Top = ptb_roadtrack2.Top - ptb_roadtrack2dup.Height;
            }
        }
    }
}
