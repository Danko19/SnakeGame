namespace SnakeGame.Server
{
    static class Program
    {
        public static void Main()
        {
            var lobby = new Lobby();
            var loginHandler = new LoginHandler(32228, "127.0.0.1", lobby);
            loginHandler.Run();
        }
    }
}