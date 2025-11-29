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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Dang_Nhap));
            this.lb_pass = new System.Windows.Forms.Label();
            this.lb_user = new System.Windows.Forms.Label();
            this.lb_dangnhap = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btn_vaogame = new System.Windows.Forms.Button();
            this.btn_quenmatkhau = new System.Windows.Forms.Button();
            this.btn_backdk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_pass
            // 
            this.lb_pass.AutoSize = true;
            this.lb_pass.BackColor = System.Drawing.Color.Transparent;
            this.lb_pass.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_pass.ForeColor = System.Drawing.Color.White;
            this.lb_pass.Location = new System.Drawing.Point(126, 162);
            this.lb_pass.Name = "lb_pass";
            this.lb_pass.Size = new System.Drawing.Size(108, 28);
            this.lb_pass.TabIndex = 2;
            this.lb_pass.Text = "Mật khẩu";
            // 
            // lb_user
            // 
            this.lb_user.AutoSize = true;
            this.lb_user.BackColor = System.Drawing.Color.Transparent;
            this.lb_user.Font = new System.Drawing.Font("Times New Roman", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lb_user.ForeColor = System.Drawing.Color.White;
            this.lb_user.Location = new System.Drawing.Point(84, 90);
            this.lb_user.Name = "lb_user";
            this.lb_user.Size = new System.Drawing.Size(150, 25);
            this.lb_user.TabIndex = 1;
            this.lb_user.Text = "Tên đăng nhập";
            // 
            // lb_dangnhap
            // 
            this.lb_dangnhap.AutoSize = true;
            this.lb_dangnhap.BackColor = System.Drawing.Color.Transparent;
            this.lb_dangnhap.Font = new System.Drawing.Font("Times New Roman", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_dangnhap.ForeColor = System.Drawing.Color.Yellow;
            this.lb_dangnhap.Location = new System.Drawing.Point(194, 31);
            this.lb_dangnhap.Name = "lb_dangnhap";
            this.lb_dangnhap.Size = new System.Drawing.Size(176, 37);
            this.lb_dangnhap.TabIndex = 0;
            this.lb_dangnhap.Text = "Đăng Nhập";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(244, 90);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(276, 26);
            this.textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(244, 162);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.PasswordChar = '*';
            this.textBox2.Size = new System.Drawing.Size(276, 26);
            this.textBox2.TabIndex = 4;
            // 
            // btn_vaogame
            // 
            this.btn_vaogame.BackColor = System.Drawing.Color.Transparent;
            this.btn_vaogame.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_vaogame.ForeColor = System.Drawing.Color.Black;
            this.btn_vaogame.Location = new System.Drawing.Point(199, 222);
            this.btn_vaogame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_vaogame.Name = "btn_vaogame";
            this.btn_vaogame.Size = new System.Drawing.Size(143, 41);
            this.btn_vaogame.TabIndex = 5;
            this.btn_vaogame.Text = "Đăng Nhập";
            this.btn_vaogame.UseVisualStyleBackColor = false;
            this.btn_vaogame.Click += new System.EventHandler(this.btn_vaogame_Click);
            // 
            // btn_quenmatkhau
            // 
            this.btn_quenmatkhau.BackColor = System.Drawing.Color.Transparent;
            this.btn_quenmatkhau.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_quenmatkhau.FlatAppearance.BorderSize = 0;
            this.btn_quenmatkhau.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_quenmatkhau.ForeColor = System.Drawing.Color.Orange;
            this.btn_quenmatkhau.Location = new System.Drawing.Point(70, 281);
            this.btn_quenmatkhau.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_quenmatkhau.Name = "btn_quenmatkhau";
            this.btn_quenmatkhau.Size = new System.Drawing.Size(195, 45);
            this.btn_quenmatkhau.TabIndex = 0;
            this.btn_quenmatkhau.Text = "Quên / Đổi mật khẩu ";
            this.btn_quenmatkhau.UseVisualStyleBackColor = false;
            this.btn_quenmatkhau.Click += new System.EventHandler(this.btn_quenmatkhau_Click);
            // 
            // btn_backdk
            // 
            this.btn_backdk.BackColor = System.Drawing.Color.Transparent;
            this.btn_backdk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_backdk.FlatAppearance.BorderSize = 0;
            this.btn_backdk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_backdk.ForeColor = System.Drawing.Color.Orange;
            this.btn_backdk.Location = new System.Drawing.Point(271, 281);
            this.btn_backdk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_backdk.Name = "btn_backdk";
            this.btn_backdk.Size = new System.Drawing.Size(242, 45);
            this.btn_backdk.TabIndex = 6;
            this.btn_backdk.Text = "Chưa có tài khoản? Đăng kí";
            this.btn_backdk.UseVisualStyleBackColor = false;
            this.btn_backdk.Click += new System.EventHandler(this.btn_backdk_Click);
            // 
            // Form_Dang_Nhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(550, 356);
            this.Controls.Add(this.btn_backdk);
            this.Controls.Add(this.btn_quenmatkhau);
            this.Controls.Add(this.btn_vaogame);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lb_pass);
            this.Controls.Add(this.lb_user);
            this.Controls.Add(this.lb_dangnhap);
            this.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form_Dang_Nhap";
            this.Text = "Pixel Drift";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_pass;
        private System.Windows.Forms.Label lb_user;
        private System.Windows.Forms.Label lb_dangnhap;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btn_vaogame;
        private Button btn_quenmatkhau;
        private Button btn_backdk;
    }
}
