using System.Collections.Concurrent;
using SnakeGame.Domain;

namespace SnakeGame.Server
{
    public class SnakeController
    {
        private readonly Snake snake;
        private readonly ConcurrentQueue<SnakeDirection> commands = new ConcurrentQueue<SnakeDirection>();

        public SnakeController(Snake snake)
        {
            this.snake = snake;
        }

        public void EnqueueCommand(SnakeDirection direction)
        {
            commands.Enqueue(direction);
        }

        public void ApplyCommand()
        {
            if (!commands.TryDequeue(out var newDirection))
                return;

            var currentDirection = snake.Direction;
            var diff = newDirection - currentDirection;
            if (diff != 2 && diff != 254)
                snake.Direction = newDirection;
        }
    }
}