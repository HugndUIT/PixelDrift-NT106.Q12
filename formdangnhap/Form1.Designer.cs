namespace formdangnhap
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
            lb_dangnhap.Location = new Point(215, 39);
            lb_dangnhap.Name = "lb_dangnhap";
            lb_dangnhap.Size = new Size(151, 32);
            lb_dangnhap.TabIndex = 0;
            lb_dangnhap.Text = "Đăng Nhập";
            // 
            // lb_user
            // 
            lb_user.AutoSize = true;
            lb_user.Font = new Font("Times New Roman", 11F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lb_user.Location = new Point(12, 112);
            lb_user.Name = "lb_user";
            lb_user.Size = new Size(233, 25);
            lb_user.TabIndex = 1;
            lb_user.Text = "Email hoặc số điện thoại";
            lb_user.Click += lb_user_Click;
            // 
            // lb_pass
            // 
            lb_pass.AutoSize = true;
            lb_pass.Font = new Font("Times New Roman", 11F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lb_pass.Location = new Point(144, 203);
            lb_pass.Name = "lb_pass";
            lb_pass.Size = new Size(101, 25);
            lb_pass.TabIndex = 2;
            lb_pass.Text = "Mật Khẩu";
            lb_pass.Click += lb_pass_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(272, 112);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(219, 31);
            textBox1.TabIndex = 3;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(272, 203);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(219, 31);
            textBox2.TabIndex = 4;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // btn_vaogame
            // 
            btn_vaogame.Font = new Font("Times New Roman", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_vaogame.Location = new Point(237, 297);
            btn_vaogame.Name = "btn_vaogame";
            btn_vaogame.Size = new Size(112, 34);
            btn_vaogame.TabIndex = 5;
            btn_vaogame.Text = "Đăng Nhập";
            btn_vaogame.UseVisualStyleBackColor = true;
            btn_vaogame.Click += btn_vaogame_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 255, 128);
            ClientSize = new Size(611, 425);
            Controls.Add(btn_vaogame);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(lb_pass);
            Controls.Add(lb_user);
            Controls.Add(lb_dangnhap);
            Name = "Form1";
            Text = "Pixel Driver";
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
