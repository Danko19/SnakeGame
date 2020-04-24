using System;
using System.Linq;

namespace SnakeGame.Domain
{
    public class GameFactory
    {
        private readonly Random random = new Random();

        public Game CreateNewGame(int width = 100, int height = 50, int snakesCount = 2)
        {
            var map = new Map(width, height);
            for (var i = 0; i < snakesCount; i++)
                AddSnake(map);
            return new Game(map);
        }

        private void AddSnake(Map map)
        {
            var emptyCells = map.GetEmptyPoints().ToList();
            var randomIndex = random.Next(emptyCells.Count);
            var direction = (SnakeDirection) random.Next(0, 4);
            var snake = new Snake(emptyCells[randomIndex], direction);
            map.AddSnake(snake);
        }
    }
}