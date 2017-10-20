using System.Windows;

using MVVMApplication.View;

namespace MVVMApplication
{
    /// <summary>
    /// Interaction logic for FrontWindow.xaml
    /// </summary>
    public partial class FrontWindow : Window
    {
        public FrontWindow()
        {
            InitializeComponent();
        }

        private void Plain_Click(object sender, RoutedEventArgs e)
        {
            //MainWindowViewModel mainViewModel =
            //    AutoMapperHelpers.GetNewViewModel<MainWindowViewModel>("MainWindowVM", propFactory: SettingsExtensions.ThePropFactory);

            MainWindowPlain mwp = new MainWindowPlain();

            mwp.ShowDialog();
        }

        private void PropBagProxyEmit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();

            mw.ShowDialog();
        }
    }
}
