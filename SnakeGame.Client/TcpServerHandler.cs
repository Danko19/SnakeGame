using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using Newtonsoft.Json;
using SnakeGame.Domain.JsonModels;
using SnakeGame.Server;

namespace SnakeGame.Client
{
    public class TcpServerHandler
    {
        private readonly MainWindow mainWindow;

        public TcpServerHandler(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void Handle(TcpClient client)
        {
            try
            {
                var usedPort = ((IPEndPoint) client.Client.LocalEndPoint).Port;
                CreateUdp(usedPort);
                using (var tcpTerminal = new TcpTerminal(client))
                {
                    tcpTerminal.WriteCommand(new TcpCommand("LOGIN", mainWindow.Nickname));
                    mainWindow.ReadyButton.Click += (o, e) =>
                    {
                        tcpTerminal.WriteCommand(new TcpCommand("READY", ""));
                        mainWindow.ReadyButton.Dispatcher.Invoke(() =>
                        {
                            mainWindow.ReadyButton.Visibility = Visibility.Hidden;
                            mainWindow.ReadyButton.IsEnabled = false;
                        });
                    };
                    var start = false;
                    do
                    {
                        var command = tcpTerminal.ReadCommand();
                        if (command?.Method == "PLAYERS")
                        {
                            var playersServer = JsonConvert.DeserializeObject<List<Player>>(command.Data);
                            var playersClient = mainWindow.Players.Items;
                            for (int i = 0; i < playersClient.Count; i++)
                            {
                                var clientPlayer = (string) playersClient[i];
                                var serverPlayer = playersServer.Single(x => clientPlayer.StartsWith(x.NickName));
                                mainWindow.Players.Dispatcher.Invoke(() =>
                                {
                                    playersClient[i] = serverPlayer.ToString();
                                    playersServer.Remove(serverPlayer);
                                });
                            }

                            foreach (var newPlayers in playersServer)
                            {
                                mainWindow.Players.Dispatcher.Invoke(() => playersClient.Add(newPlayers.ToString()));
                                ;
                            }
                        }

                        if (command?.Method == "START")
                        {
                            start = true;
                            Thread.Sleep(3000);
                        }
                    } while (!start);
                }

                client.Close();
            }
            catch (ArgumentNullException e)
            {
            }
            catch (SocketException e)
            {
            }
        }


        private void CreateUdp(int port)
        {
            new Thread(() => { new UdpServerHandler(mainWindow).Handle(new UdpClient(port)); }).Start();
        }
    }
}