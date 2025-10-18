using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Supabase;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace Pixel_Drift
{
    public partial class Form_Dang_Nhap : Form
    {
        private const string Supabase_URL = "https://rppuqqzvoarjmoefezyj.supabase.co";
        private const string Supabase_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJwcHVxcXp2b2Fyam1vZWZlenlqIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjA3OTEzNTIsImV4cCI6MjA3NjM2NzM1Mn0.IjiZuVa99-g5PxolFVXJ7hb76QWcNzLuhPJLYxnV_FM";
        private Supabase.Client _supabase;


        public Form_Dang_Nhap()
        {
            InitializeComponent();
            _supabase = new Supabase.Client(Supabase_URL, Supabase_KEY);
        }

        

        // Ham ma hoa mat khau
        string MaHoa(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2")); // Chuyen byte sang hex de luu
                return builder.ToString();
            }
        }

        private async void btn_vaogame_Click(object sender, EventArgs e)
        {

            string username = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string MaHoaMK = MaHoa(password);
                var Response = await _supabase.From<TaiKhoanNguoiDung>().Where(u => u.Email == username && u.Password == MaHoaMK).Limit(1).Get();

                if (Response.Models.Count > 0)
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form_Dang_Nhap mainForm = new Form_Dang_Nhap();
                    mainForm.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
