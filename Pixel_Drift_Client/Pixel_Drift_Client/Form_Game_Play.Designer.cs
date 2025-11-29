using System;
using Guna.UI2.WinForms;
using System.Drawing;

namespace Pixel_Drift
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
            this.game_timer = new System.Windows.Forms.Timer(this.components);
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.btnClose = new Guna.UI2.WinForms.Guna2ControlBox();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2ControlBox();
            this.panel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.lbl_P1_Status = new System.Windows.Forms.Label();
            this.ptb_decreasingroad1 = new System.Windows.Forms.PictureBox();
            this.ptb_player1 = new System.Windows.Forms.PictureBox();
            this.ptb_AICar5 = new System.Windows.Forms.PictureBox();
            this.lbl_Score1 = new System.Windows.Forms.Label();
            this.ptb_AICar1 = new System.Windows.Forms.PictureBox();
            this.ptb_increasingroad1 = new System.Windows.Forms.PictureBox();
            this.ptb_roadtrack1dup = new System.Windows.Forms.PictureBox();
            this.ptb_roadtrack1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.lbl_P2_Status = new System.Windows.Forms.Label();
            this.ptb_decreasingroad2 = new System.Windows.Forms.PictureBox();
            this.ptb_AICar6 = new System.Windows.Forms.PictureBox();
            this.lbl_Score2 = new System.Windows.Forms.Label();
            this.ptb_AICar3 = new System.Windows.Forms.PictureBox();
            this.ptb_player2 = new System.Windows.Forms.PictureBox();
            this.ptb_increasingroad2 = new System.Windows.Forms.PictureBox();
            this.ptb_roadtrack2dup = new System.Windows.Forms.PictureBox();
            this.ptb_roadtrack2 = new System.Windows.Forms.PictureBox();
            this.btn_Scoreboard = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btn_Ready = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btn_ID = new Guna.UI2.WinForms.Guna2GradientButton();
            this.lbl_Countdown = new System.Windows.Forms.Label();
            this.lbl_GameTimer = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_decreasingroad1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_player1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_increasingroad1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack1dup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_decreasingroad2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_player2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_increasingroad2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack2dup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack2)).BeginInit();
            this.SuspendLayout();
            // 
            // game_timer
            // 
            this.game_timer.Interval = 20;
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.AnimateWindow = true;
            this.guna2BorderlessForm1.BorderRadius = 20;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BorderRadius = 10;
            this.btnClose.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnClose.IconColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1217, 15);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(52, 36);
            this.btnClose.TabIndex = 13;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BorderRadius = 10;
            this.btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.btnMinimize.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnMinimize.IconColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(1157, 15);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(4);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(52, 36);
            this.btnMinimize.TabIndex = 14;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.lbl_P1_Status);
            this.panel1.Controls.Add(this.ptb_decreasingroad1);
            this.panel1.Controls.Add(this.ptb_player1);
            this.panel1.Controls.Add(this.ptb_AICar5);
            this.panel1.Controls.Add(this.lbl_Score1);
            this.panel1.Controls.Add(this.ptb_AICar1);
            this.panel1.Controls.Add(this.ptb_increasingroad1);
            this.panel1.Controls.Add(this.ptb_roadtrack1dup);
            this.panel1.Controls.Add(this.ptb_roadtrack1);
            this.panel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.panel1.Location = new System.Drawing.Point(12, 62);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.panel1.Name = "panel1";
            this.panel1.Radius = 15;
            this.panel1.ShadowColor = System.Drawing.Color.Cyan;
            this.panel1.ShadowDepth = 50;
            this.panel1.ShadowShift = 8;
            this.panel1.Size = new System.Drawing.Size(617, 735);
            this.panel1.TabIndex = 0;
            // 
            // lbl_P1_Status
            // 
            this.lbl_P1_Status.AutoSize = true;
            this.lbl_P1_Status.BackColor = System.Drawing.Color.Black;
            this.lbl_P1_Status.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_P1_Status.ForeColor = System.Drawing.Color.Cyan;
            this.lbl_P1_Status.Location = new System.Drawing.Point(1, 699);
            this.lbl_P1_Status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_P1_Status.Name = "lbl_P1_Status";
            this.lbl_P1_Status.Size = new System.Drawing.Size(249, 35);
            this.lbl_P1_Status.TabIndex = 6;
            this.lbl_P1_Status.Text = "Waiting for player 1";
            // 
            // ptb_decreasingroad1
            // 
            this.ptb_decreasingroad1.Image = global::Pixel_Drift.Properties.Resources.slowdown;
            this.ptb_decreasingroad1.Location = new System.Drawing.Point(387, -70);
            this.ptb_decreasingroad1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.ptb_decreasingroad1.Name = "ptb_decreasingroad1";
            this.ptb_decreasingroad1.Size = new System.Drawing.Size(67, 65);
            this.ptb_decreasingroad1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptb_decreasingroad1.TabIndex = 5;
            this.ptb_decreasingroad1.TabStop = false;
            // 
            // ptb_player1
            // 
            this.ptb_player1.BackColor = System.Drawing.Color.Transparent;
            this.ptb_player1.Image = ((System.Drawing.Image)(resources.GetObject("ptb_player1.Image")));
            this.ptb_player1.Location = new System.Drawing.Point(271, 570);
            this.ptb_player1.Margin = new System.Windows.Forms.Padding(4);
            this.ptb_player1.Name = "ptb_player1";
            this.ptb_player1.Size = new System.Drawing.Size(80, 140);
            this.ptb_player1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptb_player1.TabIndex = 9;
            this.ptb_player1.TabStop = false;
            // 
            // ptb_AICar5
            // 
            this.ptb_AICar5.Image = global::Pixel_Drift.Properties.Resources.RamB;
            this.ptb_AICar5.Location = new System.Drawing.Point(151, 295);
            this.ptb_AICar5.Margin = new System.Windows.Forms.Padding(4);
            this.ptb_AICar5.Name = "ptb_AICar5";
            this.ptb_AICar5.Size = new System.Drawing.Size(80, 140);
            this.ptb_AICar5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_AICar5.TabIndex = 9;
            this.ptb_AICar5.TabStop = false;
            // 
            // lbl_Score1
            // 
            this.lbl_Score1.BackColor = System.Drawing.Color.Black;
            this.lbl_Score1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Score1.ForeColor = System.Drawing.Color.Cyan;
            this.lbl_Score1.Location = new System.Drawing.Point(418, 699);
            this.lbl_Score1.Name = "lbl_Score1";
            this.lbl_Score1.Size = new System.Drawing.Size(143, 31);
            this.lbl_Score1.TabIndex = 10;
            this.lbl_Score1.Text = "Score:";
            // 
            // ptb_AICar1
            // 
            this.ptb_AICar1.Image = global::Pixel_Drift.Properties.Resources.BuickerB;
            this.ptb_AICar1.Location = new System.Drawing.Point(407, 295);
            this.ptb_AICar1.Margin = new System.Windows.Forms.Padding(4);
            this.ptb_AICar1.Name = "ptb_AICar1";
            this.ptb_AICar1.Size = new System.Drawing.Size(80, 140);
            this.ptb_AICar1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_AICar1.TabIndex = 7;
            this.ptb_AICar1.TabStop = false;
            // 
            // ptb_increasingroad1
            // 
            this.ptb_increasingroad1.Image = global::Pixel_Drift.Properties.Resources.speedup;
            this.ptb_increasingroad1.Location = new System.Drawing.Point(120, -70);
            this.ptb_increasingroad1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.ptb_increasingroad1.Name = "ptb_increasingroad1";
            this.ptb_increasingroad1.Size = new System.Drawing.Size(67, 65);
            this.ptb_increasingroad1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptb_increasingroad1.TabIndex = 6;
            this.ptb_increasingroad1.TabStop = false;
            // 
            // ptb_roadtrack1dup
            // 
            this.ptb_roadtrack1dup.Image = global::Pixel_Drift.Properties.Resources.road;
            this.ptb_roadtrack1dup.Location = new System.Drawing.Point(0, 735);
            this.ptb_roadtrack1dup.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.ptb_roadtrack1dup.Name = "ptb_roadtrack1dup";
            this.ptb_roadtrack1dup.Size = new System.Drawing.Size(617, 735);
            this.ptb_roadtrack1dup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_roadtrack1dup.TabIndex = 1;
            this.ptb_roadtrack1dup.TabStop = false;
            // 
            // ptb_roadtrack1
            // 
            this.ptb_roadtrack1.Image = global::Pixel_Drift.Properties.Resources.road;
            this.ptb_roadtrack1.Location = new System.Drawing.Point(0, -1);
            this.ptb_roadtrack1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.ptb_roadtrack1.Name = "ptb_roadtrack1";
            this.ptb_roadtrack1.Size = new System.Drawing.Size(617, 735);
            this.ptb_roadtrack1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_roadtrack1.TabIndex = 10;
            this.ptb_roadtrack1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.lbl_P2_Status);
            this.panel2.Controls.Add(this.ptb_decreasingroad2);
            this.panel2.Controls.Add(this.ptb_AICar6);
            this.panel2.Controls.Add(this.lbl_Score2);
            this.panel2.Controls.Add(this.ptb_AICar3);
            this.panel2.Controls.Add(this.ptb_player2);
            this.panel2.Controls.Add(this.ptb_increasingroad2);
            this.panel2.Controls.Add(this.ptb_roadtrack2dup);
            this.panel2.Controls.Add(this.ptb_roadtrack2);
            this.panel2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.panel2.Location = new System.Drawing.Point(660, 62);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.panel2.Name = "panel2";
            this.panel2.Radius = 15;
            this.panel2.ShadowColor = System.Drawing.Color.Magenta;
            this.panel2.ShadowDepth = 50;
            this.panel2.ShadowShift = 8;
            this.panel2.Size = new System.Drawing.Size(611, 735);
            this.panel2.TabIndex = 1;
            // 
            // lbl_P2_Status
            // 
            this.lbl_P2_Status.BackColor = System.Drawing.Color.Black;
            this.lbl_P2_Status.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_P2_Status.ForeColor = System.Drawing.Color.Magenta;
            this.lbl_P2_Status.Location = new System.Drawing.Point(341, 699);
            this.lbl_P2_Status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_P2_Status.Name = "lbl_P2_Status";
            this.lbl_P2_Status.Size = new System.Drawing.Size(271, 35);
            this.lbl_P2_Status.TabIndex = 7;
            this.lbl_P2_Status.Text = "Waiting for player 2";
            // 
            // ptb_decreasingroad2
            // 
            this.ptb_decreasingroad2.BackColor = System.Drawing.SystemColors.ControlText;
            this.ptb_decreasingroad2.Image = global::Pixel_Drift.Properties.Resources.slowdown;
            this.ptb_decreasingroad2.Location = new System.Drawing.Point(400, -70);
            this.ptb_decreasingroad2.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.ptb_decreasingroad2.Name = "ptb_decreasingroad2";
            this.ptb_decreasingroad2.Size = new System.Drawing.Size(67, 65);
            this.ptb_decreasingroad2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptb_decreasingroad2.TabIndex = 7;
            this.ptb_decreasingroad2.TabStop = false;
            // 
            // ptb_AICar6
            // 
            this.ptb_AICar6.Image = global::Pixel_Drift.Properties.Resources.GalardB;
            this.ptb_AICar6.Location = new System.Drawing.Point(403, 295);
            this.ptb_AICar6.Margin = new System.Windows.Forms.Padding(4);
            this.ptb_AICar6.Name = "ptb_AICar6";
            this.ptb_AICar6.Size = new System.Drawing.Size(80, 140);
            this.ptb_AICar6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_AICar6.TabIndex = 11;
            this.ptb_AICar6.TabStop = false;
            // 
            // lbl_Score2
            // 
            this.lbl_Score2.BackColor = System.Drawing.Color.Black;
            this.lbl_Score2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Score2.ForeColor = System.Drawing.Color.Magenta;
            this.lbl_Score2.Location = new System.Drawing.Point(90, 700);
            this.lbl_Score2.Name = "lbl_Score2";
            this.lbl_Score2.Size = new System.Drawing.Size(143, 31);
            this.lbl_Score2.TabIndex = 11;
            this.lbl_Score2.Text = "Score:";
            // 
            // ptb_AICar3
            // 
            this.ptb_AICar3.Image = global::Pixel_Drift.Properties.Resources.JeepB;
            this.ptb_AICar3.Location = new System.Drawing.Point(144, 295);
            this.ptb_AICar3.Margin = new System.Windows.Forms.Padding(4);
            this.ptb_AICar3.Name = "ptb_AICar3";
            this.ptb_AICar3.Size = new System.Drawing.Size(80, 140);
            this.ptb_AICar3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_AICar3.TabIndex = 9;
            this.ptb_AICar3.TabStop = false;
            // 
            // ptb_player2
            // 
            this.ptb_player2.BackColor = System.Drawing.Color.Transparent;
            this.ptb_player2.Image = ((System.Drawing.Image)(resources.GetObject("ptb_player2.Image")));
            this.ptb_player2.Location = new System.Drawing.Point(265, 577);
            this.ptb_player2.Margin = new System.Windows.Forms.Padding(4);
            this.ptb_player2.Name = "ptb_player2";
            this.ptb_player2.Size = new System.Drawing.Size(80, 140);
            this.ptb_player2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptb_player2.TabIndex = 10;
            this.ptb_player2.TabStop = false;
            // 
            // ptb_increasingroad2
            // 
            this.ptb_increasingroad2.BackColor = System.Drawing.SystemColors.ControlText;
            this.ptb_increasingroad2.Image = global::Pixel_Drift.Properties.Resources.speedup;
            this.ptb_increasingroad2.Location = new System.Drawing.Point(131, -70);
            this.ptb_increasingroad2.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.ptb_increasingroad2.Name = "ptb_increasingroad2";
            this.ptb_increasingroad2.Size = new System.Drawing.Size(67, 65);
            this.ptb_increasingroad2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptb_increasingroad2.TabIndex = 8;
            this.ptb_increasingroad2.TabStop = false;
            // 
            // ptb_roadtrack2dup
            // 
            this.ptb_roadtrack2dup.Image = global::Pixel_Drift.Properties.Resources.road;
            this.ptb_roadtrack2dup.Location = new System.Drawing.Point(0, 735);
            this.ptb_roadtrack2dup.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.ptb_roadtrack2dup.Name = "ptb_roadtrack2dup";
            this.ptb_roadtrack2dup.Size = new System.Drawing.Size(611, 735);
            this.ptb_roadtrack2dup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_roadtrack2dup.TabIndex = 1;
            this.ptb_roadtrack2dup.TabStop = false;
            // 
            // ptb_roadtrack2
            // 
            this.ptb_roadtrack2.BackColor = System.Drawing.SystemColors.ControlText;
            this.ptb_roadtrack2.Image = global::Pixel_Drift.Properties.Resources.road;
            this.ptb_roadtrack2.Location = new System.Drawing.Point(0, 1);
            this.ptb_roadtrack2.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.ptb_roadtrack2.Name = "ptb_roadtrack2";
            this.ptb_roadtrack2.Size = new System.Drawing.Size(611, 735);
            this.ptb_roadtrack2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_roadtrack2.TabIndex = 1;
            this.ptb_roadtrack2.TabStop = false;
            // 
            // btn_Scoreboard
            // 
            this.btn_Scoreboard.BorderRadius = 20;
            this.btn_Scoreboard.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Scoreboard.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_Scoreboard.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_Scoreboard.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_Scoreboard.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_Scoreboard.FillColor = System.Drawing.Color.DarkBlue;
            this.btn_Scoreboard.FillColor2 = System.Drawing.Color.Purple;
            this.btn_Scoreboard.Font = new System.Drawing.Font("Segoe UI Black", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Scoreboard.ForeColor = System.Drawing.Color.White;
            this.btn_Scoreboard.Location = new System.Drawing.Point(103, 810);
            this.btn_Scoreboard.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btn_Scoreboard.Name = "btn_Scoreboard";
            this.btn_Scoreboard.Size = new System.Drawing.Size(380, 96);
            this.btn_Scoreboard.TabIndex = 4;
            this.btn_Scoreboard.Text = "🏆 SCORE BOARD 🏆";
            this.btn_Scoreboard.Click += new System.EventHandler(this.btn_Scoreboard_Click);
            // 
            // btn_Ready
            // 
            this.btn_Ready.BorderRadius = 20;
            this.btn_Ready.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Ready.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_Ready.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_Ready.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_Ready.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_Ready.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_Ready.FillColor2 = System.Drawing.Color.Teal;
            this.btn_Ready.Font = new System.Drawing.Font("Segoe UI Black", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btn_Ready.ForeColor = System.Drawing.Color.White;
            this.btn_Ready.Location = new System.Drawing.Point(525, 810);
            this.btn_Ready.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btn_Ready.Name = "btn_Ready";
            this.btn_Ready.Size = new System.Drawing.Size(224, 96);
            this.btn_Ready.TabIndex = 5;
            this.btn_Ready.Text = "READY";
            this.btn_Ready.Click += new System.EventHandler(this.btn_Ready_Click);
            // 
            // btn_ID
            // 
            this.btn_ID.BorderRadius = 20;
            this.btn_ID.Enabled = false;
            this.btn_ID.FillColor = System.Drawing.Color.Gray;
            this.btn_ID.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_ID.Font = new System.Drawing.Font("Segoe UI Black", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ID.ForeColor = System.Drawing.Color.White;
            this.btn_ID.Location = new System.Drawing.Point(791, 809);
            this.btn_ID.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btn_ID.Name = "btn_ID";
            this.btn_ID.Size = new System.Drawing.Size(380, 96);
            this.btn_ID.TabIndex = 12;
            this.btn_ID.Text = "ID:";
            // 
            // lbl_Countdown
            // 
            this.lbl_Countdown.AutoSize = true;
            this.lbl_Countdown.Font = new System.Drawing.Font("Segoe UI Black", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Countdown.ForeColor = System.Drawing.Color.Yellow;
            this.lbl_Countdown.Location = new System.Drawing.Point(620, 745);
            this.lbl_Countdown.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Countdown.Name = "lbl_Countdown";
            this.lbl_Countdown.Size = new System.Drawing.Size(47, 54);
            this.lbl_Countdown.TabIndex = 8;
            this.lbl_Countdown.Text = "5";
            this.lbl_Countdown.Visible = false;
            // 
            // lbl_GameTimer
            // 
            this.lbl_GameTimer.AutoSize = true;
            this.lbl_GameTimer.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_GameTimer.ForeColor = System.Drawing.Color.White;
            this.lbl_GameTimer.Location = new System.Drawing.Point(580, 757);
            this.lbl_GameTimer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_GameTimer.Name = "lbl_GameTimer";
            this.lbl_GameTimer.Size = new System.Drawing.Size(116, 35);
            this.lbl_GameTimer.TabIndex = 9;
            this.lbl_GameTimer.Text = "Time: 60";
            this.lbl_GameTimer.Visible = false;
            // 
            // Game_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(20)))));
            this.BackgroundImage = global::Pixel_Drift.Properties.Resources._31564d5d33242f05596b7641770675451;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1285, 917);
            this.Controls.Add(this.btnMinimize);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btn_ID);
            this.Controls.Add(this.lbl_GameTimer);
            this.Controls.Add(this.lbl_Countdown);
            this.Controls.Add(this.btn_Ready);
            this.Controls.Add(this.btn_Scoreboard);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.Name = "Game_Window";
            this.Text = "Game_Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Game_Window_FormClosing);
            this.Load += new System.EventHandler(this.Game_Window_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Game_Window_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Game_Window_KeyUp);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_decreasingroad1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_player1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_increasingroad1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack1dup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptb_decreasingroad2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_AICar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_player2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_increasingroad2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack2dup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Game_timer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
        private System.Windows.Forms.Timer game_timer;
        private System.Windows.Forms.PictureBox ptb_roadtrack1dup;
        private System.Windows.Forms.PictureBox ptb_increasingroad1;
        private System.Windows.Forms.PictureBox ptb_decreasingroad1;
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2ControlBox btnClose;
        private Guna.UI2.WinForms.Guna2ControlBox btnMinimize;
        private Guna.UI2.WinForms.Guna2ShadowPanel panel1;
        private Guna.UI2.WinForms.Guna2ShadowPanel panel2;
        private Guna.UI2.WinForms.Guna2GradientButton btn_Scoreboard;
        private Guna.UI2.WinForms.Guna2GradientButton btn_Ready;
        private Guna.UI2.WinForms.Guna2GradientButton btn_ID;
        private System.Windows.Forms.PictureBox ptb_roadtrack2dup;
        private System.Windows.Forms.PictureBox ptb_increasingroad2;
        private System.Windows.Forms.PictureBox ptb_decreasingroad2;
        private System.Windows.Forms.PictureBox ptb_player1;
        private System.Windows.Forms.PictureBox ptb_player2;
        private System.Windows.Forms.PictureBox ptb_AICar6;
        private System.Windows.Forms.PictureBox ptb_AICar5;
        private System.Windows.Forms.PictureBox ptb_AICar3;
        private System.Windows.Forms.PictureBox ptb_AICar1;
        private System.Windows.Forms.PictureBox ptb_roadtrack2;
        private System.Windows.Forms.PictureBox ptb_roadtrack1;
        private System.Windows.Forms.Label lbl_P1_Status;
        private System.Windows.Forms.Label lbl_P2_Status;
        private System.Windows.Forms.Label lbl_Countdown;
        private System.Windows.Forms.Label lbl_GameTimer;
        private System.Windows.Forms.Label lbl_Score1;
        private System.Windows.Forms.Label lbl_Score2;
    }
}
