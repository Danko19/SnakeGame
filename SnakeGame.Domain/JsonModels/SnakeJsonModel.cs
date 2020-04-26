using System.Collections.Generic;

namespace SnakeGame.Domain.JsonModels
{
    public class SnakeJsonModel
    {
        public List<Point> Body { get; set; } = new List<Point>();
        public SnakeDirection Direction { get; set; }
        public string Name { get; set; }
        public bool IsDead { get; set; }
    }
}