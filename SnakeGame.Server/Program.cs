namespace SnakeGame.Server
{
    internal static class Program
    {
        public static void Main()
        {
            new TcpClientListener(32228, "127.0.0.1").Run();
        }
    }
}