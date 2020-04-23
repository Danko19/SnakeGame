using System.Net;

namespace SnakeGame.Server
{
    public class Player
    {
        public Player(string nickName, IPEndPoint endPoint)
        {
            NickName = nickName;
            EndPoint = endPoint;
        }

        public string NickName { get; }
        public IPEndPoint EndPoint { get; }
    }
}