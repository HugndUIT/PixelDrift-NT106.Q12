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
            this.label1 = new System.Windows.Forms.Label();
            this.btn_dang_ki = new System.Windows.Forms.Button();
            this.btn_dang_nhap = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_thoat = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 44F);
            this.label1.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(605, 117);
            this.label1.TabIndex = 4;
            this.label1.Text = "PIXEL DRIFT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_dang_ki
            // 
            this.btn_dang_ki.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.btn_dang_ki.Location = new System.Drawing.Point(144, 120);
            this.btn_dang_ki.Name = "btn_dang_ki";
            this.btn_dang_ki.Size = new System.Drawing.Size(157, 85);
            this.btn_dang_ki.TabIndex = 0;
            this.btn_dang_ki.Text = "Đăng Kí";
            this.btn_dang_ki.UseVisualStyleBackColor = true;
            // 
            // btn_dang_nhap
            // 
            this.btn_dang_nhap.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.btn_dang_nhap.Location = new System.Drawing.Point(307, 120);
            this.btn_dang_nhap.Name = "btn_dang_nhap";
            this.btn_dang_nhap.Size = new System.Drawing.Size(158, 85);
            this.btn_dang_nhap.TabIndex = 3;
            this.btn_dang_nhap.Text = "Đăng Nhập";
            this.btn_dang_nhap.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(24, 222);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(558, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Lưu ý !!!! Nếu chưa có tài khoản xin đăng kí tài khoản trước tiên";
            // 
            // btn_thoat
            // 
            this.btn_thoat.BackColor = System.Drawing.Color.Firebrick;
            this.btn_thoat.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_thoat.ForeColor = System.Drawing.Color.White;
            this.btn_thoat.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_thoat.Location = new System.Drawing.Point(216, 263);
            this.btn_thoat.Name = "btn_thoat";
            this.btn_thoat.Size = new System.Drawing.Size(150, 42);
            this.btn_thoat.TabIndex = 2;
            this.btn_thoat.Text = "Thoát Game";
            this.btn_thoat.UseVisualStyleBackColor = false;
            // 
            // Form_Mo_Dau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(605, 331);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_dang_nhap);
            this.Controls.Add(this.btn_thoat);
            this.Controls.Add(this.btn_dang_ki);
            this.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.Name = "Form_Mo_Dau";
            this.Text = "Pixel Drift";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_dang_ki;
        private System.Windows.Forms.Button btn_dang_nhap;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_thoat;
    }
}

