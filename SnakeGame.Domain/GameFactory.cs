using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Domain
{
    public class GameFactory
    {
        private readonly Random random = new Random();

        public Game CreateNewGame(int width = 100, int height = 50, int snakesCount = 2)
        {
            var map = new Map(width, height);
            var snakes = new List<Snake>();
            for (int i = 0; i < snakesCount; i++)
                snakes.Add(CreateSnake(map));
            return new Game(map, snakes);
        }

        private Snake CreateSnake(Map map)
        {
            var emptyCells = map.Where(x => x.IsEmpty).ToList();
            var randomIndex = random.Next(emptyCells.Count);
            var direction = (SnakeDirection) random.Next(0, 4);
            return new Snake(emptyCells[randomIndex], direction);
        }
    }
}