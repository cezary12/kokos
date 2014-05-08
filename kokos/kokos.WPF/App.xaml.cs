using System;
using System.Windows;
using System.Windows.Threading;

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
    }
}
