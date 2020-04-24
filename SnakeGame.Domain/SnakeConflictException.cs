using System;

namespace SnakeGame.Domain
{
    public class SnakeConflictException : Exception
    {
        public SnakeConflictException(Point point)
        {
            Point = point;
        }

        public Point Point { get; }
    }
}