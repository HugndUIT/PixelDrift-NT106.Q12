namespace Pixel_Drift
{
    partial class Form_DoiMatKhau
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
            this.lb_doimatkhau = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_mkmoi = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_mkcu = new System.Windows.Forms.TextBox();
            this.txt_mkmoi = new System.Windows.Forms.TextBox();
            this.txt_xacnhanmk = new System.Windows.Forms.TextBox();
            this.btn_doimk = new System.Windows.Forms.Button();
            this.btn_thoat = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_doimatkhau
            // 
            this.lb_doimatkhau.AutoSize = true;
            this.lb_doimatkhau.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_doimatkhau.ForeColor = System.Drawing.Color.Red;
            this.lb_doimatkhau.Location = new System.Drawing.Point(308, 20);
            this.lb_doimatkhau.Name = "lb_doimatkhau";
            this.lb_doimatkhau.Size = new System.Drawing.Size(176, 32);
            this.lb_doimatkhau.TabIndex = 0;
            this.lb_doimatkhau.Text = "Đổi mật khẩu";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(280, 22);
            this.label2.TabIndex = 1;
            this.label2.Text = "Nhập chuỗi mã hóa đã gửi về mail";
            // 
            // lb_mkmoi
            // 
            this.lb_mkmoi.AutoSize = true;
            this.lb_mkmoi.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_mkmoi.Location = new System.Drawing.Point(13, 166);
            this.lb_mkmoi.Name = "lb_mkmoi";
            this.lb_mkmoi.Size = new System.Drawing.Size(162, 22);
            this.lb_mkmoi.TabIndex = 2;
            this.lb_mkmoi.Text = "Nhập mật khẩu mới";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 273);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(193, 22);
            this.label4.TabIndex = 3;
            this.label4.Text = "Xác nhận mật khẩu mới";
            // 
            // txt_mkcu
            // 
            this.txt_mkcu.Location = new System.Drawing.Point(26, 84);
            this.txt_mkcu.Name = "txt_mkcu";
            this.txt_mkcu.Size = new System.Drawing.Size(337, 26);
            this.txt_mkcu.TabIndex = 4;
            // 
            // txt_mkmoi
            // 
            this.txt_mkmoi.Location = new System.Drawing.Point(26, 191);
            this.txt_mkmoi.Name = "txt_mkmoi";
            this.txt_mkmoi.Size = new System.Drawing.Size(337, 26);
            this.txt_mkmoi.TabIndex = 5;
            // 
            // txt_xacnhanmk
            // 
            this.txt_xacnhanmk.Location = new System.Drawing.Point(26, 299);
            this.txt_xacnhanmk.Name = "txt_xacnhanmk";
            this.txt_xacnhanmk.Size = new System.Drawing.Size(337, 26);
            this.txt_xacnhanmk.TabIndex = 6;
            // 
            // btn_doimk
            // 
            this.btn_doimk.BackColor = System.Drawing.Color.Green;
            this.btn_doimk.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_doimk.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_doimk.Location = new System.Drawing.Point(455, 115);
            this.btn_doimk.Name = "btn_doimk";
            this.btn_doimk.Size = new System.Drawing.Size(143, 85);
            this.btn_doimk.TabIndex = 7;
            this.btn_doimk.Text = "Đổi mật khẩu";
            this.btn_doimk.UseVisualStyleBackColor = false;
            this.btn_doimk.Click += new System.EventHandler(this.btn_doimk_Click);
            // 
            // btn_thoat
            // 
            this.btn_thoat.BackColor = System.Drawing.Color.Red;
            this.btn_thoat.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_thoat.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_thoat.Location = new System.Drawing.Point(455, 241);
            this.btn_thoat.Name = "btn_thoat";
            this.btn_thoat.Size = new System.Drawing.Size(143, 54);
            this.btn_thoat.TabIndex = 8;
            this.btn_thoat.Text = "Thoát";
            this.btn_thoat.UseVisualStyleBackColor = false;
            this.btn_thoat.Click += new System.EventHandler(this.btn_thoat_Click);
            // 
            // Form_DoiMatKhau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 352);
            this.Controls.Add(this.btn_thoat);
            this.Controls.Add(this.btn_doimk);
            this.Controls.Add(this.txt_xacnhanmk);
            this.Controls.Add(this.txt_mkmoi);
            this.Controls.Add(this.txt_mkcu);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lb_mkmoi);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lb_doimatkhau);
            this.Name = "Form_DoiMatKhau";
            this.Text = "Form_DoiMatKhau";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_doimatkhau;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_mkmoi;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_mkcu;
        private System.Windows.Forms.TextBox txt_mkmoi;
        private System.Windows.Forms.TextBox txt_xacnhanmk;
        private System.Windows.Forms.Button btn_doimk;
        private System.Windows.Forms.Button btn_thoat;
    }
}