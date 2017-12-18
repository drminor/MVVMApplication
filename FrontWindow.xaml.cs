using MVVMApplication.View;
using System.Windows;

namespace MVVMApplication
{
    public partial class FrontWindow : Window
    {
        public FrontWindow()
        {
            InitializeComponent();
        }

        private void Plain_Click(object sender, RoutedEventArgs e)
        {
            MainWindowPlain mwp = new MainWindowPlain();
            mwp.ShowDialog();
        }

        private void PropBagProxyEmit_Click(object sender, RoutedEventArgs e)
        {
            ShowMain("Emit_Proxy");
        }

        private void PropBagExtraMembers_Click(object sender, RoutedEventArgs e)
        {
            ShowMain("Extra_Members");
        }

        private void ShowMain(string packageConfigName)
        {
            MainWindow mw = new MainWindow(packageConfigName);
            mw.ShowDialog();
        }

    }
}
