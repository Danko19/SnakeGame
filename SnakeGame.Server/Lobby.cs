using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using SnakeGame.Domain;

namespace SnakeGame.Server
{
    public class Lobby
    {
        private readonly ConcurrentDictionary<Player, IPEndPoint> players = new ConcurrentDictionary<Player, IPEndPoint>();

        public void AddPlayer(Player player, IPEndPoint endPoint)
        {
            Console.WriteLine($"New player {player.NickName} from {endPoint}");
            players.AddOrUpdate(player, (k) => endPoint, (k, v) => endPoint);
        }

        public void Run()
        {
            new Thread(() =>
            {
                var udpClient = new UdpClient(32228);
                while (players.IsEmpty || players.Keys.Any(x => !x.Ready))
                {
                    Thread.Sleep(100);
                }

                Game = new GameFactory().CreateNewGame(Players.Select(x => x.NickName).ToList());
                Thread.Sleep(1000);
                Console.WriteLine($"Created game for players {string.Join(", ", Players.Select(x => x.NickName))}");
                var dict = Game.Map.AliveSnakes.ToDictionary(
                    s => players.Single(x => x.Key.NickName == s.Name).Value,
                    s => s);
                var handler = new SnakeMoveUdpHandler(dict).Run(udpClient);
                var json = JsonConvert.SerializeObject(Game.Map.ToJsonModel());
                var bytes = Encoding.UTF8.GetBytes(json);
                foreach (var player in players.Values)
                {
                    udpClient.SendAsync(bytes, bytes.Length, player);
                }
                Thread.Sleep(3000);
                while (Game.Map.Winner == null)
                {
                    Game.Tick();
                    json = JsonConvert.SerializeObject(Game.Map.ToJsonModel());
                    bytes = Encoding.UTF8.GetBytes(json);
                    foreach (var player in players.Values)
                    {
                        udpClient.SendAsync(bytes, bytes.Length, player);
                    }
                    Thread.Sleep(100);
                }
            }).Start();
        }

        public Game Game { get; private set; }
        public IReadOnlyList<Player> Players => players.Keys.ToList().AsReadOnly();
    }
}