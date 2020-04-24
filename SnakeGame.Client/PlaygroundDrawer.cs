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
            canvas.Visibility = Visibility.Visible;
            var stepX = canvas.Width / map.Width;
            var stepY = canvas.Height / map.Height;
            for (var x = 0; x <= map.Width; x++)
            {
                var markupLine = CreateMarkupLine(x * stepX, x * stepX, 0, canvas.Height);
                canvas.Children.Add(markupLine);
            }

            for (var y = 0; y <= map.Height; y++)
            {
                var markupLine = CreateMarkupLine(0, canvas.Width, y * stepY, y * stepY);
                canvas.Children.Add(markupLine);
            }
        }

        private Line CreateMarkupLine(double x1, double x2, double y1, double y2)
        {
            return new Line
            {
                X1 = x1,
                X2 = x2,
                Y1 = y1,
                Y2 = y2,
                Stroke = Brushes.DimGray,
                StrokeThickness = 1
            };
        }
    }
}