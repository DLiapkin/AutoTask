using Autofac;
using AutoTask.UI.MVVM.Model;
using System.Windows;

namespace AutoTask.UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer container;

        public App()
        {
            container = ContainerConfig.Configure();
        }

        private void OnStartUp(object sender, StartupEventArgs e)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                var mainWindow = container.Resolve<MainWindow>();
                mainWindow.Show();
            }
        }
    }
}
