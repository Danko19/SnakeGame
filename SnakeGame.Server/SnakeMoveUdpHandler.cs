using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SnakeGame.Domain;

namespace SnakeGame.Server
{
    public class SnakeMoveUdpHandler
    {
        private readonly Snake snake;

        public SnakeMoveUdpHandler(Snake snake)
        {
            this.snake = snake;
        }

        public Thread Run(UdpClient udpClient, IPEndPoint endPoint)
        {
            var thread = new Thread(start: () =>
            {
                while (true)
                {
                    var receive = udpClient.Receive(ref endPoint).Single();
                    var newDirection = (SnakeDirection) receive;
                    var currentDirection = snake.Direction;
                    var diff = newDirection - currentDirection;
                    if (diff != 2 && diff != -2)
                        snake.Direction = newDirection;
                }
            });
            thread.Start();
            return thread;
        }
    }
}