using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
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
                while (players.IsEmpty || players.Keys.Any(x => !x.Ready))
                {
                    Thread.Sleep(100);
                }
                
                Game = new GameFactory().CreateNewGame(Players.Select(x => x.NickName).ToList());
                Thread.Sleep(3000);
            }).Start();
        }

        public Game Game { get; private set; }
        public IReadOnlyList<Player> Players => players.Keys.ToList().AsReadOnly();
    }
}