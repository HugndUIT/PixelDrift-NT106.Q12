namespace PixelDirft_Server
{
    partial class frm_Server
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
            this.btn_Start_Server = new System.Windows.Forms.Button();
            this.btn_End_Server = new System.Windows.Forms.Button();
            this.tb_hienthi = new System.Windows.Forms.TextBox();
            this.lb_danhsachclient = new System.Windows.Forms.ListBox();
            this.tb_nhaptinnhan = new System.Windows.Forms.TextBox();
            this.btn_Send_All = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_Start_Server
            // 
            this.btn_Start_Server.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btn_Start_Server.Location = new System.Drawing.Point(14, 15);
            this.btn_Start_Server.Name = "btn_Start_Server";
            this.btn_Start_Server.Size = new System.Drawing.Size(462, 74);
            this.btn_Start_Server.TabIndex = 0;
            this.btn_Start_Server.Text = "Start Server";
            this.btn_Start_Server.UseVisualStyleBackColor = true;
            this.btn_Start_Server.Click += new System.EventHandler(this.btn_Start_Server_Click);
            // 
            // btn_End_Server
            // 
            this.btn_End_Server.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btn_End_Server.Location = new System.Drawing.Point(474, 15);
            this.btn_End_Server.Name = "btn_End_Server";
            this.btn_End_Server.Size = new System.Drawing.Size(478, 74);
            this.btn_End_Server.TabIndex = 1;
            this.btn_End_Server.Text = "End Server";
            this.btn_End_Server.UseVisualStyleBackColor = true;
            this.btn_End_Server.Click += new System.EventHandler(this.btn_End_Server_Click);
            // 
            // tb_hienthi
            // 
            this.tb_hienthi.Location = new System.Drawing.Point(14, 97);
            this.tb_hienthi.Multiline = true;
            this.tb_hienthi.Name = "tb_hienthi";
            this.tb_hienthi.Size = new System.Drawing.Size(937, 273);
            this.tb_hienthi.TabIndex = 3;
            // 
            // lb_danhsachclient
            // 
            this.lb_danhsachclient.FormattingEnabled = true;
            this.lb_danhsachclient.ItemHeight = 20;
            this.lb_danhsachclient.Location = new System.Drawing.Point(14, 378);
            this.lb_danhsachclient.Name = "lb_danhsachclient";
            this.lb_danhsachclient.Size = new System.Drawing.Size(937, 84);
            this.lb_danhsachclient.TabIndex = 5;
            // 
            // tb_nhaptinnhan
            // 
            this.tb_nhaptinnhan.Location = new System.Drawing.Point(14, 471);
            this.tb_nhaptinnhan.Multiline = true;
            this.tb_nhaptinnhan.Name = "tb_nhaptinnhan";
            this.tb_nhaptinnhan.Size = new System.Drawing.Size(622, 75);
            this.tb_nhaptinnhan.TabIndex = 6;
            // 
            // btn_Send_All
            // 
            this.btn_Send_All.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btn_Send_All.Location = new System.Drawing.Point(644, 474);
            this.btn_Send_All.Name = "btn_Send_All";
            this.btn_Send_All.Size = new System.Drawing.Size(309, 74);
            this.btn_Send_All.TabIndex = 7;
            this.btn_Send_All.Text = "Send All";
            this.btn_Send_All.UseVisualStyleBackColor = true;

            // 
            // frm_Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 563);
            this.Controls.Add(this.btn_Send_All);
            this.Controls.Add(this.tb_nhaptinnhan);
            this.Controls.Add(this.lb_danhsachclient);
            this.Controls.Add(this.tb_hienthi);
            this.Controls.Add(this.btn_End_Server);
            this.Controls.Add(this.btn_Start_Server);
            this.Name = "frm_Server";
            this.Text = "Server Form";
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Start_Server;
        private System.Windows.Forms.Button btn_End_Server;
        private System.Windows.Forms.TextBox tb_hienthi;
        private System.Windows.Forms.ListBox lb_danhsachclient;
        private System.Windows.Forms.TextBox tb_nhaptinnhan;
        private System.Windows.Forms.Button btn_Send_All;
    }
}

