using System.Collections.Generic;
using System.Linq;
using SnakeGame.Domain.JsonModels;

namespace SnakeGame.Domain
{
    public class Snake
    {
        private readonly LinkedList<Point> body = new LinkedList<Point>();

        public Snake(Point start, SnakeDirection direction, string name)
        {
            body.AddFirst(start);
            Direction = direction;
            Name = name;
        }

        public IReadOnlyList<Point> Body => body.ToList().AsReadOnly();
        public Point Head => body.First();
        public Point Tail => body.Last();
        public SnakeDirection Direction { get; set; }
        public string Name { get; }
        public bool IsDead { get; set; }

        public void Move(Map map)
        {
            var newX = Head.X;
            var newY = Head.Y;
            var direction = Direction;
            if (direction == SnakeDirection.Up)
                newY--;
            else if (direction == SnakeDirection.Down)
                newY++;
            else if (direction == SnakeDirection.Left)
                newX--;
            else if (direction == SnakeDirection.Right)
                newX++;
            var newHead = map.GetNewPoint(newX, newY);

            var isConflict = map.IsSnake(newHead);

            body.AddFirst(newHead);
            if (map.IsFood(Tail))
                map.RemoveFood(Tail);
            else body.RemoveLast();

            if (isConflict)
                throw new SnakeConflictException(newHead);
        }

        public SnakeJsonModel ToJsonModel()
        {
            return new SnakeJsonModel
            {
                Name = Name,
                Direction = Direction,
                Body = body.ToList(),
                IsDead = IsDead
            };
        }
    }
}