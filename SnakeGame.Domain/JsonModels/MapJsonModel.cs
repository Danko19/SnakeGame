using System.Collections.Generic;

namespace SnakeGame.Domain.JsonModels
{
    public class MapJsonModel
    {
        public List<SnakeJsonModel> Snakes { get; set; } = new List<SnakeJsonModel>();
        public List<Point> Foods { get; set; } = new List<Point>();
        public string Winner { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}