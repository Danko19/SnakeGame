using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SnakeGame.Domain;

namespace SnakeGame.Server
{
    public class SnakeMoveUdpHandler
    {
        private readonly Dictionary<IPEndPoint, Snake> snakes;

        public SnakeMoveUdpHandler(Dictionary<IPEndPoint, Snake> snakes)
        {
            this.snakes = snakes;
        }

        public Thread Run(UdpClient udpClient)
        {
            var thread = new Thread(start: () =>
            {
                while (true)
                {
                    IPEndPoint nEp = null;
                    var receive = udpClient.Receive(ref nEp);
                    if (receive.Length != 1)
                        continue;

                    var snake = snakes[nEp];
                    var newDirection = (SnakeDirection) receive.Single();
                    var currentDirection = snake.Direction;
                    var diff = newDirection - currentDirection;
                    if (diff != 2 && diff != 254)
                        snake.Direction = newDirection;
                }
            });
            thread.Start();
            return thread;
        }
    }
}