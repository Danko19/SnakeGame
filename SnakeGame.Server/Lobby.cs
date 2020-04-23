using System;
using System.Collections.Generic;

namespace SnakeGame.Server
{
    public class Lobby
    {
        private readonly List<Player> players = new List<Player>();

        public void AddPlayer(Player player)
        {
            Console.WriteLine($"New player {player.NickName} from {player.EndPoint}");
            players.Add(player);
        }
    }
}