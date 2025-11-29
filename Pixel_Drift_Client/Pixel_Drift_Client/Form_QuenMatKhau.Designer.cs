namespace Pixel_Drift
{
    partial class Form_QuenMatKhau
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
            this.lb_nhapemail = new System.Windows.Forms.Label();
            this.txt_email = new System.Windows.Forms.TextBox();
            this.btn_guimahoa = new System.Windows.Forms.Button();
            this.btn_quaylai = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_nhapemail
            // 
            this.lb_nhapemail.AutoSize = true;
            this.lb_nhapemail.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_nhapemail.Location = new System.Drawing.Point(284, 62);
            this.lb_nhapemail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_nhapemail.Name = "lb_nhapemail";
            this.lb_nhapemail.Size = new System.Drawing.Size(203, 27);
            this.lb_nhapemail.TabIndex = 1;
            this.lb_nhapemail.Text = "Nhập email của bạn";
            // 
            // txt_email
            // 
            this.txt_email.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_email.Location = new System.Drawing.Point(200, 127);
            this.txt_email.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txt_email.Name = "txt_email";
            this.txt_email.Size = new System.Drawing.Size(385, 35);
            this.txt_email.TabIndex = 2;
            // 
            // btn_guimahoa
            // 
            this.btn_guimahoa.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_guimahoa.Location = new System.Drawing.Point(276, 188);
            this.btn_guimahoa.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_guimahoa.Name = "btn_guimahoa";
            this.btn_guimahoa.Size = new System.Drawing.Size(113, 29);
            this.btn_guimahoa.TabIndex = 3;
            this.btn_guimahoa.Text = "Gửi mã xác thực";
            this.btn_guimahoa.UseVisualStyleBackColor = true;
            this.btn_guimahoa.Click += new System.EventHandler(this.btn_quenmatkhau_Click);
            // 
            // btn_quaylai
            // 
            this.btn_quaylai.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_quaylai.Location = new System.Drawing.Point(402, 188);
            this.btn_quaylai.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_quaylai.Name = "btn_quaylai";
            this.btn_quaylai.Size = new System.Drawing.Size(101, 29);
            this.btn_quaylai.TabIndex = 4;
            this.btn_quaylai.Text = "Quay lại";
            this.btn_quaylai.UseVisualStyleBackColor = true;
            this.btn_quaylai.Click += new System.EventHandler(this.btn_quaylai_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Pixel_Drift.Properties.Resources.Quen_MK;
            this.pictureBox1.Location = new System.Drawing.Point(1, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(784, 451);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // Form_QuenMatKhau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 454);
            this.Controls.Add(this.btn_quaylai);
            this.Controls.Add(this.btn_guimahoa);
            this.Controls.Add(this.txt_email);
            this.Controls.Add(this.lb_nhapemail);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form_QuenMatKhau";
            this.Text = "Quên Mật Khẩu";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lb_nhapemail;
        private System.Windows.Forms.TextBox txt_email;
        private System.Windows.Forms.Button btn_guimahoa;
        private System.Windows.Forms.Button btn_quaylai;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}