using System.Windows;
using System.Windows.Input;
using AutoTask.UI.MVVM.ViewModel;

namespace AutoTask.UI.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow(AuthorizationViewModel authorizationViewModel)
        {
            InitializeComponent();
            this.DataContext = authorizationViewModel;
        }

        private void WindowClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}
