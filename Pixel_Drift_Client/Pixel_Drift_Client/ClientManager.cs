using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pixel_Drift
{
    public static class ClientManager
    {
        private static TcpClient client;
        private static StreamWriter writer;
        private static StreamReader reader;
        private static NetworkStream stream;

        public static event Action<string> OnMessageReceived;
        private static bool isGlobalListening = false;

        public static TcpClient GetClient() => client;

        public static string Get_Server_IP()
        {
            string serverIP = null;
            using (UdpClient udpClient = new UdpClient())
            {
                udpClient.EnableBroadcast = true;
                var endpoint = new IPEndPoint(IPAddress.Broadcast, 2222);
                byte[] bytes = Encoding.UTF8.GetBytes("discover_server");
                try
                {
                    udpClient.Send(bytes, bytes.Length, endpoint);
                    var asyncResult = udpClient.BeginReceive(null, null);
                    if (asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2)))
                    {
                        IPEndPoint serverEp = new IPEndPoint(IPAddress.Any, 0);
                        byte[] receivedBytes = udpClient.EndReceive(asyncResult, ref serverEp);
                        if (Encoding.UTF8.GetString(receivedBytes) == "server_here")
                            serverIP = serverEp.Address.ToString();
                    }
                }
                catch { }
            }
            return serverIP;
        }

        public static bool Connect(string ip, int port)
        {
            try
            {
                if (client != null) CloseConnection();
                client = new TcpClient();

                string finalIP = string.IsNullOrEmpty(ip) ? Get_Server_IP() : ip;
                if (string.IsNullOrEmpty(finalIP)) finalIP = "127.0.0.1";

                client.Connect(finalIP, port);
                stream = client.GetStream();
                writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                reader = new StreamReader(stream, Encoding.UTF8);

                return true;
            }
            catch { return false; }
        }

        public static void StartGlobalListening()
        {
            if (isGlobalListening) return;
            isGlobalListening = true;

            Task.Run(async () =>
            {
                try
                {
                    while (IsConnected)
                    {
                        string message = await reader.ReadLineAsync();
                        if (message != null)
                        {
                            OnMessageReceived?.Invoke(message);
                        }
                        else break;
                    }
                }
                catch { }
                finally { isGlobalListening = false; }
            });
        }

        public static void Send_And_Forget(object data)
        {
            if (IsConnected && writer != null)
            {
                try { writer.WriteLine(JsonSerializer.Serialize(data)); }
                catch { CloseConnection(); }
            }
        }

        public static string Send_And_Wait(object requestData)
        {
            if (!IsConnected) throw new Exception("Mất kết nối");
            try
            {
                writer.WriteLine(JsonSerializer.Serialize(requestData));
                return reader.ReadLine();
            }
            catch { CloseConnection(); throw; }
        }

        public static bool IsConnected => client != null && client.Connected;

        public static void CloseConnection()
        {
            isGlobalListening = false;
            writer?.Close(); reader?.Close(); stream?.Close(); client?.Close();
            client = null;
        }
    }
}