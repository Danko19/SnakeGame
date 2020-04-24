﻿using System;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs es)
        {
            try
            {
                var port = 32228;
                var client = new TcpClient("127.0.0.1", port);

                using (var writer = new StreamWriter(client.GetStream()))
                {
                    writer.Write(TextBox.Text);
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