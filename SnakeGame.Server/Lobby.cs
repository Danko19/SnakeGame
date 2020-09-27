using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly ConcurrentBag<Player> players = new ConcurrentBag<Player>();

        public void AddPlayer(Player player)
        {
            Console.WriteLine($"New player {player.NickName} from {player.Ip}");
            players.Add(player);
        }

        public void Run()
        {
            new Thread(() =>
            {
                var udpClient = new UdpClient(32228);
                while (players.IsEmpty || players.Any(x => !x.Ready))
                {
                    Thread.Sleep(100);
                }

                while (true)
                {
                    StartNewGame(udpClient);
                }
            }).Start();
        }

        private void StartNewGame(UdpClient udpClient)
        {
            Game = new GameFactory().CreateNewGame(Players.Select(x => x.NickName).ToList());
            Thread.Sleep(1000);
            Console.WriteLine($"Created game for players {string.Join(", ", Players.Select(x => x.NickName))}");
            var dict = Game.Map.AliveSnakes.ToDictionary(
                s => GetUdpEndPoint(players.Single(p => p.NickName == s.Name)),
                s => new SnakeController(s));
            var endPoints = dict.Keys.ToList();
            var handler = new SnakeMoveUdpHandler(dict).Run(udpClient);
            var json = JsonConvert.SerializeObject(Game.Map.ToJsonModel());
            var bytes = Encoding.UTF8.GetBytes(json);
            foreach (var player in endPoints)
            {
                udpClient.SendAsync(bytes, bytes.Length, player);
            }

            Thread.Sleep(3000);
            var sw = new Stopwatch();
            while (Game.Map.Winner == null)
            {
                sw.Restart();
                foreach (var controllers in dict.Values)
                {
                    controllers.ApplyCommand();
                }

                Game.Tick();
                json = JsonConvert.SerializeObject(Game.Map.ToJsonModel());
                bytes = Encoding.UTF8.GetBytes(json);
                foreach (var player in endPoints)
                {
                    udpClient.SendAsync(bytes, bytes.Length, player);
                }

                var sleep = 100 - sw.ElapsedMilliseconds;
                if (sleep > 0)
                    Thread.Sleep(TimeSpan.FromMilliseconds(sleep));
            }

            handler.Abort();
        }

        private static IPEndPoint GetUdpEndPoint(Player player)
        {
            return new IPEndPoint(player.Ip, player.UdpPort);
        }

        public Game Game { get; private set; }
        public IReadOnlyList<Player> Players => players.ToList().AsReadOnly();
    }
}