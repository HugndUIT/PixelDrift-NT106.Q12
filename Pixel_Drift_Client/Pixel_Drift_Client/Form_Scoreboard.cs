using System;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Text.Json;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Pixel_Drift
{
    public partial class Form_ScoreBoard : Form
    {
        public Form_ScoreBoard(TcpClient tcpClient)
        {
            InitializeComponent();

            ClientManager.OnMessageReceived += HandleServerMessage;

            LoadScoreBoard();
        }

        private void HandleServerMessage(string message)
        {
            if (this.Disposing || this.IsDisposed || !this.IsHandleCreated) return;

            this.Invoke(new Action(() =>
            {
                try
                {
                    var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(message);
                    if (!data.ContainsKey("action")) return;

                    string action = data["action"].GetString();

                    if (action == "scoreboard_data" || action == "search_result")
                    {
                        if (data.ContainsKey("data"))
                        {
                            string scoreData = data["data"].GetString();
                            DisplayScoreBoard(scoreData);
                        }
                    }
                }
                catch { }
            }));
        }

        private void LoadScoreBoard()
        {
            var request = new { action = "get_scoreboard", top_count = 50 };
            ClientManager.Send_And_Forget(request);
        }

        public void DisplayScoreBoard(string jsonData)
        {
            try
            {
                if (string.IsNullOrEmpty(jsonData) || jsonData == "EMPTY" || jsonData == "ERROR")
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

                        dt.Rows.Add(rank++, playerName, winCount, crashCount, totalScore, datePlayed);
                    }
                }

                dgv_ScoreBoard.DataSource = dt;
                StyleDataGridView();
                HighlightTopPlayers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị bảng điểm: " + ex.Message);
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
            var request = new { action = "search_player", search_text = searchText };
            ClientManager.Send_And_Forget(request);
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            LoadScoreBoard();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btn_Search_Click(sender, e);
                e.Handled = true;
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
                dgv_ScoreBoard.Rows[0].DefaultCellStyle.BackColor = Color.Gold;
                dgv_ScoreBoard.Rows[0].DefaultCellStyle.ForeColor = Color.Black;
                dgv_ScoreBoard.Rows[0].DefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);

                if (dgv_ScoreBoard.Rows.Count > 1)
                {
                    dgv_ScoreBoard.Rows[1].DefaultCellStyle.BackColor = Color.Silver;
                    dgv_ScoreBoard.Rows[1].DefaultCellStyle.ForeColor = Color.Black;
                    dgv_ScoreBoard.Rows[1].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }

                if (dgv_ScoreBoard.Rows.Count > 2)
                {
                    dgv_ScoreBoard.Rows[2].DefaultCellStyle.BackColor = Color.SandyBrown;
                    dgv_ScoreBoard.Rows[2].DefaultCellStyle.ForeColor = Color.Black;
                    dgv_ScoreBoard.Rows[2].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ClientManager.OnMessageReceived -= HandleServerMessage;
            base.OnFormClosing(e);
        }
    }
}