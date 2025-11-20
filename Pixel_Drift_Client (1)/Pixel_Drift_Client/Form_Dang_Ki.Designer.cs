using System.Drawing;

namespace Pixel_Drift
{
    partial class Form_Dang_Ki
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Dang_Ki));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_emailsdt = new System.Windows.Forms.TextBox();
            this.tb_xacnhanmk = new System.Windows.Forms.TextBox();
            this.tb_matkhau = new System.Windows.Forms.TextBox();
            this.tb_tendangnhap = new System.Windows.Forms.TextBox();
            this.btn_xacnhan = new System.Windows.Forms.Button();
            this.tb_BirthDay = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_backdn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Orange;
            this.label1.Location = new System.Drawing.Point(94, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 34);
            this.label1.TabIndex = 0;
            this.label1.Text = "Đăng Kí";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.OrangeRed;
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Email *";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.OrangeRed;
            this.label3.Location = new System.Drawing.Point(12, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Tên đăng nhập *";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.OrangeRed;
            this.label4.Location = new System.Drawing.Point(14, 229);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Mật khẩu *";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.OrangeRed;
            this.label5.Location = new System.Drawing.Point(14, 284);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(161, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Xác nhận mật khẩu *";
            // 
            // tb_emailsdt
            // 
            this.tb_emailsdt.Location = new System.Drawing.Point(16, 90);
            this.tb_emailsdt.Name = "tb_emailsdt";
            this.tb_emailsdt.Size = new System.Drawing.Size(308, 22);
            this.tb_emailsdt.TabIndex = 5;
            // 
            // tb_xacnhanmk
            // 
            this.tb_xacnhanmk.Location = new System.Drawing.Point(18, 310);
            this.tb_xacnhanmk.Name = "tb_xacnhanmk";
            this.tb_xacnhanmk.PasswordChar = '*';
            this.tb_xacnhanmk.Size = new System.Drawing.Size(308, 22);
            this.tb_xacnhanmk.TabIndex = 8;
            // 
            // tb_matkhau
            // 
            this.tb_matkhau.Location = new System.Drawing.Point(18, 255);
            this.tb_matkhau.Name = "tb_matkhau";
            this.tb_matkhau.PasswordChar = '*';
            this.tb_matkhau.Size = new System.Drawing.Size(308, 22);
            this.tb_matkhau.TabIndex = 7;
            // 
            // tb_tendangnhap
            // 
            this.tb_tendangnhap.Location = new System.Drawing.Point(18, 200);
            this.tb_tendangnhap.Name = "tb_tendangnhap";
            this.tb_tendangnhap.Size = new System.Drawing.Size(308, 22);
            this.tb_tendangnhap.TabIndex = 6;
            // 
            // btn_xacnhan
            // 
            this.btn_xacnhan.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_xacnhan.ForeColor = System.Drawing.Color.DarkOrange;
            this.btn_xacnhan.Location = new System.Drawing.Point(100, 343);
            this.btn_xacnhan.Name = "btn_xacnhan";
            this.btn_xacnhan.Size = new System.Drawing.Size(143, 43);
            this.btn_xacnhan.TabIndex = 9;
            this.btn_xacnhan.Text = "Xác nhận";
            this.btn_xacnhan.UseVisualStyleBackColor = true;
            this.btn_xacnhan.Click += new System.EventHandler(this.btn_xacnhan_Click);
            // 
            // tb_BirthDay
            // 
            this.tb_BirthDay.Location = new System.Drawing.Point(16, 145);
            this.tb_BirthDay.Name = "tb_BirthDay";
            this.tb_BirthDay.Size = new System.Drawing.Size(308, 22);
            this.tb_BirthDay.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.OrangeRed;
            this.label6.Location = new System.Drawing.Point(14, 119);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 20);
            this.label6.TabIndex = 4;
            this.label6.Text = "Ngày sinh";
            // 
            // btn_backdn
            // 
            this.btn_backdn.BackColor = System.Drawing.Color.Transparent;
            this.btn_backdn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_backdn.FlatAppearance.BorderSize = 0;
            this.btn_backdn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_backdn.ForeColor = System.Drawing.Color.Chartreuse;
            this.btn_backdn.Location = new System.Drawing.Point(83, 391);
            this.btn_backdn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_backdn.Name = "btn_backdn";
            this.btn_backdn.Size = new System.Drawing.Size(179, 32);
            this.btn_backdn.TabIndex = 10;
            this.btn_backdn.Text = "Quay lại đăng nhập";
            this.btn_backdn.UseVisualStyleBackColor = false;
            this.btn_backdn.Click += new System.EventHandler(this.btn_backdn_Click);
            // 
            // Form_Dang_Ki
            // 
            this.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(337, 425);
            this.Controls.Add(this.btn_backdn);
            this.Controls.Add(this.btn_xacnhan);
            this.Controls.Add(this.tb_BirthDay);
            this.Controls.Add(this.tb_xacnhanmk);
            this.Controls.Add(this.tb_matkhau);
            this.Controls.Add(this.tb_tendangnhap);
            this.Controls.Add(this.tb_emailsdt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Name = "Form_Dang_Ki";
            this.Text = "Form Đăng Kí";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_emailsdt;
        private System.Windows.Forms.TextBox tb_xacnhanmk;
        private System.Windows.Forms.TextBox tb_matkhau;
        private System.Windows.Forms.TextBox tb_tendangnhap;
        private System.Windows.Forms.Button btn_xacnhan;
        private System.Windows.Forms.TextBox tb_BirthDay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_backdn;
    }
}
