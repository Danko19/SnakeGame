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
        private readonly Dictionary<IPEndPoint, SnakeController> snakes;

        public SnakeMoveUdpHandler(Dictionary<IPEndPoint, SnakeController> snakes)
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
                    snake.EnqueueCommand(newDirection);
                }
            });
            thread.Start();
            return thread;
        }
    }
}