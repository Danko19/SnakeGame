using System;
using System.Linq;

namespace SnakeGame.Domain
{
    public class Game
    {
        private readonly Random random = new Random();

        public Game(Map map)
        {
            Map = map;
        }

        public void Tick()
        {
            try
            {
                GenerateFood();
                foreach (var snake in Map.AliveSnakes)
                    snake.Move(Map);
            }
            catch (SnakeConflictException e)
            {
                HandleConflict(e);
            }
        }

        private void GenerateFood()
        {
            var snakes = Map.AliveSnakes.Count;
            var foods = Map.Foods.Count;
            var foodToGenerate = snakes * 2 - foods;
            if (foodToGenerate == 0)
                return;
            var points = Map.GetEmptyPoints().ToList();
            var newFood = points.OrderBy(_ => random.Next()).Take(foodToGenerate).ToArray();
            foreach (var food in newFood)
            {
                Map.AddFood(food);
            }
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