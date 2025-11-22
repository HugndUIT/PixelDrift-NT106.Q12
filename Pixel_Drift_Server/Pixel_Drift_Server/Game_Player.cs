using System.Net.Sockets;

namespace Pixel_Drift_Server
{
    public class Game_Player
    {
        public TcpClient Client { get; set; }
        public NetworkStream Stream { get; set; }
        public string Username { get; set; }
        public bool Is_Ready { get; set; } = false;

        public Game_Player() { }

        public Game_Player(TcpClient client, string username)
        {
            this.Client = client;
            this.Stream = client.GetStream();
            this.Username = username;
            this.Is_Ready = false;
        }
    }
}