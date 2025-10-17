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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ptb_roadtrack1dup = new System.Windows.Forms.PictureBox();
            this.ptb_roadtrack1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ptb_roadtrack2dup = new System.Windows.Forms.PictureBox();
            this.ptb_roadtrack2 = new System.Windows.Forms.PictureBox();
            this.btn_startgame = new System.Windows.Forms.Button();
            this.game_timer = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack1dup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack2dup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Controls.Add(this.ptb_roadtrack1dup);
            this.panel1.Controls.Add(this.ptb_roadtrack1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(617, 733);
            this.panel1.TabIndex = 0;
            // 
            // ptb_roadtrack1dup
            // 
            this.ptb_roadtrack1dup.Image = global::Pixel_Drift.Properties.Resources.road;
            this.ptb_roadtrack1dup.Location = new System.Drawing.Point(-2, -733);
            this.ptb_roadtrack1dup.Name = "ptb_roadtrack1dup";
            this.ptb_roadtrack1dup.Size = new System.Drawing.Size(625, 733);
            this.ptb_roadtrack1dup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_roadtrack1dup.TabIndex = 1;
            this.ptb_roadtrack1dup.TabStop = false;
            // 
            // ptb_roadtrack1
            // 
            this.ptb_roadtrack1.Image = global::Pixel_Drift.Properties.Resources.road;
            this.ptb_roadtrack1.Location = new System.Drawing.Point(0, -733);
            this.ptb_roadtrack1.Name = "ptb_roadtrack1";
            this.ptb_roadtrack1.Size = new System.Drawing.Size(625, 733);
            this.ptb_roadtrack1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_roadtrack1.TabIndex = 0;
            this.ptb_roadtrack1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel2.Controls.Add(this.ptb_roadtrack2dup);
            this.panel2.Controls.Add(this.ptb_roadtrack2);
            this.panel2.Location = new System.Drawing.Point(635, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(635, 733);
            this.panel2.TabIndex = 1;
            // 
            // ptb_roadtrack2dup
            // 
            this.ptb_roadtrack2dup.Image = global::Pixel_Drift.Properties.Resources.road;
            this.ptb_roadtrack2dup.Location = new System.Drawing.Point(3, 733);
            this.ptb_roadtrack2dup.Name = "ptb_roadtrack2dup";
            this.ptb_roadtrack2dup.Size = new System.Drawing.Size(625, 733);
            this.ptb_roadtrack2dup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_roadtrack2dup.TabIndex = 2;
            this.ptb_roadtrack2dup.TabStop = false;
            // 
            // ptb_roadtrack2
            // 
            this.ptb_roadtrack2.Image = global::Pixel_Drift.Properties.Resources.road;
            this.ptb_roadtrack2.Location = new System.Drawing.Point(5, -733);
            this.ptb_roadtrack2.Name = "ptb_roadtrack2";
            this.ptb_roadtrack2.Size = new System.Drawing.Size(625, 733);
            this.ptb_roadtrack2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_roadtrack2.TabIndex = 1;
            this.ptb_roadtrack2.TabStop = false;
            // 
            // btn_startgame
            // 
            this.btn_startgame.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btn_startgame.Location = new System.Drawing.Point(466, 754);
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
            // Game_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1282, 900);
            this.Controls.Add(this.btn_startgame);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Game_Window";
            this.Text = "Game_Window";
            this.Load += new System.EventHandler(this.Game_Window_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack1dup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack2dup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_roadtrack2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox ptb_roadtrack1;
        private System.Windows.Forms.PictureBox ptb_roadtrack2;
        private System.Windows.Forms.PictureBox ptb_roadtrack1dup;
        private System.Windows.Forms.PictureBox ptb_roadtrack2dup;
        private System.Windows.Forms.Button btn_startgame;
        private System.Windows.Forms.Timer game_timer;
    }
}