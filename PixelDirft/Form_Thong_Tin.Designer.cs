namespace PixelDirft
{
    partial class Form_Thong_Tin
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
            btnThoat = new Button();
            lblTieuDe = new Label();
            lbl_Indentify = new Label();
            lbl_TenDangNhap = new Label();
            lbl_Email = new Label();
            SuspendLayout();
            // 
            // btnThoat
            // 
            btnThoat.BackColor = Color.Firebrick;
            btnThoat.Font = new Font("Times New Roman", 14F, FontStyle.Italic, GraphicsUnit.Point, 0);
            btnThoat.ForeColor = SystemColors.ButtonFace;
            btnThoat.Location = new Point(219, 471);
            btnThoat.Margin = new Padding(4);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(168, 45);
            btnThoat.TabIndex = 0;
            btnThoat.Text = "Thoát";
            btnThoat.UseVisualStyleBackColor = false;
            btnThoat.Click += btnThoat_Click;
            // 
            // lblTieuDe
            // 
            lblTieuDe.Dock = DockStyle.Top;
            lblTieuDe.Font = new Font("Times New Roman", 18F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTieuDe.Location = new Point(0, 0);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(606, 119);
            lblTieuDe.TabIndex = 1;
            lblTieuDe.Text = "Thông tin người dùng";
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbl_Indentify
            // 
            lbl_Indentify.Font = new Font("Times New Roman", 14F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lbl_Indentify.Location = new Point(34, 148);
            lbl_Indentify.Name = "lbl_Indentify";
            lbl_Indentify.Size = new Size(379, 32);
            lbl_Indentify.TabIndex = 2;
            lbl_Indentify.Text = "ID: ";
            // 
            // lbl_TenDangNhap
            // 
            lbl_TenDangNhap.Font = new Font("Times New Roman", 14F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lbl_TenDangNhap.Location = new Point(34, 235);
            lbl_TenDangNhap.Name = "lbl_TenDangNhap";
            lbl_TenDangNhap.Size = new Size(379, 32);
            lbl_TenDangNhap.TabIndex = 3;
            lbl_TenDangNhap.Text = "Username: ";
            // 
            // lbl_Email
            // 
            lbl_Email.Font = new Font("Times New Roman", 14F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lbl_Email.Location = new Point(34, 332);
            lbl_Email.Name = "lbl_Email";
            lbl_Email.Size = new Size(435, 32);
            lbl_Email.TabIndex = 4;
            lbl_Email.Text = "Email/Sđt: ";
            // 
            // frmThongTin
            // 
            AutoScaleDimensions = new SizeF(15F, 33F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(255, 255, 192);
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(606, 550);
            Controls.Add(lbl_Email);
            Controls.Add(lbl_TenDangNhap);
            Controls.Add(lbl_Indentify);
            Controls.Add(lblTieuDe);
            Controls.Add(btnThoat);
            Font = new Font("Times New Roman", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.DarkGoldenrod;
            Margin = new Padding(4);
            Name = "frmThongTin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thông tin ";
            Load += frmThongTin_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button btnThoat;
        private Label lblTieuDe;
        private Label lbl_Indentify;
        private Label lbl_TenDangNhap;
        private Label lbl_Email;
    }
}
