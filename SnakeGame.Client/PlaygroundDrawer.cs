using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SnakeGame.Domain;

namespace SnakeGame.Client
{
    public class PlaygroundDrawer
    {
        private readonly Canvas canvas;

        public PlaygroundDrawer(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void Show(Map map)
        {
            var stepX = canvas.Width / map.Width;
            var stepY = canvas.Height / map.Height;
            DrawSnake(map.Snakes.First(), stepX - 1);
        }

        private void DrawSnake(Snake snake, double stepX, double stepY, double width)
        {
            var polyline = canvas.Children.Cast<Polyline>().SingleOrDefault(x => x.Name == snake.Name);
            if (polyline == null)
            {
                polyline = new Polyline();
                canvas.Children.Add(polyline);
            }
            
            polyline.Points.First().
        }
    }
}