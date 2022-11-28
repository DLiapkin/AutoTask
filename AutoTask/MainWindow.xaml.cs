using System;
using System.Configuration;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AutoTask.UI.Core.Interface;
using AutoTask.UI.MVVM.Model.Interface;
using AutoTask.UI.MVVM.ViewModel;

namespace AutoTask.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IAccount account, IHttpClientFactory clientFactory, IAccountOperation operation)
        {
            InitializeComponent();
            HttpClient client = clientFactory.CreateClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings.Get("AutoTaskApi"));
            DataContext = new MainViewModel(account, client, operation);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void WindowStateButtonClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }
    }
}
