using MahApps.Metro.Controls;
using System.Windows;

namespace kokos.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            PasswordBox.GotFocus += PasswordBox_GotFocus;
        }

        void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox.SelectAll();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoginBox.CaretIndex = int.MaxValue;
        }
    }
}
