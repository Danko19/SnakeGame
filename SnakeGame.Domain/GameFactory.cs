using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Domain
{
    public class GameFactory
    {
        private readonly Random random = new Random();

        public Game CreateNewGame(IEnumerable<string> players, int width = 100, int height = 50)
        {
            var map = new Map(width, height);
            foreach (var player in players)
            {
                AddSnake(map, player);
            }
            return new Game(map);
        }

        private void AddSnake(Map map, string nickname)
        {
            var emptyCells = map.GetEmptyPoints().ToList();
            var randomIndex = random.Next(emptyCells.Count);
            var direction = (SnakeDirection) random.Next(0, 4);
            var snake = new Snake(emptyCells[randomIndex], direction, nickname);
            map.AddSnake(snake);
        }
    }
}