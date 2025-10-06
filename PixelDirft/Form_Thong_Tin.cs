using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelDirft
{
    public partial class Form_Thong_Tin : Form
    {
        private void ConnectToDatabase()
        {
            string connectionString = "Data Source=DESKTOP-VOALG2L;Initial Catalog=QlyNguoiDung;Integrated Security=True;Trust Server Certificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                int id = 1;

                SqlCommand command = new SqlCommand($"SELECT * FROM Users Where Id = {id}", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    lbl_Indentify.Text += reader["Id"].ToString();
                    lbl_TenDangNhap.Text += reader["Username"].ToString();
                    lbl_Email.Text += reader["Email"].ToString();

                }
                reader.Close();
            }
        }

        public Form_Thong_Tin()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void frmThongTin_Load(object sender, EventArgs e)
        {
            ConnectToDatabase();

        }
    }
}

