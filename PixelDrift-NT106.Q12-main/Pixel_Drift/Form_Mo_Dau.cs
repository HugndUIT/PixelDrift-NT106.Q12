using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Form_Mo_Dau : Form
    {
        public Form_Mo_Dau()
        {
            InitializeComponent();
        }

        private void btn_dang_ki_Click(object sender, EventArgs e)
        {
            Form_Dang_Ki formdangki = new Form_Dang_Ki();
            this.Hide();
            formdangki.Show();
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btn_dang_nhap_Click(object sender, EventArgs e)
        {
            Form_Dang_Nhap formdangnhap = new Form_Dang_Nhap();
            this.Hide();
            formdangnhap.Show();
        }
    }
}
