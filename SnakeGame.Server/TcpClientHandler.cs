using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SnakeGame.Server
{
    public class TcpClientHandler
    {
        private readonly Lobby lobby;

        public TcpClientHandler(Lobby lobby)
        {
            this.lobby = lobby;
        }

        public void Handle(TcpClient client)
        {
            var endPoint = (IPEndPoint) client.Client.RemoteEndPoint;
            Console.WriteLine($"Start handle {endPoint}");
            using (var tcpTerminal = new TcpTerminal(client))
            {
                var command = tcpTerminal.ReadCommand();
                if (command.Method != "LOGIN")
                    return;
                var nickName = command.Data;
                Console.WriteLine($"{nickName} is logged in");
                var player = new Player(nickName);
                lobby.AddPlayer(player, endPoint);

                var waitAllTask = Task.Run(() => NotifyPlayersList(tcpTerminal));

                command = tcpTerminal.ReadCommand();
                if (command.Method != "READY")
                    return;
                Console.WriteLine($"{nickName} is ready");
                player.Ready = true;
                waitAllTask.Wait();
                tcpTerminal.WriteCommand(new TcpCommand("START", ""));
            }

            client.Close();
        }

        private void NotifyPlayersList(TcpTerminal terminal)
        {
            while (lobby.Game == null)
            {
                var command = new TcpCommand("PLAYERS", JsonConvert.SerializeObject(lobby.Players));
                terminal.WriteCommand(command);
                Thread.Sleep(1000);
            }
        }
    }
}