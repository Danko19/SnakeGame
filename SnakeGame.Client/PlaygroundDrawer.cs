using System.Collections.Generic;
using System.Windows.Controls;
using SnakeGame.Domain;

namespace SnakeGame.Client
{
    public class PlaygroundDrawer
    {
        private readonly Canvas canvas;
        private readonly Dictionary<string, SnakeDrawer> snakeDrawers = new Dictionary<string, SnakeDrawer>();

        public PlaygroundDrawer(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void Show(Map map)
        {
            var step = canvas.Width / map.Width;
            for (var i = 0; i < map.Snakes.Count; i++)
            {
                var color = SnakeColors.PreparedColors[i];
                var snakeDrawer = new SnakeDrawer(canvas, color);
                var snake = map.Snakes[i];
                snakeDrawers.Add(snake.Name, snakeDrawer);
                snakeDrawer.Show(snake, step);
            }
        }

        public void Update(Map map)
        {
            var step = canvas.Width / map.Width;
            foreach (var snake in map.Snakes)
            {
                var snakeDrawer = snakeDrawers[snake.Name];
                snakeDrawer.Update(snake, step);
            }
        }
    }
}