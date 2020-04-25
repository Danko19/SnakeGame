using System.Linq;

namespace SnakeGame.Server
{
    public class TcpCommand
    {
        public TcpCommand(string method, string data)
        {
            Method = method.ToUpper();
            Data = data;
        }


        public string Method { get; }
        public string Data { get; }

        public override string ToString()
        {
            return $"{Method} {Data}";
        }

        public static TcpCommand FromString(string rawCommand)
        {
            if (rawCommand == null)
                return null;
            var method = rawCommand.Split(' ').First();
            var data = new string(rawCommand.Skip(method.Length + 1).ToArray());
            return new TcpCommand(method, data);
        }
    }
}