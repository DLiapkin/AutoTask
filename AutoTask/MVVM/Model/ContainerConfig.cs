using Autofac;
using AutoTask.UI.MVVM.Model;
using AutoTask.UI.MVVM.Model.Interface;

namespace AutoTask.UI.MVVM.Model
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Account>().As<IAccount>().SingleInstance();
            builder.RegisterType<MainWindow>().AsSelf();

            return builder.Build();
        }
    }
}
