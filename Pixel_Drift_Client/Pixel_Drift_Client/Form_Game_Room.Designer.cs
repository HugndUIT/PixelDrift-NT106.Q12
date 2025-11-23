namespace Pixel_Drift
{
    partial class Form_Game_Room
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
            this.btn_join_room = new System.Windows.Forms.Button();
            this.btn_create_room = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_join_room
            // 
            this.btn_join_room.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_join_room.Location = new System.Drawing.Point(12, 12);
            this.btn_join_room.Name = "btn_join_room";
            this.btn_join_room.Size = new System.Drawing.Size(776, 121);
            this.btn_join_room.TabIndex = 0;
            this.btn_join_room.Text = "Join Room";
            this.btn_join_room.UseVisualStyleBackColor = true;
            this.btn_join_room.Click += new System.EventHandler(this.btn_join_room_Click);
            // 
            // btn_create_room
            // 
            this.btn_create_room.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_create_room.Location = new System.Drawing.Point(12, 139);
            this.btn_create_room.Name = "btn_create_room";
            this.btn_create_room.Size = new System.Drawing.Size(776, 121);
            this.btn_create_room.TabIndex = 1;
            this.btn_create_room.Text = "Create Room";
            this.btn_create_room.UseVisualStyleBackColor = true;
            this.btn_create_room.Click += new System.EventHandler(this.btn_create_room_Click);
            // 
            // Form_Game_Room
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 267);
            this.Controls.Add(this.btn_create_room);
            this.Controls.Add(this.btn_join_room);
            this.Name = "Form_Game_Room";
            this.Text = "Form_Game_Room";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_join_room;
        private System.Windows.Forms.Button btn_create_room;
    }
}