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
            this.lbl_Indentify = new System.Windows.Forms.Label();
            this.lbl_TieuDe = new System.Windows.Forms.Label();
            this.lbl_Email = new System.Windows.Forms.Label();
            this.lbl_TenDangNhap = new System.Windows.Forms.Label();
            this.btnThoat = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnThoat
            // 
            btnThoat.BackColor = Color.Firebrick;
            btnThoat.Font = new Font("Times New Roman", 14F, FontStyle.Italic, GraphicsUnit.Point, 0);
            btnThoat.ForeColor = SystemColors.ButtonFace;
            btnThoat.Location = new Point(219, 471);
            btnThoat.Margin = new Padding(4);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(168, 45);
            btnThoat.TabIndex = 0;
            btnThoat.Text = "Thoát";
            btnThoat.UseVisualStyleBackColor = false;
            btnThoat.Click += btnThoat_Click;
            // 
            // lblTieuDe
            // 
            lbl_TieuDe.Dock = DockStyle.Top;
            lbl_TieuDe.Font = new Font("Times New Roman", 18F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lbl_TieuDe.Location = new Point(0, 0);
            lbl_TieuDe.Name = "lblTieuDe";
            lbl_TieuDe.Size = new Size(606, 119);
            lbl_TieuDe.TabIndex = 1;
            lbl_TieuDe.Text = "Thông tin người dùng";
            lbl_TieuDe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbl_Indentify
            // 
            lbl_Indentify.Font = new Font("Times New Roman", 14F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lbl_Indentify.Location = new Point(34, 148);
            lbl_Indentify.Name = "lbl_Indentify";
            lbl_Indentify.Size = new Size(379, 32);
            lbl_Indentify.TabIndex = 2;
            lbl_Indentify.Text = "ID: ";
            // 
            // lbl_TenDangNhap
            // 
            lbl_TenDangNhap.Font = new Font("Times New Roman", 14F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lbl_TenDangNhap.Location = new Point(34, 235);
            lbl_TenDangNhap.Name = "lbl_TenDangNhap";
            lbl_TenDangNhap.Size = new Size(379, 32);
            lbl_TenDangNhap.TabIndex = 3;
            lbl_TenDangNhap.Text = "Username: ";
            // 
            // lbl_Email
            // 
            lbl_Email.Font = new Font("Times New Roman", 14F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lbl_Email.Location = new Point(34, 332);
            lbl_Email.Name = "lbl_Email";
            lbl_Email.Size = new Size(435, 32);
            lbl_Email.TabIndex = 4;
            lbl_Email.Text = "Email/Sđt: ";
            // 
            // frmThongTin
            // 
            AutoScaleDimensions = new SizeF(15F, 33F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 255, 192);
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(606, 550);
            Controls.Add(lbl_Email);
            Controls.Add(lbl_TenDangNhap);
            Controls.Add(lbl_Indentify);
            Controls.Add(lbl_TieuDe);
            Controls.Add(btnThoat);
            Font = new Font("Times New Roman", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.DarkGoldenrod;
            Margin = new Padding(4);
            Name = "frmThongTin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thông tin ";
            Load += frmThongTin_Load;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lbl_Indentify;
        private System.Windows.Forms.Label lbl_TieuDe;
        private System.Windows.Forms.Label lbl_Email;
        private System.Windows.Forms.Label lbl_TenDangNhap;
        private System.Windows.Forms.Button btnThoat;
    }
}