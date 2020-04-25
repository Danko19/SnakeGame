namespace SnakeGame.Server
{
    public class Player
    {
        public Player(string nickName)
        {
            NickName = nickName;
        }

        public string NickName { get; }
        public bool Ready { get; set; }

        public override string ToString()
        {
            return $"{NickName} ({(Ready ? "Ready" : "Not ready")})";
        }
    }
}