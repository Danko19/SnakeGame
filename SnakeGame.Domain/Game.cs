using System;
using System.Linq;

namespace SnakeGame.Domain
{
    public class Game
    {
        private readonly int foodFrequency;
        private readonly Random random = new Random();
        private long tick = 0;

        public Game(Map map, int foodFrequency = 10)
        {
            this.foodFrequency = foodFrequency;
            Map = map;
        }

        public void Tick()
        {
            try
            {
                foreach (var snake in Map.Snakes)
                    snake.Move(Map);

                if (tick++ % foodFrequency == 0)
                    GenerateFood();
            }
            catch (SnakeConflictException e)
            {
                HandleConflict(e);
            }
        }

        private void GenerateFood()
        {
            var points = Map.GetEmptyPoints().ToList();
            if (points.Count == 0)
                return;
            if (points.Count == 1)
                Map.AddFood(points.Single());
            var randomIndex = random.Next(0, points.Count);
            Map.AddFood(points[randomIndex]);
        }

        private void HandleConflict(SnakeConflictException exception)
        {
            var conflictPoint = exception.Point;
            var conflictedSnakes = Map.Snakes.Where(s => s.Body.Contains(conflictPoint)).ToList();
            var deadSnakes = conflictedSnakes.Where(s => s.Head.Equals(conflictPoint)).ToList();
            deadSnakes.ForEach(Map.RemoveSnake);

            if (Map.Snakes.Count == 1)
                Winner = Map.Snakes.Single();

            if (Map.Snakes.Count == 0)
                Winner = deadSnakes.OrderByDescending(x => x.Body.Count).First();
        }

        public Map Map { get; }
        public Snake Winner { get; private set; }
    }
}