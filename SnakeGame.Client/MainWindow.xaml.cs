using System;
using System.IO;
using System.Net.Sockets;
using System.Windows;

namespace SnakeGame.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const int port = 32228;
        private readonly PlaygroundDrawer playgroundDrawer;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            playgroundDrawer = new PlaygroundDrawer(Playground);
        }

        public string Ip { get; set; } = "";
        public string Nickname { get; set; } = "";

        private void ButtonBase_OnClick(object sender, RoutedEventArgs es)
        {
            try
            {
                var client = new TcpClient(Ip, port);
        
                using (var writer = new StreamWriter(client.GetStream()))
                {
                    writer.Write(Nickname);
                }
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        
            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}