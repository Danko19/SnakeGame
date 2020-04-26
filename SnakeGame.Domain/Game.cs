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
                foreach (var snake in Map.AliveSnakes)
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
            var conflictedSnakes = Map.AliveSnakes.Where(s => s.Body.Contains(conflictPoint)).ToList();
            var deadSnakes = conflictedSnakes.Where(s => s.Head.Equals(conflictPoint)).ToList();
            deadSnakes.ForEach(x => x.IsDead = true);

            if (Map.AliveSnakes.Count == 1)
                Map.Winner = Map.AliveSnakes.Single().Name;

            if (Map.AliveSnakes.Count == 0)
                Map.Winner = deadSnakes.OrderByDescending(x => x.Body.Count).First().Name;
        }

        public Map Map { get; }
    }
}