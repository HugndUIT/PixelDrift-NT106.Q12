using System.Drawing;
using System.Windows.Forms;

namespace Pixel_Drift
{
    partial class Form_Thong_Tin
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
            this.lbl_Birthday = new System.Windows.Forms.Label();
            this.lbl_TieuDe = new System.Windows.Forms.Label();
            this.lbl_Email = new System.Windows.Forms.Label();
            this.lbl_TenDangNhap = new System.Windows.Forms.Label();
            this.btnVaoGame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_Birthday
            // 
            this.lbl_Birthday.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Birthday.Location = new System.Drawing.Point(34, 148);
            this.lbl_Birthday.Name = "lbl_Birthday";
            this.lbl_Birthday.Size = new System.Drawing.Size(379, 32);
            this.lbl_Birthday.TabIndex = 2;
            this.lbl_Birthday.Text = "Birthday: ";
            // 
            // lbl_TieuDe
            // 
            this.lbl_TieuDe.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_TieuDe.Font = new System.Drawing.Font("Times New Roman", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TieuDe.Location = new System.Drawing.Point(0, 0);
            this.lbl_TieuDe.Name = "lbl_TieuDe";
            this.lbl_TieuDe.Size = new System.Drawing.Size(606, 119);
            this.lbl_TieuDe.TabIndex = 1;
            this.lbl_TieuDe.Text = "Thông tin người dùng";
            this.lbl_TieuDe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Email
            // 
            this.lbl_Email.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Email.Location = new System.Drawing.Point(34, 332);
            this.lbl_Email.Name = "lbl_Email";
            this.lbl_Email.Size = new System.Drawing.Size(435, 32);
            this.lbl_Email.TabIndex = 4;
            this.lbl_Email.Text = "Email/Sđt: ";
            // 
            // lbl_TenDangNhap
            // 
            this.lbl_TenDangNhap.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TenDangNhap.Location = new System.Drawing.Point(34, 235);
            this.lbl_TenDangNhap.Name = "lbl_TenDangNhap";
            this.lbl_TenDangNhap.Size = new System.Drawing.Size(379, 32);
            this.lbl_TenDangNhap.TabIndex = 3;
            this.lbl_TenDangNhap.Text = "Username: ";
            // 
            // btnVaoGame
            // 
            this.btnVaoGame.BackColor = System.Drawing.Color.Firebrick;
            this.btnVaoGame.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVaoGame.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnVaoGame.Location = new System.Drawing.Point(204, 458);
            this.btnVaoGame.Margin = new System.Windows.Forms.Padding(4);
            this.btnVaoGame.Name = "btnVaoGame";
            this.btnVaoGame.Size = new System.Drawing.Size(168, 45);
            this.btnVaoGame.TabIndex = 0;
            this.btnVaoGame.Text = "Vào game";
            this.btnVaoGame.UseVisualStyleBackColor = false;
            this.btnVaoGame.Click += new System.EventHandler(this.btnThoat_Click_1);
            // 
            // Form_Thong_Tin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(606, 550);
            this.Controls.Add(this.lbl_Email);
            this.Controls.Add(this.lbl_TenDangNhap);
            this.Controls.Add(this.lbl_Birthday);
            this.Controls.Add(this.lbl_TieuDe);
            this.Controls.Add(this.btnVaoGame);
            this.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form_Thong_Tin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông tin ";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_Birthday;
        private System.Windows.Forms.Label lbl_TieuDe;
        private System.Windows.Forms.Label lbl_Email;
        private System.Windows.Forms.Label lbl_TenDangNhap;
        private System.Windows.Forms.Button btnVaoGame;
    }
}
