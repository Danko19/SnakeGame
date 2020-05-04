using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SnakeGame.Domain;
using SnakeGame.Domain.JsonModels;
using Point = SnakeGame.Domain.Point;

namespace SnakeGame.Client
{
    public class SnakeDrawer
    {
        private readonly Canvas canvas;
        private readonly Color color;
        private readonly Dictionary<Point, Rectangle> rectangles = new Dictionary<Point, Rectangle>();
        private readonly Ellipse[] eyes = new Ellipse[2];

        public SnakeDrawer(Canvas canvas, Color color)
        {
            this.canvas = canvas;
            this.color = color;
        }

        public void Show(SnakeJsonModel snake, double size)
        {
            foreach (var point in snake.Body)
                AddSnakeSegment(point, CreateRectangle(point, size));

            DrawEyes(snake);
        }

        public void Update(SnakeJsonModel snake, double size)
        {
            var points = snake.IsDead ? new HashSet<Point>() : snake.Body.ToHashSet();
            var pointsToAdd = points.Except(rectangles.Keys).ToList();
            var pointsToRemove = rectangles.Keys.Except(points).ToList();

            foreach (var point in pointsToAdd)
                AddSnakeSegment(point, CreateRectangle(point, size));

            foreach (var point in pointsToRemove)
                RemoveSnakeSegment(point);

            DrawEyes(snake, true);
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
                RadiusY = size / 3,
                Opacity = 0.75
            };
            Canvas.SetTop(rectangle, point.Y * size);
            Canvas.SetLeft(rectangle, point.X * size);
            return rectangle;
        }

        private void DrawEyes(SnakeJsonModel snake, bool update = false)
        {
            var head = snake.Body.FirstOrDefault();
            if (head == null)
                return;

            if (update)
            {
                foreach (var eye in eyes)
                    canvas.Children.Remove(eye);
            }

            (eyes[0], eyes[1]) = GetEyes(new Point(head.X * 10, head.Y * 10), snake.Direction);
            foreach (var eye in eyes)
                canvas.Children.Add(eye);
        }

        private (Ellipse, Ellipse) GetEyes(Point point, SnakeDirection direction)
        {
            var eye1 = new Ellipse
            {
                Width = 2,
                Height = 2,
                Fill = new SolidColorBrush(Colors.Black)
            };
            var eye2 = new Ellipse
            {
                Width = 2,
                Height = 2,
                Fill = new SolidColorBrush(Colors.Black)
            };
            var (point1, point2) = GetEyesPoints(point, direction);
            Canvas.SetTop(eye1, point1.Y);
            Canvas.SetLeft(eye1, point1.X);
            Canvas.SetZIndex(eye1, 1);
            Canvas.SetTop(eye2, point2.Y);
            Canvas.SetLeft(eye2, point2.X);
            Canvas.SetZIndex(eye2, 1);
            return (eye1, eye2);
        }

        private (Point, Point) GetEyesPoints(Point point, SnakeDirection direction)
        {
            Point TopLeft() => new Point(point.X + 1, point.Y + 1);
            Point TopRight() => new Point(point.X + 7, point.Y + 1);
            Point BottomLeft() => new Point(point.X + 1, point.Y + 7);
            Point BottomRight() => new Point(point.X + 7, point.Y + 7);
            if (direction == SnakeDirection.Up)
                return (TopLeft(), TopRight());
            if (direction == SnakeDirection.Down)
                return (BottomLeft(), BottomRight());
            if (direction == SnakeDirection.Left)
                return (TopLeft(), BottomLeft());
            return (TopRight(), BottomRight());
        }
    }
}