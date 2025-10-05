using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
namespace PixelDirft
{
    public partial class FormDangKi : Form
    {
        public FormDangKi()
        {
            InitializeComponent();
        }

        private void FormDangKi_Load(object sender, EventArgs e)
        {

        }

        private void tb_emailsdt_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_tendangnhap_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_matkhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_xacnhanmk_TextChanged(object sender, EventArgs e)
        {

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

        private void btn_xacnhan_Click(object sender, EventArgs e)
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

            // Kiem tra do manh yeu cua mat khau

            // Kiem tra xac nhan mat khau
            if (password != confirmpass)
            {
                MessageBox.Show("Mật khẩu không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Ma hoa mat khau

            // Ket noi voi co so du lieu
            try
            {
                using (SqlConnection Connection = Database.GetConnection())
                {
                    Connection.Open();
                    // Kiem tra email da ton tai chua?
                    string CheckQuery = "SELECT COUNT(*) FROM Users WHERE Email = @e";
                    using (SqlCommand Checkmail = new SqlCommand(CheckQuery,Connection))
                    {
                        Checkmail.Parameters.AddWithValue("@e", emailsdt);
                        int count = (int)Checkmail.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Email hoặc số điện thoại đã tồn tại, vui lòng sử dụng email hoặc số điện thoại khác!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return; 
                        }
                    }
                    // Them email, so dien thoai moi
                    string InsertQuery = "INSERT INTO Users (Username, Password, Email) VALUES (@u, @p, @e)";
                    using (SqlCommand cmd = new SqlCommand(InsertQuery, Connection))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        cmd.Parameters.AddWithValue("@p", password);
                        cmd.Parameters.AddWithValue("@e", emailsdt);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                            MessageBox.Show("Đăng ký thành công!");
                        else
                            MessageBox.Show("Đăng ký thất bại!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        public static class Database
        {
            private static string ConnStr = @"Data Source=DESKTOP-SJ246G6;Initial Catalog=QlyNguoiDung;Integrated Security=True";
            public static SqlConnection GetConnection()
            {
                return new SqlConnection(ConnStr);
            }
        }
    }
}