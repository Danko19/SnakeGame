using System.Collections.Generic;
using System.Windows.Controls;
using SnakeGame.Domain.JsonModels;

namespace SnakeGame.Client
{
    public class PlaygroundDrawer
    {
        private readonly Canvas canvas;
        private readonly Dictionary<string, SnakeDrawer> snakeDrawers = new Dictionary<string, SnakeDrawer>();
        private readonly FoodDrawer foodDrawer;

        public PlaygroundDrawer(Canvas canvas)
        {
            this.canvas = canvas;
            foodDrawer = new FoodDrawer(canvas);
        }

        public void Show(MapJsonModel map)
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
            foodDrawer.Show(map.Foods, step);
        }

        public void Update(MapJsonModel map)
        {
            var step = canvas.Width / map.Width;
            foreach (var snake in map.Snakes)
            {
                var snakeDrawer = snakeDrawers[snake.Name];
                snakeDrawer.Update(snake, step);
            }
            foodDrawer.Update(map.Foods, step);
        }
    }
}