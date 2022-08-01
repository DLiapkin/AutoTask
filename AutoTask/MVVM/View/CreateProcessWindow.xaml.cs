using System.Windows;
using System.Windows.Input;
using AutoTask.UI.MVVM.ViewModel;

namespace AutoTask.UI.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для CreateProcessWindow.xaml
    /// </summary>
    public partial class CreateProcessWindow : Window
    {
        public CreateProcessWindow(ProcessViewModel processViewModel)
        {
            InitializeComponent();
            this.DataContext = processViewModel;
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
