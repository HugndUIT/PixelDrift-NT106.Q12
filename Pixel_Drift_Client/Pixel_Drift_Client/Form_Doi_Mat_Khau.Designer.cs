namespace Pixel_Drift
{
    partial class Form_Doi_Mat_Khau
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
            this.label2 = new System.Windows.Forms.Label();
            this.lb_mkmoi = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_mkcu = new System.Windows.Forms.TextBox();
            this.txt_mkmoi = new System.Windows.Forms.TextBox();
            this.txt_xacnhanmk = new System.Windows.Forms.TextBox();
            this.btn_doimk = new System.Windows.Forms.Button();
            this.btn_thoat = new System.Windows.Forms.Button();
            this.ptb_Doi_MK = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Doi_MK)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(191, 42);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Nhập token được gửi về email";
            // 
            // lb_mkmoi
            // 
            this.lb_mkmoi.AutoSize = true;
            this.lb_mkmoi.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_mkmoi.Location = new System.Drawing.Point(191, 93);
            this.lb_mkmoi.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_mkmoi.Name = "lb_mkmoi";
            this.lb_mkmoi.Size = new System.Drawing.Size(118, 16);
            this.lb_mkmoi.TabIndex = 2;
            this.lb_mkmoi.Text = "Nhập mật khẩu mới";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(191, 144);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Xác nhận mật khẩu mới";
            // 
            // txt_mkcu
            // 
            this.txt_mkcu.Location = new System.Drawing.Point(194, 111);
            this.txt_mkcu.Margin = new System.Windows.Forms.Padding(2);
            this.txt_mkcu.Multiline = true;
            this.txt_mkcu.Name = "txt_mkcu";
            this.txt_mkcu.Size = new System.Drawing.Size(268, 31);
            this.txt_mkcu.TabIndex = 4;
            // 
            // txt_mkmoi
            // 
            this.txt_mkmoi.Location = new System.Drawing.Point(194, 162);
            this.txt_mkmoi.Margin = new System.Windows.Forms.Padding(2);
            this.txt_mkmoi.Multiline = true;
            this.txt_mkmoi.Name = "txt_mkmoi";
            this.txt_mkmoi.Size = new System.Drawing.Size(268, 31);
            this.txt_mkmoi.TabIndex = 5;
            // 
            // txt_xacnhanmk
            // 
            this.txt_xacnhanmk.Location = new System.Drawing.Point(194, 60);
            this.txt_xacnhanmk.Margin = new System.Windows.Forms.Padding(2);
            this.txt_xacnhanmk.Multiline = true;
            this.txt_xacnhanmk.Name = "txt_xacnhanmk";
            this.txt_xacnhanmk.Size = new System.Drawing.Size(268, 31);
            this.txt_xacnhanmk.TabIndex = 6;
            // 
            // btn_doimk
            // 
            this.btn_doimk.BackColor = System.Drawing.Color.Green;
            this.btn_doimk.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_doimk.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_doimk.Location = new System.Drawing.Point(499, 35);
            this.btn_doimk.Margin = new System.Windows.Forms.Padding(2);
            this.btn_doimk.Name = "btn_doimk";
            this.btn_doimk.Size = new System.Drawing.Size(66, 88);
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
            this.btn_thoat.Location = new System.Drawing.Point(499, 127);
            this.btn_thoat.Margin = new System.Windows.Forms.Padding(2);
            this.btn_thoat.Name = "btn_thoat";
            this.btn_thoat.Size = new System.Drawing.Size(66, 72);
            this.btn_thoat.TabIndex = 8;
            this.btn_thoat.Text = "Thoát";
            this.btn_thoat.UseVisualStyleBackColor = false;
            this.btn_thoat.Click += new System.EventHandler(this.btn_thoat_Click);
            // 
            // ptb_Doi_MK
            // 
            this.ptb_Doi_MK.Image = global::Pixel_Drift.Properties.Resources.Doi_MK;
            this.ptb_Doi_MK.Location = new System.Drawing.Point(0, 0);
            this.ptb_Doi_MK.Name = "ptb_Doi_MK";
            this.ptb_Doi_MK.Size = new System.Drawing.Size(752, 446);
            this.ptb_Doi_MK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_Doi_MK.TabIndex = 9;
            this.ptb_Doi_MK.TabStop = false;
            // 
            // Form_Doi_Mat_Khau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 450);
            this.Controls.Add(this.btn_thoat);
            this.Controls.Add(this.btn_doimk);
            this.Controls.Add(this.txt_xacnhanmk);
            this.Controls.Add(this.txt_mkmoi);
            this.Controls.Add(this.txt_mkcu);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lb_mkmoi);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ptb_Doi_MK);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form_Doi_Mat_Khau";
            this.Text = "Form_DoiMatKhau";
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Doi_MK)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_mkmoi;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_mkcu;
        private System.Windows.Forms.TextBox txt_mkmoi;
        private System.Windows.Forms.TextBox txt_xacnhanmk;
        private System.Windows.Forms.Button btn_doimk;
        private System.Windows.Forms.Button btn_thoat;
        private System.Windows.Forms.PictureBox ptb_Doi_MK;
    }
}