using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Domain
{
    public class Snake : ICreature
    {
        private readonly LinkedList<Cell> body = new LinkedList<Cell>();

        public Snake(Cell emptyCell, SnakeDirection direction)
        {
            AddHeadCell(emptyCell);
            Direction = direction;
        }

        public IReadOnlyList<Cell> Body => body.ToList();

        public SnakeDirection Direction { get; set; }

        public void Move(Map map)
        {
            var head = body.First.Value;
            var newX = head.X;
            var newY = head.Y;
            if (Direction == SnakeDirection.Up)
                newY--;
            else if (Direction == SnakeDirection.Down)
                newY++;
            else if (Direction == SnakeDirection.Left)
                newX--;
            else if (Direction == SnakeDirection.Right)
                newX++;
            var newHead = map[newX, newY];
            AddHeadCell(newHead);
            RemoveTailCell();
        }

        private void AddHeadCell(Cell cell)
        {
            cell.Creatures.Add(this);
            body.AddFirst(cell);
        }

        private void RemoveTailCell()
        {
            var tail = body.Last.Value;
            var food = tail.Creatures.SingleOrDefault(c => c is Food);
            if (food != null)
                tail.Creatures.Remove(food);
            else
            {
                tail.Creatures.Remove(this);
                body.RemoveLast();
            }
        }
    }
}