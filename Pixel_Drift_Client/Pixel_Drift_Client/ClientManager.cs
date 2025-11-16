using System;
using System.Net.Sockets;
using System.Text;
using System.IO; // <-- Cần cho Reader/Writer
using System.Text.Json;

namespace Pixel_Drift // <-- Đảm bảo namespace này khớp với dự án của bạn
{
    public static class ClientManager
    {
        private static TcpClient client;
        private static StreamWriter writer;
        private static StreamReader reader;
        private static NetworkStream stream;

        /// <summary>
        /// Kết nối đến server và giữ kết nối.
        /// </summary>
        public static bool Connect(string ip, int port)
        {
            try
            {
                // Nếu đang kết nối, đóng nó đi để kết nối lại
                if (client != null)
                    CloseConnection();

                client = new TcpClient();
                client.Connect(ip, port); // Kết nối

                stream = client.GetStream();

                // === SỬA LỖI CỐT LÕI ===
                // 1. Bọc stream bằng Writer và Reader để xử lý JSON Lines
                writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                reader = new StreamReader(stream, Encoding.UTF8);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gửi một yêu cầu (Request) VÀ chờ nhận lại một phản hồi (Response).
        /// Dùng cho: Đăng nhập, Đăng ký, Lấy thông tin...
        /// </summary>
        public static string SendRequest(object requestData)
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("Chưa kết nối đến server.");
            }

            try
            {
                // 1. Serialize đối tượng thành JSON
                string jsonRequest = JsonSerializer.Serialize(requestData);

                // 2. Gửi JSON + ký tự '\n'
                // Server (dùng ReadLine) đang chờ chính xác ký tự '\n' này
                writer.WriteLine(jsonRequest);

                // 3. Đọc phản hồi (đọc cho đến khi gặp '\n')
                // Server (dùng SendMessage) cũng gửi kèm '\n'
                string response = reader.ReadLine();

                if (response == null)
                {
                    // Server ngắt kết nối
                    throw new IOException("Server đã ngắt kết nối.");
                }

                return response;
            }
            catch (IOException) // Lỗi I/O (thường là mất kết nối)
            {
                CloseConnection(); // Dọn dẹp
                throw new SocketException(); // Ném lỗi để Form (ví dụ: Form_Dang_Nhap) bắt được
            }
        }

        /// <summary>
        /// Chỉ gửi một tin nhắn đi (Fire-and-Forget).
        /// Dùng cho: Gửi input (nhấn phím), "đã sẵn sàng"...
        /// </summary>
        public static void SendMessage(object data)
        {
            if (IsConnected)
            {
                try
                {
                    string json = JsonSerializer.Serialize(data);
                    writer.WriteLine(json); // Chỉ gửi, không chờ phản hồi
                }
                catch (IOException)
                {
                    // Lỗi (mất kết nối) thì dọn dẹp
                    CloseConnection();
                }
            }
        }

        /// <summary>
        /// Lấy Reader để một luồng khác (ví dụ: luồng game) có thể lắng nghe.
        /// </summary>
        public static StreamReader GetReader()
        {
            return reader;
        }

        /// <summary>
        /// Kiểm tra xem còn kết nối không
        /// </summary>
        public static bool IsConnected
        {
            get { return client != null && client.Connected && writer != null && reader != null; }
        }

        /// <summary>
        /// Đóng tất cả kết nối (khi thoát game, v.v.)
        /// </summary>
        public static void CloseConnection()
        {
            // Đóng theo thứ tự (Wrapper -> Stream -> Client)
            writer?.Close();
            reader?.Close();
            stream?.Close();
            client?.Close();

            // Reset
            writer = null;
            reader = null;
            stream = null;
            client = null;
        }
    }
}