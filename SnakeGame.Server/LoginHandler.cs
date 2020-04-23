using System.IO;
using System.Net;
using System.Net.Sockets;

namespace SnakeGame.Server
{
    public class LoginHandler : TcpHandlerBase
    {
        private readonly Lobby lobby;

        public LoginHandler(int port, string ip, Lobby lobby) : base(port, ip)
        {
            this.lobby = lobby;
        }

        protected override void HandleRequest(TcpClient client)
        {
            var endPoint = (IPEndPoint) client.Client.RemoteEndPoint;
            using (var reader = new StreamReader(client.GetStream()))
            {
                var nickName = reader.ReadToEnd();
                var player = new Player(nickName, endPoint);
                lobby.AddPlayer(player);
            }
        }
    }
}