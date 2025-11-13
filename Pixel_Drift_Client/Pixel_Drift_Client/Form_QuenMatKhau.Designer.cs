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
            this.lb_laylaimk = new System.Windows.Forms.Label();
            this.lb_nhapemail = new System.Windows.Forms.Label();
            this.txt_email = new System.Windows.Forms.TextBox();
            this.btn_guimahoa = new System.Windows.Forms.Button();
            this.btn_quaylai = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_laylaimk
            // 
            this.lb_laylaimk.AutoSize = true;
            this.lb_laylaimk.Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_laylaimk.ForeColor = System.Drawing.Color.Crimson;
            this.lb_laylaimk.Location = new System.Drawing.Point(271, 27);
            this.lb_laylaimk.Name = "lb_laylaimk";
            this.lb_laylaimk.Size = new System.Drawing.Size(248, 36);
            this.lb_laylaimk.TabIndex = 0;
            this.lb_laylaimk.Text = "Lấy lại mật khẩu";
            // 
            // lb_nhapemail
            // 
            this.lb_nhapemail.AutoSize = true;
            this.lb_nhapemail.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_nhapemail.Location = new System.Drawing.Point(12, 113);
            this.lb_nhapemail.Name = "lb_nhapemail";
            this.lb_nhapemail.Size = new System.Drawing.Size(187, 25);
            this.lb_nhapemail.TabIndex = 1;
            this.lb_nhapemail.Text = "Nhập email của bạn";
            // 
            // txt_email
            // 
            this.txt_email.Location = new System.Drawing.Point(205, 114);
            this.txt_email.Name = "txt_email";
            this.txt_email.Size = new System.Drawing.Size(517, 26);
            this.txt_email.TabIndex = 2;
            // 
            // btn_guimahoa
            // 
            this.btn_guimahoa.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_guimahoa.Location = new System.Drawing.Point(137, 230);
            this.btn_guimahoa.Name = "btn_guimahoa";
            this.btn_guimahoa.Size = new System.Drawing.Size(200, 45);
            this.btn_guimahoa.TabIndex = 3;
            this.btn_guimahoa.Text = "Gửi chuỗi mã hóa";
            this.btn_guimahoa.UseVisualStyleBackColor = true;
            this.btn_guimahoa.Click += new System.EventHandler(this.btn_quenmatkhau_Click);
            // 
            // btn_quaylai
            // 
            this.btn_quaylai.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_quaylai.Location = new System.Drawing.Point(469, 230);
            this.btn_quaylai.Name = "btn_quaylai";
            this.btn_quaylai.Size = new System.Drawing.Size(200, 45);
            this.btn_quaylai.TabIndex = 4;
            this.btn_quaylai.Text = "Quay lại";
            this.btn_quaylai.UseVisualStyleBackColor = true;
            this.btn_quaylai.Click += new System.EventHandler(this.btn_quaylai_Click);
            // 
            // Form_QuenMatKhau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 343);
            this.Controls.Add(this.btn_quaylai);
            this.Controls.Add(this.btn_guimahoa);
            this.Controls.Add(this.txt_email);
            this.Controls.Add(this.lb_nhapemail);
            this.Controls.Add(this.lb_laylaimk);
            this.Name = "Form_QuenMatKhau";
            this.Text = "Form_QuenMatKhau";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_laylaimk;
        private System.Windows.Forms.Label lb_nhapemail;
        private System.Windows.Forms.TextBox txt_email;
        private System.Windows.Forms.Button btn_guimahoa;
        private System.Windows.Forms.Button btn_quaylai;
    }
}