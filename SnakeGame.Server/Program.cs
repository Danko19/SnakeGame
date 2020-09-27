using System;
using System.Linq;

namespace SnakeGame.Server
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            if (args?.Length != 1)
            {
                Console.WriteLine("Enter server ip");
                return;
            }

            new TcpClientListener(32228, args.Single()).Run();
        }
    }
}