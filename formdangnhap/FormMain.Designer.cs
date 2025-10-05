namespace formdangnhap
{
    partial class FormMain
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
            lb_Welcome = new Label();
            SuspendLayout();
            // 
            // lb_Welcome
            // 
            lb_Welcome.AutoSize = true;
            lb_Welcome.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lb_Welcome.Location = new Point(50, 50);
            lb_Welcome.Name = "lb_Welcome";
            lb_Welcome.Size = new Size(121, 32);
            lb_Welcome.TabIndex = 0;
            lb_Welcome.Text = "Xin chào!";
            lb_Welcome.Click += lb_Welcome_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(754, 230);
            Controls.Add(lb_Welcome);
            Name = "FormMain";
            Text = "FormMain";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lb_Welcome;
    }
}