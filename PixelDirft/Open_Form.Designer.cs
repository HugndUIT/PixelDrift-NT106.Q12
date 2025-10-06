namespace PixelDirft
{
    partial class Open_Form
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
            btn_dang_ki = new Button();
            btn_thoat = new Button();
            btn_dang_nhap = new Button();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // btn_dang_ki
            // 
            btn_dang_ki.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            btn_dang_ki.Location = new Point(101, 161);
            btn_dang_ki.Name = "btn_dang_ki";
            btn_dang_ki.Size = new Size(169, 98);
            btn_dang_ki.TabIndex = 0;
            btn_dang_ki.Text = "Đăng Kí";
            btn_dang_ki.UseVisualStyleBackColor = true;
            btn_dang_ki.Click += btn_dang_ki_Click;
            // 
            // btn_thoat
            // 
            btn_thoat.BackColor = Color.Firebrick;
            btn_thoat.Font = new Font("Segoe UI", 12F);
            btn_thoat.ForeColor = Color.White;
            btn_thoat.Location = new Point(241, 336);
            btn_thoat.Name = "btn_thoat";
            btn_thoat.Size = new Size(111, 49);
            btn_thoat.TabIndex = 2;
            btn_thoat.Text = "Thoát Game";
            btn_thoat.UseVisualStyleBackColor = false;
            btn_thoat.Click += btn_thoat_Click;
            // 
            // btn_dang_nhap
            // 
            btn_dang_nhap.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            btn_dang_nhap.Location = new Point(322, 161);
            btn_dang_nhap.Name = "btn_dang_nhap";
            btn_dang_nhap.Size = new Size(169, 98);
            btn_dang_nhap.TabIndex = 3;
            btn_dang_nhap.Text = "Đăng Nhập";
            btn_dang_nhap.UseVisualStyleBackColor = true;
            btn_dang_nhap.Click += btn_dang_nhap_Click;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 44F);
            label1.ForeColor = Color.DarkGoldenrod;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(592, 135);
            label1.TabIndex = 4;
            label1.Text = "PIXEL DRIFT";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            label2.Location = new Point(22, 281);
            label2.Name = "label2";
            label2.Size = new Size(558, 25);
            label2.TabIndex = 5;
            label2.Text = "Lưu ý !!!! Nếu chưa có tài khoản xin đăng kí tài khoản trước tiên";
            label2.Click += label2_Click;
            // 
            // Open_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 255, 192);
            ClientSize = new Size(592, 420);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btn_dang_nhap);
            Controls.Add(btn_thoat);
            Controls.Add(btn_dang_ki);
            ForeColor = Color.DarkGoldenrod;
            Name = "Open_Form";
            Text = "Pixel Drift";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btn_dang_ki;
        private Button btn_thoat;
        private Button btn_dang_nhap;
        private Label label1;
        private Label label2;
    }
}