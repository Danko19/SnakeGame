using System.Net.Sockets;
using System.Threading.Tasks;
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

        public string Ip { get; set; } = "127.0.0.1";
        public string Nickname { get; set; } = "Danko";

        private void ConnectButton_OnClick(object sender, RoutedEventArgs es)
        {
            ConnectButton.IsEnabled = false;
            var tcpServerHandler = new TcpServerHandler(this);
            var tcpClient = new TcpClient(Ip, port);
            Task.Run(() => tcpServerHandler.Handle(tcpClient));
            ConnectButton.Visibility = Visibility.Hidden;
            IpInput.IsEnabled = false;
            IpInput.Visibility = Visibility.Hidden;
            NicknameInput.IsEnabled = false;
            NicknameInput.Visibility = Visibility.Hidden;
            Playground.Visibility = Visibility.Visible;
            Lobby.IsEnabled = true;
            Lobby.Visibility = Visibility.Visible;
        }
    }
}