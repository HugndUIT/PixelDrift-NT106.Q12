using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase;


namespace PixelDirft_Server
{
    public class SupabaseHelper
    {
        private static Client _client;

        // Khởi tạo client Supabase
        public static async Task Initialize()
        {
            try
            {
                string url = "https://rppuqqzvoarjmoefezyj.supabase.co";
                string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJwcHVxcXp2b2Fyam1vZWZlenlqIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjA3OTEzNTIsImV4cCI6MjA3NjM2NzM1Mn0.IjiZuVa99-g5PxolFVXJ7hb76QWcNzLuhPJLYxnV_FM";

                _client = new Client(url, key, new SupabaseOptions
                {
                    AutoConnectRealtime = false
                });

                await _client.InitializeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể khởi tạo Supabase: " + ex.Message);
            }
        }

        public static Client Client
        {
            get
            {
                if (_client == null)
                    throw new InvalidOperationException("Supabase chưa được khởi tạo!");
                return _client;
            }
        }
    }
}
