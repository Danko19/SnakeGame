using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Domain
{
    public class Cell
    {
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
        public List<ICreature> Creatures { get; } = new List<ICreature>();
        public bool IsEmpty => !Creatures.Any();
    }
}