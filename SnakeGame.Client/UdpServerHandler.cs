using System.Net;
using System.Net.Sockets;
using System.Windows.Input;
using SnakeGame.Domain;

namespace SnakeGame.Client
{
    public class UdpServerHandler
    {
        private readonly MainWindow mainWindow;
        private readonly IPEndPoint udpServer;

        public UdpServerHandler(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            udpServer = new IPEndPoint(IPAddress.Parse(mainWindow.Ip), MainWindow.ServerPort);
        }

        public void Handle(UdpClient udpClient)
        {
            udpClient.Send(new byte[] {1, 2, 3}, 3, mainWindow.Ip, 32228);
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(mainWindow.Ip), 0);
            PlayGame(udpClient, ipEndPoint);
        }

        private void PlayGame(UdpClient udpClient, IPEndPoint ipEndPoint)
        {
            var receive = udpClient.Receive(ref ipEndPoint);
            mainWindow.ShowPlayground(receive);
            mainWindow.KeyDown += (_, e) => KeyDown(e, udpClient);
            while (true)
            {
                receive = udpClient.Receive(ref ipEndPoint);
                mainWindow.UpdatePlayground(receive);
            }
        }

        private void KeyDown(KeyEventArgs e, UdpClient udpClient)
        {
            if (e.Key == Key.Up)
                udpClient.Send(new[] {(byte) SnakeDirection.Up}, 1, udpServer);
            if (e.Key == Key.Down)
                udpClient.Send(new[] {(byte) SnakeDirection.Down}, 1, udpServer);
            if (e.Key == Key.Left)
                udpClient.Send(new[] {(byte) SnakeDirection.Left}, 1, udpServer);
            if (e.Key == Key.Right)
                udpClient.Send(new[] {(byte) SnakeDirection.Right}, 1, udpServer);
        }
    }
}