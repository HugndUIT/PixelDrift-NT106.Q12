﻿namespace Pixel_Drift
{
    partial class Game_Window
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Game_Window));
            this.btn_startgame = new System.Windows.Forms.Button();
            this.game_timer = new System.Windows.Forms.Timer(this.components);
            this.ptb_roadtrack1 = new System.Windows.Forms.PictureBox();
            this.ptb_roadtrack1dup = new System.Windows.Forms.PictureBox();
            this.ptb_increasingroad1 = new System.Windows.Forms.PictureBox();
            this.ptb_decreasingroad1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ptb_AICar5 = new System.Windows.Forms.PictureBox();
            this.ptb_player1 = new System.Windows.Forms.PictureBox();
            this.ptb_AICar1 = new System.Windows.Forms.PictureBox();
            this.ptb_AICar6 = new System.Windows.Forms.PictureBox();
            this.ptb_AICar3 = new System.Windows.Forms.PictureBox();
            this.ptb_roadtrack2dup = new System.Windows.Forms.PictureBox();
            this.ptb_increasingroad2 = new System.Windows.Forms.PictureBox();
            this.ptb_decreasingroad2 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ptb_player2 = new System.Windows.Forms.PictureBox();
            this.ptb_roadtrack2 = new System.Windows.Forms.PictureBox();
            this.btn_Scoreboard = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack1dup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_increasingroad1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_decreasingroad1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_player1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack2dup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_increasingroad2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_decreasingroad2)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_player2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack2)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_startgame
            // 
            this.btn_startgame.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btn_startgame.Location = new System.Drawing.Point(467, 754);
            this.btn_startgame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_startgame.Name = "btn_startgame";
            this.btn_startgame.Size = new System.Drawing.Size(321, 134);
            this.btn_startgame.TabIndex = 3;
            this.btn_startgame.Text = "Start Game";
            this.btn_startgame.UseVisualStyleBackColor = true;
            this.btn_startgame.Click += new System.EventHandler(this.btn_startgame_Click);
            // 
            // game_timer
            // 
            this.game_timer.Interval = 20;
            this.game_timer.Tick += new System.EventHandler(this.game_timer_Tick);
            // 
            // ptb_roadtrack1
            // 
            this.ptb_roadtrack1.Image = global::Pixel_Drift.Properties.Resources.road;
            this.ptb_roadtrack1.Location = new System.Drawing.Point(-3, 0);
            this.ptb_roadtrack1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ptb_roadtrack1.Name = "ptb_roadtrack1";
            this.ptb_roadtrack1.Size = new System.Drawing.Size(617, 734);
            this.ptb_roadtrack1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_roadtrack1.TabIndex = 0;
            this.ptb_roadtrack1.TabStop = false;
            // 
            // ptb_roadtrack1dup
            // 
            this.ptb_roadtrack1dup.Image = global::Pixel_Drift.Properties.Resources.road;
            this.ptb_roadtrack1dup.Location = new System.Drawing.Point(0, 734);
            this.ptb_roadtrack1dup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ptb_roadtrack1dup.Name = "ptb_roadtrack1dup";
            this.ptb_roadtrack1dup.Size = new System.Drawing.Size(617, 734);
            this.ptb_roadtrack1dup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_roadtrack1dup.TabIndex = 1;
            this.ptb_roadtrack1dup.TabStop = false;
            // 
            // ptb_increasingroad1
            // 
            this.ptb_increasingroad1.Image = global::Pixel_Drift.Properties.Resources.speedup;
            this.ptb_increasingroad1.Location = new System.Drawing.Point(120, -70);
            this.ptb_increasingroad1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ptb_increasingroad1.Name = "ptb_increasingroad1";
            this.ptb_increasingroad1.Size = new System.Drawing.Size(67, 66);
            this.ptb_increasingroad1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptb_increasingroad1.TabIndex = 6;
            this.ptb_increasingroad1.TabStop = false;
            // 
            // ptb_decreasingroad1
            // 
            this.ptb_decreasingroad1.Image = global::Pixel_Drift.Properties.Resources.slowdown;
            this.ptb_decreasingroad1.Location = new System.Drawing.Point(387, -70);
            this.ptb_decreasingroad1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ptb_decreasingroad1.Name = "ptb_decreasingroad1";
            this.ptb_decreasingroad1.Size = new System.Drawing.Size(67, 66);
            this.ptb_decreasingroad1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptb_decreasingroad1.TabIndex = 5;
            this.ptb_decreasingroad1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Controls.Add(this.ptb_AICar5);
            this.panel1.Controls.Add(this.ptb_player1);
            this.panel1.Controls.Add(this.ptb_decreasingroad1);
            this.panel1.Controls.Add(this.ptb_AICar1);
            this.panel1.Controls.Add(this.ptb_increasingroad1);
            this.panel1.Controls.Add(this.ptb_roadtrack1dup);
            this.panel1.Controls.Add(this.ptb_roadtrack1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(617, 734);
            this.panel1.TabIndex = 0;
            // 
            // ptb_AICar5
            // 
            this.ptb_AICar5.Image = global::Pixel_Drift.Properties.Resources.RamB;
            this.ptb_AICar5.Location = new System.Drawing.Point(61, 238);
            this.ptb_AICar5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ptb_AICar5.Name = "ptb_AICar5";
            this.ptb_AICar5.Size = new System.Drawing.Size(99, 170);
            this.ptb_AICar5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_AICar5.TabIndex = 9;
            this.ptb_AICar5.TabStop = false;
            // 
            // ptb_player1
            // 
            this.ptb_player1.BackColor = System.Drawing.Color.Transparent;
            this.ptb_player1.Image = ((System.Drawing.Image)(resources.GetObject("ptb_player1.Image")));
            this.ptb_player1.Location = new System.Drawing.Point(269, 578);
            this.ptb_player1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ptb_player1.Name = "ptb_player1";
            this.ptb_player1.Size = new System.Drawing.Size(96, 144);
            this.ptb_player1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptb_player1.TabIndex = 9;
            this.ptb_player1.TabStop = false;
            // 
            // ptb_AICar1
            // 
            this.ptb_AICar1.Image = global::Pixel_Drift.Properties.Resources.BuickerB;
            this.ptb_AICar1.Location = new System.Drawing.Point(427, 228);
            this.ptb_AICar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ptb_AICar1.Name = "ptb_AICar1";
            this.ptb_AICar1.Size = new System.Drawing.Size(83, 142);
            this.ptb_AICar1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_AICar1.TabIndex = 7;
            this.ptb_AICar1.TabStop = false;
            // 
            // ptb_AICar6
            // 
            this.ptb_AICar6.Image = global::Pixel_Drift.Properties.Resources.GalardB;
            this.ptb_AICar6.Location = new System.Drawing.Point(207, 217);
            this.ptb_AICar6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ptb_AICar6.Name = "ptb_AICar6";
            this.ptb_AICar6.Size = new System.Drawing.Size(95, 153);
            this.ptb_AICar6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_AICar6.TabIndex = 11;
            this.ptb_AICar6.TabStop = false;
            // 
            // ptb_AICar3
            // 
            this.ptb_AICar3.Image = global::Pixel_Drift.Properties.Resources.JeepB;
            this.ptb_AICar3.Location = new System.Drawing.Point(12, 330);
            this.ptb_AICar3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ptb_AICar3.Name = "ptb_AICar3";
            this.ptb_AICar3.Size = new System.Drawing.Size(99, 158);
            this.ptb_AICar3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_AICar3.TabIndex = 9;
            this.ptb_AICar3.TabStop = false;
            // 
            // ptb_roadtrack2dup
            // 
            this.ptb_roadtrack2dup.Image = global::Pixel_Drift.Properties.Resources.road;
            this.ptb_roadtrack2dup.Location = new System.Drawing.Point(0, 734);
            this.ptb_roadtrack2dup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ptb_roadtrack2dup.Name = "ptb_roadtrack2dup";
            this.ptb_roadtrack2dup.Size = new System.Drawing.Size(611, 734);
            this.ptb_roadtrack2dup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_roadtrack2dup.TabIndex = 1;
            this.ptb_roadtrack2dup.TabStop = false;
            // 
            // ptb_increasingroad2
            // 
            this.ptb_increasingroad2.BackColor = System.Drawing.SystemColors.ControlText;
            this.ptb_increasingroad2.Image = global::Pixel_Drift.Properties.Resources.speedup;
            this.ptb_increasingroad2.Location = new System.Drawing.Point(131, -70);
            this.ptb_increasingroad2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ptb_increasingroad2.Name = "ptb_increasingroad2";
            this.ptb_increasingroad2.Size = new System.Drawing.Size(67, 66);
            this.ptb_increasingroad2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptb_increasingroad2.TabIndex = 8;
            this.ptb_increasingroad2.TabStop = false;
            // 
            // ptb_decreasingroad2
            // 
            this.ptb_decreasingroad2.BackColor = System.Drawing.SystemColors.ControlText;
            this.ptb_decreasingroad2.Image = global::Pixel_Drift.Properties.Resources.slowdown;
            this.ptb_decreasingroad2.Location = new System.Drawing.Point(400, -70);
            this.ptb_decreasingroad2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ptb_decreasingroad2.Name = "ptb_decreasingroad2";
            this.ptb_decreasingroad2.Size = new System.Drawing.Size(67, 66);
            this.ptb_decreasingroad2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptb_decreasingroad2.TabIndex = 7;
            this.ptb_decreasingroad2.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel2.Controls.Add(this.ptb_AICar6);
            this.panel2.Controls.Add(this.ptb_player2);
            this.panel2.Controls.Add(this.ptb_decreasingroad2);
            this.panel2.Controls.Add(this.ptb_increasingroad2);
            this.panel2.Controls.Add(this.ptb_AICar3);
            this.panel2.Controls.Add(this.ptb_roadtrack2dup);
            this.panel2.Controls.Add(this.ptb_roadtrack2);
            this.panel2.Location = new System.Drawing.Point(660, 12);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(611, 734);
            this.panel2.TabIndex = 1;
            // 
            // ptb_player2
            // 
            this.ptb_player2.BackColor = System.Drawing.Color.Transparent;
            this.ptb_player2.Image = ((System.Drawing.Image)(resources.GetObject("ptb_player2.Image")));
            this.ptb_player2.Location = new System.Drawing.Point(268, 578);
            this.ptb_player2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ptb_player2.Name = "ptb_player2";
            this.ptb_player2.Size = new System.Drawing.Size(96, 144);
            this.ptb_player2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptb_player2.TabIndex = 10;
            this.ptb_player2.TabStop = false;
            // 
            // ptb_roadtrack2
            // 
            this.ptb_roadtrack2.BackColor = System.Drawing.SystemColors.ControlText;
            this.ptb_roadtrack2.Image = global::Pixel_Drift.Properties.Resources.road;
            this.ptb_roadtrack2.Location = new System.Drawing.Point(0, 2);
            this.ptb_roadtrack2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ptb_roadtrack2.Name = "ptb_roadtrack2";
            this.ptb_roadtrack2.Size = new System.Drawing.Size(611, 734);
            this.ptb_roadtrack2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_roadtrack2.TabIndex = 1;
            this.ptb_roadtrack2.TabStop = false;
            // 
            // btn_Scoreboard
            // 
            this.btn_Scoreboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Scoreboard.Location = new System.Drawing.Point(144, 765);
            this.btn_Scoreboard.Name = "btn_Scoreboard";
            this.btn_Scoreboard.Size = new System.Drawing.Size(255, 96);
            this.btn_Scoreboard.TabIndex = 4;
            this.btn_Scoreboard.Text = "🏆 SCORE BOARD 🏆";
            this.btn_Scoreboard.UseVisualStyleBackColor = true;
            this.btn_Scoreboard.Click += new System.EventHandler(this.btn_Scoreboard_Click);
            // 
            // Game_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1283, 811);
            this.Controls.Add(this.btn_Scoreboard);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btn_startgame);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Game_Window";
            this.Text = "Game_Window";
            this.Load += new System.EventHandler(this.Game_Window_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Game_Window_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Game_Window_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack1dup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_increasingroad1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_decreasingroad1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_player1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack2dup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_increasingroad2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_decreasingroad2)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptb_player2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_startgame;
        private System.Windows.Forms.Timer game_timer;
        private System.Windows.Forms.PictureBox ptb_roadtrack1;
        private System.Windows.Forms.PictureBox ptb_roadtrack1dup;
        private System.Windows.Forms.PictureBox ptb_increasingroad1;
        private System.Windows.Forms.PictureBox ptb_decreasingroad1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox ptb_roadtrack2dup;
        private System.Windows.Forms.PictureBox ptb_increasingroad2;
        private System.Windows.Forms.PictureBox ptb_decreasingroad2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox ptb_player1;
        private System.Windows.Forms.PictureBox ptb_player2;
        private System.Windows.Forms.PictureBox ptb_AICar6;
        private System.Windows.Forms.PictureBox ptb_AICar5;
        private System.Windows.Forms.PictureBox ptb_AICar3;
        private System.Windows.Forms.PictureBox ptb_AICar1;
        private System.Windows.Forms.PictureBox ptb_roadtrack2;
        private System.Windows.Forms.Button btn_Scoreboard;
    }
}
