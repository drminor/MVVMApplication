using MVVMApplication.Infra;
using MVVMApplication.ViewModel;
using System.Windows;

namespace MVVMApplication
{
    /// <summary>
    /// Interaction logic for StartUpWindow.xaml
    /// </summary>
    public partial class StartUpWindow : Window
    {
        public StartUpWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel mainViewModel =
                AutoMapperHelpers.GetNewViewModel<MainWindowViewModel>("MainWindowVM", propFactory: SettingsExtensions.ThePropFactory);

            MainWindow mw = new MainWindow(mainViewModel);

            mw.ShowDialog();
        }
    }
}
