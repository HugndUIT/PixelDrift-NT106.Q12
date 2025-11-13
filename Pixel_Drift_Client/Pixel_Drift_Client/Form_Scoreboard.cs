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

namespace Pixel_Drift
{
    public partial class Form_ScoreBoard : Form
    {
        public Form_ScoreBoard()
        {
            InitializeComponent();
            LoadScoreBoard();
        }

        private void LoadScoreBoard()
        {
            try
            {
                // Dữ liệu mẫu để test
                DataTable mockData = new DataTable();
                mockData.Columns.Add("Tên người chơi");
                mockData.Columns.Add("Số trận thắng", typeof(int));
                mockData.Columns.Add("Số lần va chạm", typeof(int));
                mockData.Columns.Add("Tổng điểm", typeof(double)); // quãng đường xe chạy sau 60 giây

                mockData.Rows.Add("Nguyễn Văn A", 5, 2, 1250.4);
                mockData.Rows.Add("Trần Thị B", 3, 4, 980.2);
                mockData.Rows.Add("Lê C", 1, 6, 730.9);
                mockData.Rows.Add("Phạm D", 4, 3, 1150.7);
                mockData.Rows.Add("Hoàng E", 2, 5, 850.6);

                // Sắp xếp theo Tổng điểm giảm dần
                var sortedRows = mockData.AsEnumerable()
                    .OrderByDescending(r => r.Field<double>("Tổng điểm"))
                    .ToList();

                // Tạo bảng xếp hạng có thêm STT
                DataTable rankedData = new DataTable();
                rankedData.Columns.Add("STT", typeof(int));
                rankedData.Columns.Add("Tên người chơi");
                rankedData.Columns.Add("Số trận thắng", typeof(int));
                rankedData.Columns.Add("Số lần va chạm", typeof(int));
                rankedData.Columns.Add("Tổng điểm", typeof(double));

                int stt = 1;
                foreach (DataRow row in sortedRows)
                {
                    rankedData.Rows.Add(stt++, row["Tên người chơi"], row["Số trận thắng"], row["Số lần va chạm"], row["Tổng điểm"]);
                }

                // Gán dữ liệu cho DataGridView
                dgv_ScoreBoard.DataSource = rankedData;

                // Style cho bảng
                dgv_ScoreBoard.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv_ScoreBoard.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv_ScoreBoard.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv_ScoreBoard.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                dgv_ScoreBoard.DefaultCellStyle.Font = new Font("Segoe UI", 11);
                dgv_ScoreBoard.BackgroundColor = Color.WhiteSmoke;
                dgv_ScoreBoard.BorderStyle = BorderStyle.None;
                dgv_ScoreBoard.GridColor = Color.LightGray;
                dgv_ScoreBoard.RowHeadersVisible = false;
                dgv_ScoreBoard.AllowUserToAddRows = false;

                // Khi bảng đã load xong tô màu cho top 1
                dgv_ScoreBoard.DataBindingComplete += (s, e) =>
                {
                    if (dgv_ScoreBoard.Rows.Count > 0)
                    {
                        var topRow = dgv_ScoreBoard.Rows[0];
                        topRow.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                        topRow.DefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải bảng xếp hạng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}