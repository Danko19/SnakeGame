namespace SnakeGame.Server
{
    internal static class Program
    {
        public static void Main()
        {
            new TcpClientListener(32228, "192.168.1.8").Run();
        }
    }
}