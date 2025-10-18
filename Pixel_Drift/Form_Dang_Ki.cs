using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Supabase;

namespace Pixel_Drift
{
    public partial class Form_Dang_Ki : Form
    {
        private const string Supabase_URL = "https://rppuqqzvoarjmoefezyj.supabase.co";
        private const string Supabase_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJwcHVxcXp2b2Fyam1vZWZlenlqIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjA3OTEzNTIsImV4cCI6MjA3NjM2NzM1Mn0.IjiZuVa99-g5PxolFVXJ7hb76QWcNzLuhPJLYxnV_FM";
        private Supabase.Client _supabase;
        public Form_Dang_Ki()
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

        // Ham kiem tra do manh yeu cua mat khau
        bool KiemTraDoManhMatKhau(string password)
        {
            if (password.Length < 8) return false; // tối thiểu 8 ký tự
            bool coChuHoa = Regex.IsMatch(password, "[A-Z]");
            bool coChuThuong = Regex.IsMatch(password, "[a-z]");
            bool coSo = Regex.IsMatch(password, "[0-9]");
            bool coKyTuDacBiet = Regex.IsMatch(password, @"[@$!%*?&#]");

            return coChuHoa && coChuThuong && coSo && coKyTuDacBiet;
        }
        private async void btn_xacnhan_Click(object sender, EventArgs e)
        {
            // Lay thong tin nguoi dung
            string username = tb_tendangnhap.Text.Trim();
            string password = tb_matkhau.Text.Trim();
            string confirmpass = tb_xacnhanmk.Text.Trim();
            string emailsdt = tb_emailsdt.Text.Trim();
            // Kiem tra du lieu trong
            if (username == "" || password == "" || emailsdt == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Kiem tra dinh dang email
            bool isEmail = Regex.IsMatch(emailsdt, @"^[a-zA-Z0-9._%+-]+@gmail\.com$");  // định dạng Gmail
            bool isPhone = Regex.IsMatch(emailsdt, @"^(0[0-9]{9})$"); // số điện thoại VN: bắt đầu bằng 0, 10 số
            if (!isEmail && !isPhone)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng Gmail hoặc số điện thoại (10 số, bắt đầu bằng 0)!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Kiem tra do manh yeu cua mat khau
            if (!KiemTraDoManhMatKhau(password))
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Kiem tra xac nhan mat khau
            if (password != confirmpass)
            {
                MessageBox.Show("Mật khẩu không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Ma hoa mat khau
            string hashedPassword = MaHoa(password);
            // Ket noi voi co so du lieu
            try
            {

                // Kiem tra email da ton tai chua?
                var CheckResponse = await _supabase.From<TaiKhoanNguoiDung>().Where(u => u.Email == emailsdt).Limit(1).Get();

                if (CheckResponse.Models.Count > 0)
                {
                    MessageBox.Show("Email hoặc số điện thoại đã tồn tại, vui lòng sử dụng email hoặc số điện thoại khác!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Them email, so dien thoai moi
                var NewUser = new TaiKhoanNguoiDung()
                {
                    Username = username,
                    Password = hashedPassword,
                    Email = emailsdt
                };

                var InsertRespose = await _supabase.From<TaiKhoanNguoiDung>().Insert(NewUser);

                if (InsertRespose.Models.Count > 0)
                {
                    MessageBox.Show("Đăng ký thành công!");
                    Form_Dang_Nhap dangnhap = new Form_Dang_Nhap();
                    dangnhap.ShowDialog();
                    this.Close();
                }
                else
                    MessageBox.Show("Đăng ký thất bại!");


            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}


