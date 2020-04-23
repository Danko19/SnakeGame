using System;

namespace SnakeGame.Domain
{
    public class EndGameException : Exception
    {
        public EndGameException(Snake winner)
        {
            Winner = winner;
        }
        
        public Snake Winner { get; }
    }
}