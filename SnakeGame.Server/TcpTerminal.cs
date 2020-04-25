using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace SnakeGame.Server
{
    public class TcpTerminal : IDisposable
    {
        private readonly NetworkStream stream;
        private readonly BinaryReader reader;
        private readonly BinaryWriter writer;
        private readonly object lockerWrite = new object();
        private readonly object lockerRead = new object();

        public TcpTerminal(TcpClient tcpClient)
        {
            stream = tcpClient.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }

        public TcpCommand ReadCommand()
        {
            lock (lockerRead)
            {
                return TcpCommand.FromString(reader.ReadLine());
            }
        }

        public void WriteCommand(TcpCommand command)
        {
            lock (lockerWrite)
            {
                var bytes = Encoding.UTF8.GetBytes(command + "\r\n");
                writer.Write(bytes);
                writer.Flush();
            }
        }

        public void Dispose()
        {
            reader.Close();
            writer.Close();
            stream.Close();
        }
    }
}