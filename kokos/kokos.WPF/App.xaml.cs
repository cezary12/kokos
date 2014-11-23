using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Autofac.Core;
using kokos.WPF.ViewModel;

namespace kokos.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message + Environment.NewLine + e.Exception.StackTrace, "Exception",
                MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                   .AsSelf()
                   .AsImplementedInterfaces();
            var container = builder.Build();

            var window = container.Resolve<MainWindow>();
            window.DataContext = container.Resolve<MainViewModel>();
            window.Show();
        }
    }
}
