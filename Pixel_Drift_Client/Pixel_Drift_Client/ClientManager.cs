using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Pixel_Drift
{
    class ClientManager
    {
        private static TcpClient client;
        private static NetworkStream stream;
        private static byte[] buffer = new byte[4096];

        public static bool Connect(string serverIP, int serverPort)
        {
            try
            {
                // Nếu đang kết nối, đóng nó đi để kết nối lại
                if (client != null && client.Connected)
                {
                    client.Close();
                }

                client = new TcpClient();
                client.Connect(serverIP, serverPort);
                stream = client.GetStream();
                return true;
            }
            catch (SocketException)
            {
                // Không thể kết nối
                return false;
            }
        }

        public static string SendRequest(object data)
        {
            if (client == null || !client.Connected || stream == null)
            {
                throw new Exception("Chưa kết nối tới server!");
            }

            try
            {
                // 1. Gửi
                string json = JsonSerializer.Serialize(data);
                byte[] sendBytes = Encoding.UTF8.GetBytes(json);
                stream.Write(sendBytes, 0, sendBytes.Length);

                // 2. Nhận
                int len = stream.Read(buffer, 0, buffer.Length);
                if (len == 0)
                {
                    // Server đã ngắt kết nối
                    throw new Exception("Server đã ngắt kết nối.");
                }
                string response = Encoding.UTF8.GetString(buffer, 0, len);
                return response;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ví dụ: mất kết nối)
                Console.WriteLine("Lỗi SendRequest: " + ex.Message);
                return JsonSerializer.Serialize(new { status = "error", message = "Mất kết nối tới server." });
            }
        }

        // Hàm đóng kết nối khi thoát game
        public static void CloseConnection()
        {
            if (client != null && client.Connected)
            {
                stream?.Close();
                client.Close();
            }
        }
    }
}
