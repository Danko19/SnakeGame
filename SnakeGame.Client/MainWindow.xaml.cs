﻿using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using SnakeGame.Domain.JsonModels;

namespace SnakeGame.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public const int ServerPort = 32228;
        private readonly PlaygroundDrawer playgroundDrawer;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            playgroundDrawer = new PlaygroundDrawer(Playground);
        }

        public string Ip { get; set; }
        public string Nickname { get; set; }

        private void ConnectButton_OnClick(object sender, RoutedEventArgs es)
        {
            ConnectButton.IsEnabled = false;
            var tcpServerHandler = new TcpServerHandler(this);
            var tcpClient = new TcpClient(Ip, ServerPort);
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

        public void ShowPlayground(byte[] udpData)
        {
            var json = Encoding.UTF8.GetString(udpData);
            var mapJsonModel = JsonConvert.DeserializeObject<MapJsonModel>(json);
            Playground.Dispatcher.Invoke(() => playgroundDrawer.Show(mapJsonModel, Players));
        }

        public void UpdatePlayground(byte[] udpData)
        {
            var json = Encoding.UTF8.GetString(udpData);
            var mapJsonModel = JsonConvert.DeserializeObject<MapJsonModel>(json);
            Playground.Dispatcher.Invoke(() => playgroundDrawer.Update(mapJsonModel, Players));
        }
    }
}