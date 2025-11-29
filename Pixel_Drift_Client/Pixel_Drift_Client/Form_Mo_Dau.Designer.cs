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
            this.btn_dang_ki = new Guna.UI2.WinForms.Guna2Button();
            this.btn_dang_nhap = new Guna.UI2.WinForms.Guna2Button();
            this.btn_thoat = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // btn_dang_ki
            // 
            this.btn_dang_ki.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_dang_ki.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btn_dang_ki.BorderRadius = 15;
            this.btn_dang_ki.BorderThickness = 3;
            this.btn_dang_ki.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_dang_ki.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_dang_ki.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_dang_ki.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_dang_ki.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_dang_ki.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.btn_dang_ki.ForeColor = System.Drawing.Color.Black;
            this.btn_dang_ki.HoverState.BorderColor = System.Drawing.Color.White;
            this.btn_dang_ki.HoverState.FillColor = System.Drawing.Color.Lime;
            this.btn_dang_ki.Location = new System.Drawing.Point(294, 348);
            this.btn_dang_ki.Margin = new System.Windows.Forms.Padding(4);
            this.btn_dang_ki.Name = "btn_dang_ki";
            this.btn_dang_ki.Size = new System.Drawing.Size(344, 73);
            this.btn_dang_ki.TabIndex = 0;
            this.btn_dang_ki.Text = "Đăng Kí";
            this.btn_dang_ki.Click += new System.EventHandler(this.btn_dang_ki_Click);
            // 
            // btn_dang_nhap
            // 
            this.btn_dang_nhap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_dang_nhap.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_dang_nhap.BorderRadius = 15;
            this.btn_dang_nhap.BorderThickness = 3;
            this.btn_dang_nhap.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_dang_nhap.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_dang_nhap.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_dang_nhap.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_dang_nhap.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btn_dang_nhap.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.btn_dang_nhap.ForeColor = System.Drawing.Color.Black;
            this.btn_dang_nhap.HoverState.BorderColor = System.Drawing.Color.White;
            this.btn_dang_nhap.HoverState.FillColor = System.Drawing.Color.Aqua;
            this.btn_dang_nhap.Location = new System.Drawing.Point(294, 418);
            this.btn_dang_nhap.Margin = new System.Windows.Forms.Padding(4);
            this.btn_dang_nhap.Name = "btn_dang_nhap";
            this.btn_dang_nhap.Size = new System.Drawing.Size(344, 73);
            this.btn_dang_nhap.TabIndex = 3;
            this.btn_dang_nhap.Text = "Đăng Nhập";
            this.btn_dang_nhap.Click += new System.EventHandler(this.btn_dang_nhap_Click);
            // 
            // btn_thoat
            // 
            this.btn_thoat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_thoat.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btn_thoat.BorderRadius = 15;
            this.btn_thoat.BorderThickness = 3;
            this.btn_thoat.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_thoat.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_thoat.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_thoat.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_thoat.FillColor = System.Drawing.Color.Red;
            this.btn_thoat.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_thoat.ForeColor = System.Drawing.Color.White;
            this.btn_thoat.HoverState.BorderColor = System.Drawing.Color.White;
            this.btn_thoat.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btn_thoat.Location = new System.Drawing.Point(294, 485);
            this.btn_thoat.Margin = new System.Windows.Forms.Padding(4);
            this.btn_thoat.Name = "btn_thoat";
            this.btn_thoat.Size = new System.Drawing.Size(344, 73);
            this.btn_thoat.TabIndex = 2;
            this.btn_thoat.Text = "Thoát Game";
            this.btn_thoat.Click += new System.EventHandler(this.btn_thoat_Click);
            // 
            // Form_Mo_Dau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Pixel_Drift.Properties.Resources.formm_odau;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(917, 571);
            this.Controls.Add(this.btn_dang_nhap);
            this.Controls.Add(this.btn_thoat);
            this.Controls.Add(this.btn_dang_ki);
            this.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form_Mo_Dau";
            this.Text = "Pixel Drift";
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Button btn_dang_ki;
        private Guna.UI2.WinForms.Guna2Button btn_dang_nhap;
        private Guna.UI2.WinForms.Guna2Button btn_thoat;
    }
}

