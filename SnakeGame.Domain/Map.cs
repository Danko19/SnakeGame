using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Domain
{
    public class Map
    {
        private readonly List<Snake> snakes = new List<Snake>();
        private readonly HashSet<Point> foods = new HashSet<Point>();

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Point GetNewPoint(int x, int y)
        {
            if (x >= Width)
                x -= Width;
            if (y >= Height)
                y -= Height;
            return new Point(x, y);
        }

        public IEnumerable<Point> GetAllPoints()
        {
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
            {
                yield return new Point(x, y);
            }
        }

        public IEnumerable<Point> GetEmptyPoints()
        {
            var filledPoints = snakes
                .SelectMany(x => x.Body)
                .Concat(foods)
                .ToHashSet();
            var allPoints = GetAllPoints().ToHashSet();
            return allPoints.Except(filledPoints);
        }

        public void AddSnake(Snake snake)
        {
            snakes.Add(snake);
        }

        public void RemoveSnake(Snake snake)
        {
            snakes.Remove(snake);
        }

        public void AddFood(Point foodPoint)
        {
            foods.Add(foodPoint);
        }

        public void RemoveFood(Point foodPoint)
        {
            foods.Remove(foodPoint);
        }

        public bool IsFood(Point point)
        {
            return foods.Contains(point);
        }

        public bool IsSnake(Point point)
        {
            return snakes.SelectMany(x => x.Body).ToHashSet().Contains(point);
        }

        public int Width { get; }
        public int Height { get; }
        public IReadOnlyList<Snake> Snakes => snakes.AsReadOnly();
        public IReadOnlyList<Point> Foods => foods.ToList().AsReadOnly();
    }
}