namespace Pixel_Drift
{
    partial class Form_ScoreBoard
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
            this.dgv_ScoreBoard = new System.Windows.Forms.DataGridView();
            this.btn_Close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ScoreBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(86, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(649, 69);
            this.label1.TabIndex = 0;
            this.label1.Text = "🏆 SCORE BOARD 🏆";
            // 
            // dgv_ScoreBoard
            // 
            this.dgv_ScoreBoard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_ScoreBoard.Location = new System.Drawing.Point(12, 81);
            this.dgv_ScoreBoard.Name = "dgv_ScoreBoard";
            this.dgv_ScoreBoard.RowHeadersWidth = 51;
            this.dgv_ScoreBoard.RowTemplate.Height = 24;
            this.dgv_ScoreBoard.Size = new System.Drawing.Size(776, 326);
            this.dgv_ScoreBoard.TabIndex = 1;
            // 
            // btn_Close
            // 
            this.btn_Close.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_Close.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Close.Location = new System.Drawing.Point(0, 413);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(800, 37);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "Đóng";
            this.btn_Close.UseVisualStyleBackColor = true;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // Form_ScoreBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Close;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.dgv_ScoreBoard);
            this.Controls.Add(this.label1);
            this.Name = "Form_ScoreBoard";
            this.Text = "Form_Scoreboard";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ScoreBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgv_ScoreBoard;
        private System.Windows.Forms.Button btn_Close;
    }
}