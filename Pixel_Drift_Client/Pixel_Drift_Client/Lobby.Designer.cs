namespace Pixel_Drift
{
    partial class Lobby
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
            this.ptb_Lobby = new System.Windows.Forms.PictureBox();
            this.btn_CreateRoom = new System.Windows.Forms.Button();
            this.btn_JoinRoom = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Lobby)).BeginInit();
            this.SuspendLayout();
            // 
            // ptb_Lobby
            // 
            this.ptb_Lobby.Image = global::Pixel_Drift.Properties.Resources.Gemini_Generated_Image_afaixrafaixrafai;
            this.ptb_Lobby.Location = new System.Drawing.Point(0, 0);
            this.ptb_Lobby.Name = "ptb_Lobby";
            this.ptb_Lobby.Size = new System.Drawing.Size(802, 453);
            this.ptb_Lobby.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_Lobby.TabIndex = 0;
            this.ptb_Lobby.TabStop = false;
            // 
            // btn_CreateRoom
            // 
            this.btn_CreateRoom.BackColor = System.Drawing.Color.Transparent;
            this.btn_CreateRoom.FlatAppearance.BorderSize = 0;
            this.btn_CreateRoom.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_CreateRoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CreateRoom.Font = new System.Drawing.Font("Consolas", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CreateRoom.ForeColor = System.Drawing.Color.DarkCyan;
            this.btn_CreateRoom.Location = new System.Drawing.Point(222, 144);
            this.btn_CreateRoom.Name = "btn_CreateRoom";
            this.btn_CreateRoom.Size = new System.Drawing.Size(164, 83);
            this.btn_CreateRoom.TabIndex = 1;
            this.btn_CreateRoom.Text = "CREATE ROOM";
            this.btn_CreateRoom.UseVisualStyleBackColor = false;
            this.btn_CreateRoom.Click += new System.EventHandler(this.btn_CreateRoom_Click);
            // 
            // btn_JoinRoom
            // 
            this.btn_JoinRoom.BackColor = System.Drawing.Color.Transparent;
            this.btn_JoinRoom.FlatAppearance.BorderSize = 0;
            this.btn_JoinRoom.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_JoinRoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_JoinRoom.Font = new System.Drawing.Font("Consolas", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_JoinRoom.ForeColor = System.Drawing.Color.DarkCyan;
            this.btn_JoinRoom.Location = new System.Drawing.Point(416, 144);
            this.btn_JoinRoom.Name = "btn_JoinRoom";
            this.btn_JoinRoom.Size = new System.Drawing.Size(164, 83);
            this.btn_JoinRoom.TabIndex = 2;
            this.btn_JoinRoom.Text = "JOIN ROOM";
            this.btn_JoinRoom.UseVisualStyleBackColor = false;
            this.btn_JoinRoom.Click += new System.EventHandler(this.btn_JoinRoom_Click);
            // 
            // Lobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_JoinRoom);
            this.Controls.Add(this.btn_CreateRoom);
            this.Controls.Add(this.ptb_Lobby);
            this.Name = "Lobby";
            this.Text = "Lobby";
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Lobby)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ptb_Lobby;
        private System.Windows.Forms.Button btn_CreateRoom;
        private System.Windows.Forms.Button btn_JoinRoom;
    }
}