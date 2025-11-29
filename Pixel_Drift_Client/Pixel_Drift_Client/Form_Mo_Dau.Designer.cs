using System.Drawing;
using System.Windows.Forms;

namespace Pixel_Drift
{
    partial class Form_Mo_Dau
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
            this.btn_dang_ki = new System.Windows.Forms.Button();
            this.btn_dang_nhap = new System.Windows.Forms.Button();
            this.btn_thoat = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_dang_ki
            // 
            this.btn_dang_ki.FlatAppearance.BorderSize = 0;
            this.btn_dang_ki.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.btn_dang_ki.ForeColor = System.Drawing.Color.ForestGreen;
            this.btn_dang_ki.Location = new System.Drawing.Point(179, 150);
            this.btn_dang_ki.Name = "btn_dang_ki";
            this.btn_dang_ki.Size = new System.Drawing.Size(245, 55);
            this.btn_dang_ki.TabIndex = 0;
            this.btn_dang_ki.Text = "Đăng Kí";
            this.btn_dang_ki.UseVisualStyleBackColor = true;
            this.btn_dang_ki.Click += new System.EventHandler(this.btn_dang_ki_Click);
            // 
            // btn_dang_nhap
            // 
            this.btn_dang_nhap.FlatAppearance.BorderSize = 0;
            this.btn_dang_nhap.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.btn_dang_nhap.ForeColor = System.Drawing.Color.MediumTurquoise;
            this.btn_dang_nhap.Location = new System.Drawing.Point(179, 211);
            this.btn_dang_nhap.Name = "btn_dang_nhap";
            this.btn_dang_nhap.Size = new System.Drawing.Size(245, 55);
            this.btn_dang_nhap.TabIndex = 3;
            this.btn_dang_nhap.Text = "Đăng Nhập";
            this.btn_dang_nhap.UseVisualStyleBackColor = true;
            this.btn_dang_nhap.Click += new System.EventHandler(this.btn_dang_nhap_Click);
            // 
            // btn_thoat
            // 
            this.btn_thoat.BackColor = System.Drawing.Color.White;
            this.btn_thoat.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_thoat.ForeColor = System.Drawing.Color.Red;
            this.btn_thoat.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_thoat.Location = new System.Drawing.Point(179, 272);
            this.btn_thoat.Name = "btn_thoat";
            this.btn_thoat.Size = new System.Drawing.Size(245, 55);
            this.btn_thoat.TabIndex = 2;
            this.btn_thoat.Text = "Thoát Game";
            this.btn_thoat.UseVisualStyleBackColor = false;
            this.btn_thoat.Click += new System.EventHandler(this.btn_thoat_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Pixel_Drift.Properties.Resources.Gemini_Generated_Image_hs0qr3hs0qr3hs0q;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(604, 334);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // Form_Mo_Dau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(605, 331);
            this.Controls.Add(this.btn_dang_nhap);
            this.Controls.Add(this.btn_thoat);
            this.Controls.Add(this.btn_dang_ki);
            this.Controls.Add(this.pictureBox1);
            this.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.Name = "Form_Mo_Dau";
            this.Text = "Pixel Drift";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_dang_ki;
        private System.Windows.Forms.Button btn_dang_nhap;
        private System.Windows.Forms.Button btn_thoat;
        private PictureBox pictureBox1;
    }
}

