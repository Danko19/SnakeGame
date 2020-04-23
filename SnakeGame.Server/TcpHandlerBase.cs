using System;
using System.Net;
using System.Net.Sockets;

namespace SnakeGame.Server
{
    public abstract class TcpHandlerBase
    {
        private readonly int port;
        private readonly string ip;
        private readonly TcpListener server;

        protected TcpHandlerBase(int port, string ip)
        {
            this.port = port;
            this.ip = ip;
            server = new TcpListener(IPAddress.Parse(ip), port);
        }

        public void Run()
        {
            try
            {
                server.Start();
                
                while (true)
                {
                    Console.Write("Waiting for a connection... ");
                    var client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    HandleRequest(client);
                    client.Close();
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

        protected abstract void HandleRequest(TcpClient client);
    }
}