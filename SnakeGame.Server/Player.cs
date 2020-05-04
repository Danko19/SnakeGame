using System.Net;
using Newtonsoft.Json;

namespace SnakeGame.Server
{
    public class Player
    {
        public Player(string nickName, IPAddress ip)
        {
            NickName = nickName;
            Ip = ip;
        }

        public string NickName { get; }
        
        [JsonIgnore]
        public IPAddress Ip { get; }
        public bool Ready { get; set; }
        
        [JsonIgnore]
        public int UdpPort { get; set; }

        public override string ToString()
        {
            return $"{NickName} ({(Ready ? "Ready" : "Not ready")})";
        }
    }
}