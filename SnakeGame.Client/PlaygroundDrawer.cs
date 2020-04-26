using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        public void Show(MapJsonModel map, ListBox players)
        {
            var step = canvas.Width / map.Width;
            players.Items.Clear();
            for (var i = 0; i < map.Snakes.Count; i++)
            {
                var color = SnakeColors.PreparedColors[i];
                var snakeDrawer = new SnakeDrawer(canvas, color);
                var snake = map.Snakes[i];
                snakeDrawers.Add(snake.Name, snakeDrawer);
                snakeDrawer.Show(snake, step);
                players.Items.Add(new TextBlock {Background = new SolidColorBrush(color), Text = snake.Name});
            }

            foodDrawer.Show(map.Foods, step);
        }

        public void Update(MapJsonModel map, ListBox players)
        {
            var step = canvas.Width / map.Width;
            foreach (var snake in map.Snakes)
            {
                if (!snakeDrawers.TryGetValue(snake.Name, out var snakeDrawer))
                    continue;

                snakeDrawer.Update(snake, step);
                if (snake.IsDead)
                {
                    snakeDrawers.Remove(snake.Name);
                    var textBlock = players.Items.Cast<TextBlock>().Single(x => x.Text.StartsWith(snake.Name));
                    textBlock.Text = $"{snake.Name} - Dead";
                }
            }

            foodDrawer.Update(map.Foods, step);
            if (map.Winner != null)
            {
                var textBlock = players.Items.Cast<TextBlock>().Single(x => x.Text.StartsWith(map.Winner));
                textBlock.Text = $"{map.Winner} - Winner!";
            }
        }
    }
}