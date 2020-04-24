using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SnakeGame.Domain;

namespace SnakeGame.Client
{
    public class SnakeDrawer
    {
        private readonly Canvas canvas;
        private readonly Color color;
        private readonly Dictionary<Point, Rectangle> rectangles = new Dictionary<Point, Rectangle>();

        public SnakeDrawer(Canvas canvas, Color color)
        {
            this.canvas = canvas;
            this.color = color;
        }

        public void Show(Snake snake, double size)
        {
            foreach (var point in snake.Body)
                AddSnakeSegment(point, CreateRectangle(point, size));
        }

        public void Update(Snake snake, double size)
        {
            var points = snake.Body.ToHashSet();
            var pointsToAdd = points.Except(rectangles.Keys);
            var pointsToRemove = rectangles.Keys.Except(points);

            foreach (var point in pointsToAdd)
                AddSnakeSegment(point, CreateRectangle(point, size));

            foreach (var point in pointsToRemove)
                RemoveSnakeSegment(point);
        }

        private void AddSnakeSegment(Point point, Rectangle rectangle)
        {
            canvas.Children.Add(rectangle);
            rectangles[point] = rectangle;
        }

        private void RemoveSnakeSegment(Point point)
        {
            var rectangle = rectangles[point];
            canvas.Children.Remove(rectangle);
            rectangles.Remove(point);
        }

        private Rectangle CreateRectangle(Point point, double size)
        {
            var rectangle = new Rectangle
            {
                Width = size,
                Height = size,
                Fill = new SolidColorBrush(color),
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 0.2,
                RadiusX = size / 3,
                RadiusY = size / 3
            };
            Canvas.SetTop(rectangle, point.Y * size);
            Canvas.SetLeft(rectangle, point.X * size);
            return rectangle;
        }
    }
}