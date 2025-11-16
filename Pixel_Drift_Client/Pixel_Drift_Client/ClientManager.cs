using System;
using System.Net.Sockets;
using System.Text;
using System.IO; 
using System.Text.Json;

namespace Pixel_Drift 
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

                if (client != null)
                    CloseConnection();

                client = new TcpClient();
                client.Connect(ip, port); 

                stream = client.GetStream();

                writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                reader = new StreamReader(stream, Encoding.UTF8);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

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


                writer.WriteLine(jsonRequest);

                string response = reader.ReadLine();

                if (response == null)
                {
                    // Server ngắt kết nối
                    throw new IOException("Server đã ngắt kết nối.");
                }

                return response;
            }
            catch (IOException) 
            {
                CloseConnection(); 
                throw new SocketException(); 
            }
        }

        public static void SendMessage(object data)
        {
            if (IsConnected)
            {
                try
                {
                    string json = JsonSerializer.Serialize(data);
                    writer.WriteLine(json); 
                }
                catch (IOException)
                {
                   
                    CloseConnection();
                }
            }
        }

        public static StreamReader GetReader()
        {
            return reader;
        }

        public static bool IsConnected
        {
            get { return client != null && client.Connected && writer != null && reader != null; }
        }

        public static void CloseConnection()
        {
            
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