using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SnakeGame.Domain;

namespace SnakeGame.Client
{
    public class FoodDrawer
    {
        private readonly Canvas canvas;
        private readonly Dictionary<Point, Ellipse> foods = new Dictionary<Point, Ellipse>();

        public FoodDrawer(Canvas canvas)
        {
            this.canvas = canvas;
        }

        public void Show(List<Point> newFoods, double size)
        {
            foreach (var point in newFoods)
            {
                var ellipse = CreateEllipse(point, size);
                AddFood(point, ellipse);
            }
        }

        public void Update(List<Point> newFoods, double size)
        {
            var points = newFoods.ToHashSet();
            var pointsToAdd = points.Except(foods.Keys).ToList();
            var pointsToRemove = foods.Keys.Except(points).ToList();

            foreach (var point in pointsToAdd)
                AddFood(point, CreateEllipse(point, size));

            foreach (var point in pointsToRemove)
                RemoveFood(point);
        }

        private void AddFood(Point point, Ellipse ellipse)
        {
            canvas.Children.Add(ellipse);
            foods[point] = ellipse;
        }

        private void RemoveFood(Point point)
        {
            var ellipse = foods[point];
            canvas.Children.Remove(ellipse);
            foods.Remove(point);
        }

        private Ellipse CreateEllipse(Point point, double size)
        {
            var ellipse = new Ellipse
            {
                Width = size - 1,
                Height = size - 1,
                Fill = new SolidColorBrush(Colors.Red),
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 0.2,
            };
            Canvas.SetTop(ellipse, point.Y * size);
            Canvas.SetLeft(ellipse, point.X * size);
            Canvas.SetZIndex(ellipse, -1);
            return ellipse;
        }
    }
}