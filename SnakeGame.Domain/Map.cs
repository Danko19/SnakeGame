using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.Domain
{
    public class Map : IEnumerable<Cell>
    {
        private readonly Cell[,] cells;

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            cells = new Cell[width, height];
            Clear();
        }

        public void Clear()
        {
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                cells[x, y] = new Cell(x, y);
        }

        public Cell this[int x, int y] => cells[
            x >= Width ? x - Width : x,
            y >= Height ? y - Height : y];

        public int Width { get; private set; }
        public int Height { get; private set; }
        public IEnumerator<Cell> GetEnumerator()
        {
            return cells.Cast<Cell>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}