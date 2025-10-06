using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Font = System.Drawing.Font;

namespace PixelDirft
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lb_dangnhap = new Label();
            lb_user = new Label();
            lb_pass = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            btn_vaogame = new Button();
            SuspendLayout();
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
            lb_dangnhap.Click += lb_dangnhap_Click;
            // 
            // lb_user
            // 
            lb_user.AutoSize = true;
            lb_user.Font = new Font("Times New Roman", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lb_user.Location = new Point(8, 67);
            lb_user.Margin = new Padding(2, 0, 2, 0);
            lb_user.Name = "lb_user";
            lb_user.Size = new Size(176, 19);
            lb_user.TabIndex = 1;
            lb_user.Text = "Email hoặc số điện thoại";
            lb_user.Click += lb_user_Click;
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
            lb_pass.Click += lb_pass_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(190, 67);
            textBox1.Margin = new Padding(2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(154, 23);
            textBox1.TabIndex = 3;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(190, 122);
            textBox2.Margin = new Padding(2);
            textBox2.Name = "textBox2";
            textBox2.PasswordChar = '*';
            textBox2.Size = new Size(154, 23);
            textBox2.TabIndex = 4;
            textBox2.TextChanged += textBox2_TextChanged;
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

        private Label lb_dangnhap;
            private Label lb_user;
            private Label lb_pass;
            private TextBox textBox1;
            private TextBox textBox2;
            private Button btn_vaogame;
        }
    }
