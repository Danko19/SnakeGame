using System.Net;
using System.Net.Sockets;

namespace SnakeGame.Client
{
    public static class PortFinder
    {
        public static int NextFreePort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            var port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}