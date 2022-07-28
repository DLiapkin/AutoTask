﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoTask.MVVM.ViewModel;
using DomainModule.Model;
using DomainModule.Repository;

namespace AutoTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            IEnumerable<User> users = unitOfWork.Users.GetAll().Where(u => u.IsLogged == true);
            foreach (User user in users)
            {
                user.IsLogged = false;
                unitOfWork.Users.Update(user);
            }
            unitOfWork.Save();
            unitOfWork.Dispose();
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
