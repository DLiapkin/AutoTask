using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoTask.UI.MVVM.Model.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace AutoTask.UI.MVVM.Model
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            var providerFactory = new AutofacServiceProviderFactory();
            ContainerBuilder builder = providerFactory.CreateBuilder(services);
            builder.RegisterType<Account>().As<IAccount>().SingleInstance();
            builder.RegisterType<MainWindow>().AsSelf();
            return builder.Build();
        }
    }
}
