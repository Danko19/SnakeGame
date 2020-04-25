using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SnakeGame.Server
{
    public class TcpClientListener
    {
        private readonly TcpListener server;
        private readonly Lobby lobby = new Lobby();

        public TcpClientListener(int port, string ip)
        {
            server = new TcpListener(IPAddress.Parse(ip), port);
        }

        public void Run()
        {
            try
            {
                server.Start();

                while (true)
                {
                    Console.WriteLine("Waiting for connection...");
                    var client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    Task.Run(() => new TcpClientHandler(lobby).Handle(client));
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server?.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}