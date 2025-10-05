using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace formdangnhap
{
    public partial class FormMain : Form
    {
        private string username;
        public FormMain(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void lb_Welcome_Click(object sender, EventArgs e)
        {
            lb_Welcome.Text = "Xin chào, " + username + "!";
        }
    }
}
