namespace Pixel_Drift
{
    partial class Form_ScoreBoard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.dgv_ScoreBoard = new System.Windows.Forms.DataGridView();
            this.btn_Close = new System.Windows.Forms.Button();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.btn_Search = new System.Windows.Forms.Button();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ScoreBoard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(252, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(420, 44);
            this.label1.TabIndex = 0;
            this.label1.Text = "🏆 SCORE BOARD 🏆";
            // 
            // dgv_ScoreBoard
            // 
            this.dgv_ScoreBoard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ScoreBoard.Location = new System.Drawing.Point(222, 147);
            this.dgv_ScoreBoard.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgv_ScoreBoard.Name = "dgv_ScoreBoard";
            this.dgv_ScoreBoard.RowHeadersWidth = 51;
            this.dgv_ScoreBoard.RowTemplate.Height = 35;
            this.dgv_ScoreBoard.Size = new System.Drawing.Size(483, 291);
            this.dgv_ScoreBoard.TabIndex = 1;
            // 
            // btn_Close
            // 
            this.btn_Close.BackColor = System.Drawing.Color.Crimson;
            this.btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Close.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btn_Close.ForeColor = System.Drawing.Color.White;
            this.btn_Close.Location = new System.Drawing.Point(222, 472);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(483, 62);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "Đóng";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // txt_Search
            // 
            this.txt_Search.Font = new System.Drawing.Font("Microsoft Sans Serif", 23F);
            this.txt_Search.Location = new System.Drawing.Point(239, 72);
            this.txt_Search.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(238, 42);
            this.txt_Search.TabIndex = 3;
            this.txt_Search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Search_KeyPress);
            // 
            // btn_Search
            // 
            this.btn_Search.BackColor = System.Drawing.Color.DodgerBlue;
            this.btn_Search.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Search.ForeColor = System.Drawing.Color.White;
            this.btn_Search.Location = new System.Drawing.Point(595, 72);
            this.btn_Search.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(110, 43);
            this.btn_Search.TabIndex = 4;
            this.btn_Search.Text = "🔍 Tìm kiếm";
            this.btn_Search.UseVisualStyleBackColor = false;
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.BackColor = System.Drawing.Color.ForestGreen;
            this.btn_Refresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Refresh.ForeColor = System.Drawing.Color.White;
            this.btn_Refresh.Location = new System.Drawing.Point(481, 72);
            this.btn_Refresh.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(110, 43);
            this.btn_Refresh.TabIndex = 5;
            this.btn_Refresh.Text = "🔄 Làm mới";
            this.btn_Refresh.UseVisualStyleBackColor = false;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 23F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(60, 72);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 35);
            this.label2.TabIndex = 6;
            this.label2.Text = "Tìm kiếm:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Pixel_Drift.Properties.Resources.ScoreBoard;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(927, 575);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // Form_ScoreBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.btn_Close;
            this.ClientSize = new System.Drawing.Size(927, 576);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_Refresh);
            this.Controls.Add(this.btn_Search);
            this.Controls.Add(this.txt_Search);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.dgv_ScoreBoard);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "Form_ScoreBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bảng Xếp Hạng - Pixel Drift";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ScoreBoard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgv_ScoreBoard;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}