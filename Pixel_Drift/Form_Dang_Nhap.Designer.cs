using System.Drawing;
using System.Windows.Forms;

namespace Pixel_Drift
{
    partial class Form_Dang_Nhap
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
            this.lb_pass = new System.Windows.Forms.Label();
            this.lb_user = new System.Windows.Forms.Label();
            this.lb_dangnhap = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btn_vaogame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_dangnhap
            // 
            lb_dangnhap.AutoSize = true;
            lb_dangnhap.Font = new Font("Times New Roman", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lb_dangnhap.Location = new Point(150, 23);
            lb_dangnhap.Margin = new Padding(2, 0, 2, 0);
            lb_dangnhap.Name = "lb_dangnhap";
            lb_dangnhap.Size = new Size(103, 22);
            lb_dangnhap.TabIndex = 0;
            lb_dangnhap.Text = "Đăng Nhập";
            // 
            // lb_user
            // 
            this.lb_user.AutoSize = true;
            this.lb_user.Font = new System.Drawing.Font("Times New Roman", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lb_user.Location = new System.Drawing.Point(4, 71);
            this.lb_user.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_user.Name = "lb_user";
            this.lb_user.Size = new System.Drawing.Size(209, 22);
            this.lb_user.TabIndex = 1;
            this.lb_user.Text = "Email hoặc số điện thoại";
            // 
            // lb_pass
            // 
            lb_pass.AutoSize = true;
            lb_pass.Font = new Font("Times New Roman", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lb_pass.Location = new Point(108, 122);
            lb_pass.Margin = new Padding(2, 0, 2, 0);
            lb_pass.Name = "lb_pass";
            lb_pass.Size = new Size(76, 19);
            lb_pass.TabIndex = 2;
            lb_pass.Text = "Mật Khẩu";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(190, 67);
            textBox1.Margin = new Padding(2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(154, 23);
            textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(190, 122);
            textBox2.Margin = new Padding(2);
            textBox2.Name = "textBox2";
            textBox2.PasswordChar = '*';
            textBox2.Size = new Size(154, 23);
            textBox2.TabIndex = 4;
            // 
            // btn_vaogame
            // 
            btn_vaogame.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_vaogame.Location = new Point(150, 177);
            btn_vaogame.Margin = new Padding(2);
            btn_vaogame.Name = "btn_vaogame";
            btn_vaogame.Size = new Size(111, 31);
            btn_vaogame.TabIndex = 5;
            btn_vaogame.Text = "Đăng Nhập";
            btn_vaogame.UseVisualStyleBackColor = true;
            btn_vaogame.Click += btn_vaogame_Click;
            // 
            // Form_Dang_Nhap
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 255, 192);
            ClientSize = new Size(428, 255);
            Controls.Add(btn_vaogame);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(lb_pass);
            Controls.Add(lb_user);
            Controls.Add(lb_dangnhap);
            ForeColor = Color.DarkGoldenrod;
            Margin = new Padding(2);
            Name = "Form_Dang_Nhap";
            Text = "Pixel Drift";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lb_pass;
        private System.Windows.Forms.Label lb_user;
        private System.Windows.Forms.Label lb_dangnhap;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btn_vaogame;
    }
}
