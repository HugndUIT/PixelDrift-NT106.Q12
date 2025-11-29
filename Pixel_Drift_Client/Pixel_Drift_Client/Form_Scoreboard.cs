using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Form_ScoreBoard : Form
    {
        private TcpClient client;
        private Timer refreshTimer;
        private const int REFRESH_INTERVAL = 5000;

        public Form_ScoreBoard(TcpClient tcpClient)
        {
            InitializeComponent();
            this.client = tcpClient;
            InitializeTimer();
            LoadScoreBoard();
        }

        private void InitializeTimer()
        {
            refreshTimer = new Timer();
            refreshTimer.Interval = REFRESH_INTERVAL;
            refreshTimer.Tick += (s, e) => LoadScoreBoard();
            refreshTimer.Start();
        }

        private void LoadScoreBoard()
        {
            try
            {
                var request = new { action = "get_scoreboard", top_count = 50 };
                SendToServer(JsonSerializer.Serialize(request));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải bảng xếp hạng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hiển thị dữ liệu lên DataGridView
        public void DisplayScoreBoard(string jsonData)
        {
            try
            {
                if (jsonData == "EMPTY" || jsonData == "ERROR")
                {
                    dgv_ScoreBoard.DataSource = null;
                    return;
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("STT", typeof(int));
                dt.Columns.Add("Tên người chơi", typeof(string));
                dt.Columns.Add("Số trận thắng", typeof(int));
                dt.Columns.Add("Số lần va chạm", typeof(int));
                dt.Columns.Add("Tổng điểm", typeof(double));
                dt.Columns.Add("Ngày chơi", typeof(string));

                string[] lines = jsonData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                int rank = 1;
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 6)
                    {
                        string playerName = parts[1];
                        int winCount = int.Parse(parts[2]);
                        int crashCount = int.Parse(parts[3]);
                        double totalScore = double.Parse(parts[4]);
                        string datePlayed = parts[5];

                        dt.Rows.Add(rank, playerName, winCount, crashCount, totalScore, datePlayed);
                        rank++;
                    }
                }

                dgv_ScoreBoard.DataSource = dt;
                StyleDataGridView();
                HighlightTopPlayers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hiển thị dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StyleDataGridView()
        {
            dgv_ScoreBoard.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv_ScoreBoard.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_ScoreBoard.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_ScoreBoard.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dgv_ScoreBoard.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dgv_ScoreBoard.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv_ScoreBoard.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv_ScoreBoard.BackgroundColor = Color.WhiteSmoke;
            dgv_ScoreBoard.BorderStyle = BorderStyle.None;
            dgv_ScoreBoard.GridColor = Color.LightGray;
            dgv_ScoreBoard.RowHeadersVisible = false;
            dgv_ScoreBoard.AllowUserToAddRows = false;
            dgv_ScoreBoard.ReadOnly = true;
            dgv_ScoreBoard.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_ScoreBoard.RowTemplate.Height = 35;

            if (dgv_ScoreBoard.Columns.Count > 0)
            {
                dgv_ScoreBoard.Columns[0].Width = 60;
                dgv_ScoreBoard.Columns[1].Width = 200;
            }
        }

        private void HighlightTopPlayers()
        {
            if (dgv_ScoreBoard.Rows.Count > 0)
            {
                // Top 1 - Vàng
                var row1 = dgv_ScoreBoard.Rows[0];
                row1.DefaultCellStyle.BackColor = Color.Gold;
                row1.DefaultCellStyle.ForeColor = Color.Black;
                row1.DefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);

                // Top 2 - Bạc
                if (dgv_ScoreBoard.Rows.Count > 1)
                {
                    var row2 = dgv_ScoreBoard.Rows[1];
                    row2.DefaultCellStyle.BackColor = Color.Silver;
                    row2.DefaultCellStyle.ForeColor = Color.Black;
                    row2.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }

                // Top 3 - Đồng
                if (dgv_ScoreBoard.Rows.Count > 2)
                {
                    var row3 = dgv_ScoreBoard.Rows[2];
                    row3.DefaultCellStyle.BackColor = Color.SandyBrown;
                    row3.DefaultCellStyle.ForeColor = Color.Black;
                    row3.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }
            }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            string searchText = txt_Search.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                LoadScoreBoard();
                return;
            }

            try
            {
                var request = new { action = "search_player", search_text = searchText };
                SendToServer(JsonSerializer.Serialize(request));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức này để hiển thị kết quả tìm kiếm
        public void DisplaySearchResults(string jsonData)
        {
            DisplayScoreBoard(jsonData); 
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            LoadScoreBoard();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            refreshTimer?.Stop();
            refreshTimer?.Dispose();
            this.Close();
        }

        // Gửi dữ liệu đến server sử dụng TcpClient
        private void SendToServer(string message)
        {
            if (client == null || !client.Connected) return;

            try
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(message + "\n");
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            refreshTimer?.Stop();
            refreshTimer?.Dispose();
            base.OnFormClosing(e);
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btn_Search_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}